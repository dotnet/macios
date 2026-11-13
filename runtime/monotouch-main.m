//
// main.m: Basic startup code for Mono on the iPhone
// 
// Authors:
//   Geoff Norton
//   Rolf Bjarne Kvinge <rolf@xamarin.com>
//
// Copyright 2009 Novell, Inc.
// Copyright 2011-2012 Xamarin Inc. 
//

#include <TargetConditionals.h>
#if !TARGET_OS_OSX
#include <UIKit/UIKit.h>
#endif
#include <sys/time.h>
#include <zlib.h>
#include <dlfcn.h>
#include <objc/message.h>
#include <sys/mman.h>

#include "xamarin/xamarin.h"
#include "monotouch-debug.h"

#include "shared.h"
#include "product.h"
#include "runtime-internal.h"
#include "delegates.h"

static char original_working_directory_path [MAXPATHLEN];

const char *
xamarin_get_original_working_directory_path ()
{
	return original_working_directory_path;
}

static uint64_t xamarin_launch_startDate = 0;
static uint64_t xamarin_launch_date = 0;
static int xamarin_launch_time_enabled = -1;

void xamarin_launch_time_print (const char *msg)
{
	if (xamarin_launch_time_enabled == -1) {
		const char *env = getenv ("DEBUG_LAUNCH_TIME");
		xamarin_launch_time_enabled = (env != NULL && env[0] == '1') ? 1 : 0;
	}

	if (!xamarin_launch_time_enabled)
		return;

	uint64_t unow;
	struct timeval now;

	gettimeofday (&now, NULL);
	unow = (uint64_t) now.tv_sec * 1000000ULL + (uint64_t) now.tv_usec;

	if (xamarin_launch_startDate == 0) {
		xamarin_launch_startDate = unow;
		xamarin_launch_date = xamarin_launch_startDate;
	}

	PRINT ("%s: %llu us Total: %llu us", msg, unow - xamarin_launch_date, unow - xamarin_launch_startDate);

	xamarin_launch_date = unow;
}

/*
 * This class will listen for memory warnings and when received, force
 * a full garbage collection.
 */

@interface XamarinGCSupport : NSObject {
}
- (id) init;
@end

@implementation XamarinGCSupport
- (id) init
{
	if (self = [super init]) {
#if !TARGET_OS_OSX
		[[NSNotificationCenter defaultCenter] addObserver:self selector:@selector(memoryWarning:) name:UIApplicationDidReceiveMemoryWarningNotification object:nil];
#endif
	}

	return self;
}

- (void) memoryWarning: (NSNotification *) sender
{
	GCHandle exception_gchandle = INVALID_GCHANDLE;
	xamarin_gc_collect (&exception_gchandle);
	xamarin_process_managed_exception_gchandle (exception_gchandle);
}

@end

/*
 * The main method
 */

int
xamarin_main (int argc, char *argv[], enum XamarinLaunchMode launch_mode)
{
#ifdef DEBUG
	monotouch_start_launch_timer ();
#endif

	// + 1 for the initial "monotouch" +1 for the final NULL = +2.
	// This is not an exact number (it will most likely be lower, since there
	// are other arguments besides --app-arg), but it's a guaranteed and bound
	// upper limit.
	const char **managed_argv = (const char **) alloca (sizeof (char *) * (unsigned long) (argc + 2));
	int managed_argc = 0;

	xamarin_launch_mode = launch_mode;

	memset (managed_argv, 0, sizeof (char*) * (unsigned long) (argc + 2));

#if !(TARGET_OS_OSX || TARGET_OS_MACCATALYST)
	managed_argv [managed_argc++] = "monotouch";
#endif

	XAMARIN_LAUNCH_TIME_PRINT ("Main entered");

	xamarin_setup ();
	XAMARIN_LAUNCH_TIME_PRINT ("MonoTouch setup time");

	MonoAssembly *assembly;
	GCHandle exception_gchandle = NULL;

	// Get the original working directory, and store it somewhere.
	if (getcwd (original_working_directory_path, sizeof (original_working_directory_path)) == NULL)
		original_working_directory_path [0] = '\0';

	// For legacy Xamarin.Mac, we used to chdir to $appdir/Contents/Resources (I'm not sure where this comes from, earliest commit I could find was this: https://github.com/xamarin/maccore/commit/20045dd7f85cb038cea673a9281bb6131711069c)
	// For mobile platforms, we chdir to $appdir
	// In .NET, we always chdir to $appdir, so that we're consistent
	const char *c_bundle_path = xamarin_get_app_bundle_path ();
	chdir (c_bundle_path);

	setenv ("DYLD_BIND_AT_LAUNCH", "1", 1);

	xamarin_bridge_setup ();

	XAMARIN_LAUNCH_TIME_PRINT ("Spin-up time");

	{
		/*
		 * Command line arguments for mobile targets (iOS / tvOS / macOS / Mac Catalyst):
		 * -monodevelop-port=<port>
		 *         The port MonoDevelop is listening on (or we should listen on).
		 *         Overrides whatever any configuration file says.
		 * -debug: 
		 *         Enables debugging (it is enabled by default, but maybe one day in the future
		 *         we can disable it by default so that when the user clicks on the app on the
		 *         device while *not* debugging he doesn't have to wait 2 extra seconds for it
		 *         to start up).
		 * -connection-mode=[wifi|usb|none]:
		 *         Select how to connect (or not) to MonoDevelop. No need to rebuild the
		 *         app anymore when switching between wifi and usb debugging since this
		 *         option overrides whatever the configuration files says. Setting 'none'
		 *         when not debugging or profiling saves 2s on startup (since the app
		 *         won't try to connect to MonoDevelop). If not set the current default is
		 *         to check the configuration files (some time in the future this will be
		 *         changed, so that it defaults to 'none'. This way there will be no 
		 *         2s delay on startup when running it manually by clicking on the icon).
		 * -app-arg=<argument>:
		 *         <argument> will be passed to the app as a command line argument. Can be
		 *         specified multiple times.
		 * -setenv=<key>=<value>
		 *         Set the environment variable <key> to the value <value>
		 *
		 * For desktop targets (macOS / Mac Catalyst) we pass all the command
		 * line arguments directly to the managed Main method.
		 *
		 */
		int i = 0;
		for (i = 0; i < argc; i++) {
#if TARGET_OS_OSX || TARGET_OS_MACCATALYST
			managed_argv [managed_argc++] = argv [i];
#else
			char *arg = argv [i];
			char *name;
			char *value;
			
			if (arg [0] == '-') {
				arg++;
				if (arg [0] == '-')
					arg++;
			} else if (arg [0] == '/') {
				arg++;
			} else {
				continue;
			}
			
			value = arg;
			name = NULL;
			
			while (*++value) {
				if (*value == '=' || *value == ':') {
					name = strndup (arg, (size_t) (value - arg));
					value++;
					break;
				}
			}
			
			if (name == NULL)
				name = strdup (arg);
			
			if (*value == 0)
				value = NULL;

#ifdef DEBUG
			if (!strcmp (name, "monodevelop-port")) {
				if (!value && argc > i + 1)
					value = argv [++i];
				if (value) {
					monotouch_set_monodevelop_port ((int) strtol (value, NULL, 10));
				} else {
					PRINT ("MonoTouch: --%s requires an argument.", name);
				}
			} else if (!strcmp (name, "connection-mode")) {
				if (!value && argc > i + 1)
					value = argv [++i];
				if (value) {
					monotouch_set_connection_mode (value);
				} else {
					PRINT ("MonoTouch: --%s requires an argument.", name);
				}
			} 
#endif /* DEBUG */

			if (!strcmp (name, "app-arg")) {
				if (!value && argc > i + 1)
					value = argv [++i];
				if (value) {
					managed_argv [managed_argc++] = value;
				} else {
					PRINT ("MonoTouch: --%s requires an argument.", name);
				}
			} else if (!strcmp (name, "setenv")) {
				if (!value && argc > i + 1)
					value = argv [++i];
				if (value) {
					char *k = strdup (value);
					char *v = strchr (k, '=');
					if (v) {
						*v = 0;
						v++;
						LOG ("MonoTouch: Setting %s=%s", k, v);
						// arguments comes from mtouch (and developer), i.e. a trusted source
						setenv (k, v, 1);
					}
					free (k);
				} else {
					PRINT ("MonoTouch: --%s requires an argument.", name);
				}
			}
			
			free (name);
#endif // TARGET_OS_OSX || TARGET_OS_MACCATALYST
		}
	}

#ifdef DEBUG
	xamarin_capture_debugging_settings ();
	xamarin_initialize_cocoa_threads (monotouch_configure_debugging);
#else
	xamarin_initialize_cocoa_threads (NULL);
#endif

	xamarin_vm_initialize ();
	xamarin_bridge_initialize ();

	xamarin_initialize ();
	XAMARIN_LAUNCH_TIME_PRINT ("\tmonotouch init time");

	if (xamarin_register_assemblies != NULL)
		xamarin_register_assemblies ();

#if !defined (NATIVEAOT)
	if (xamarin_executable_name) {
		assembly = xamarin_open_and_register (xamarin_executable_name, &exception_gchandle);
		xamarin_process_fatal_exception_gchandle (exception_gchandle, "An exception occurred while opening the main executable");
	} else {
		const char *last_slash = strrchr (argv [0], '/');
		const char *basename = last_slash ? last_slash + 1 : argv [0];
		char *aname = xamarin_strdup_printf ("%s.exe", basename);

		assembly = xamarin_open_and_register (aname, &exception_gchandle);
		xamarin_free (aname);
		xamarin_process_fatal_exception_gchandle (exception_gchandle, "An exception occurred while opening an assembly");
	}

	if (xamarin_supports_dynamic_registration) {
		MonoReflectionAssembly *rassembly = mono_assembly_get_object (mono_domain_get (), assembly);
		xamarin_register_entry_assembly (rassembly, &exception_gchandle);
		xamarin_mono_object_release (&rassembly);
		xamarin_process_fatal_exception_gchandle (exception_gchandle, "An exception occurred while opening the entry assembly");
	}
#else
	assembly = NULL;
	(void)exception_gchandle;
#endif // !defined (NATIVEAOT)

	XAMARIN_LAUNCH_TIME_PRINT ("\tAssembly register time");

	[[[XamarinGCSupport alloc] init] autorelease];

	XAMARIN_LAUNCH_TIME_PRINT ("\tGC defer time");

	XAMARIN_LAUNCH_TIME_PRINT ("Total initialization time");

	int rv = 0;
	switch (launch_mode) {
	case XamarinLaunchModeExtension:
		rv = xamarin_extension_main (argc, argv);
		break;
	case XamarinLaunchModeApp:
		rv = mono_jit_exec (mono_domain_get (), assembly, managed_argc, managed_argv);
		break;
	default:
		xamarin_assertion_message ("Invalid launch mode: %i.", launch_mode);
		break;
	}

	xamarin_mono_object_release (&assembly);
	
	xamarin_release_static_dictionaries ();

	xamarin_bridge_shutdown ();

	return rv;
}
