// vim: set filetype=c :
//
// mono-runtime.m: Header file to allow dynamic loading of Mono to
// be transparent from code including the mono-runtime.h header.
//
// Authors:
//   Rolf Bjarne Kvinge <rolf@xamarin.com>
//   Aaron Bockover <abock@xamarin.com>
//
// Copyright 2013-2014 Xamarin Inc.
//

#ifdef DYNAMIC_MONO_RUNTIME

#include <stdlib.h>
#include <stdio.h>
#include <stdint.h>
#include <string.h>
#include <dlfcn.h>

#include "xamarin/main.h"
#include "xamarin/mono-runtime.h"
#include "xamarin/runtime.h"
#include "product.h"
#include "runtime-internal.h"

typedef MonoClass * (* mono_class_from_name_def) (MonoImage * image, const char * name_space, const char * name);
typedef MonoMethod * (* mono_class_get_method_from_name_def) (MonoClass * klass, const char * name, int param_count);
typedef mono_bool (* mono_class_is_assignable_from_def) (MonoClass * klass, MonoClass * oklass);
typedef MonoClass * (* mono_class_from_mono_type_def) (MonoType * type);
typedef mono_bool (* mono_class_is_delegate_def) (MonoClass * klass);
typedef MonoClass * (* mono_class_get_element_class_def) (MonoClass * klass);
typedef const char * (* mono_class_get_namespace_def) (MonoClass * klass);
typedef const char * (* mono_class_get_name_def) (MonoClass * klass);
typedef mono_bool (* mono_class_is_subclass_of_def) (MonoClass * klass, MonoClass * klassc, mono_bool check_interfaces);
typedef mono_bool (* mono_class_is_valuetype_def) (MonoClass * klass);
typedef mono_bool (* mono_class_is_enum_def) (MonoClass * klass);
typedef MonoType * (* mono_class_enum_basetype_def) (MonoClass * klass);
typedef int32_t (* mono_class_value_size_def) (MonoClass * klass, uint32_t * align);
typedef MonoType * (* mono_class_get_type_def) (MonoClass * klass);
typedef gboolean (* mono_class_is_nullable_def) (MonoClass * klass);
typedef MonoClass * (* mono_class_get_nullable_param_def) (MonoClass * klass);
typedef char * (* mono_method_full_name_def) (MonoMethod * method, mono_bool signature);
typedef MonoObject * (* mono_runtime_invoke_def) (MonoMethod * method, void * obj, void ** params, MonoObject ** exc);
typedef GCHandle (* mono_gchandle_new_v2_def) (MonoObject * obj, mono_bool pinned);
typedef MonoObject * (* mono_gchandle_get_target_v2_def) (GCHandle gchandle);
typedef void (* mono_gchandle_free_v2_def) (GCHandle gchandle);
typedef GCHandle (* mono_gchandle_new_weakref_v2_def) (MonoObject * obj, mono_bool track_resurrection);
typedef void (* mono_raise_exception_def) (MonoException * ex);
typedef void (* mono_unhandled_exception_def) (MonoObject * ex);
typedef char* (* mono_array_addr_with_size_def) (MonoArray * array, int size, uintptr_t idx);
typedef MonoString * (* mono_string_new_def) (MonoDomain * domain, const char * text);
typedef MonoArray * (* mono_array_new_def) (MonoDomain * domain, MonoClass * eclass, uintptr_t n);
typedef void * (* mono_object_unbox_def) (MonoObject * obj);
typedef char * (* mono_string_to_utf8_def) (MonoString * string_obj);
typedef uintptr_t (* mono_array_length_def) (MonoArray * array);
typedef MonoObject * (* mono_object_isinst_def) (MonoObject * obj, MonoClass * klass);
typedef MonoClass * (* mono_object_get_class_def) (MonoObject * obj);
typedef MonoObject * (* mono_value_box_def) (MonoDomain * domain, MonoClass * klass, void * val);
typedef void (* mono_gc_wbarrier_set_arrayref_def) (MonoArray * arr, void * slot_ptr, MonoObject * value);
typedef void (* mono_profiler_install_def) (MonoProfiler * prof, MonoProfileFunc shutdown_callback);
typedef void (* mono_profiler_install_gc_def) (MonoProfileGCFunc callback, MonoProfileGCResizeFunc heap_resize_callback);
typedef void (* mono_profiler_load_def) (const char * desc);
typedef mono_bool (* mono_thread_is_foreign_def) (MonoThread * thread);
typedef MonoThread *  (* mono_thread_current_def) (void);
typedef MonoThread * (* mono_thread_attach_def) (MonoDomain * domain);
typedef mono_bool (* mono_thread_detach_if_exiting_def) (void);
typedef void (* mono_runtime_set_pending_exception_def) (MonoException * exc, mono_bool overwrite);
typedef void (* mono_set_assemblies_path_def) (const char * path);
typedef MonoAssembly * (* mono_assembly_open_def) (const char * filename, MonoImageOpenStatus * status);
typedef MonoImage * (* mono_assembly_get_image_def) (MonoAssembly * assembly);
typedef MonoAssemblyName * (* mono_assembly_name_new_def) (const char * name);
typedef void (* mono_assembly_name_free_def) (MonoAssemblyName * aname);
typedef MonoAssembly * (* mono_assembly_loaded_def) (MonoAssemblyName * aname);
typedef void (* mono_register_machine_config_def) (const char * config_xml);
typedef void (* mono_set_dirs_def) (const char * assembly_dir, const char * config_dir);
typedef const char * (* mono_assembly_name_get_name_def) (MonoAssemblyName * aname);
typedef const char * (* mono_assembly_name_get_culture_def) (MonoAssemblyName * aname);
typedef void (* mono_install_assembly_preload_hook_def) (MonoAssemblyPreLoadFunc func, void * user_data);
typedef MonoAssemblyName * (* mono_assembly_get_name_def) (MonoAssembly * assembly);
typedef void (* mono_add_internal_call_def) (const char * name, const void * method);
typedef MonoMethodSignature * (* mono_method_signature_def) (MonoMethod * method);
typedef MonoClass * (* mono_method_get_class_def) (MonoMethod * method);
typedef MonoDomain * (* mono_domain_get_def) (void);
typedef MonoClass * (* mono_get_intptr_class_def) (void);
typedef MonoClass * (* mono_get_string_class_def) (void);
typedef MonoImage * (* mono_get_corlib_def) (void);
typedef MonoClass * (* mono_get_array_class_def) (void);
typedef MonoDomain * (* mono_get_root_domain_def) (void);
typedef MonoReflectionAssembly * (* mono_assembly_get_object_def) (MonoDomain * domain, MonoAssembly * assembly);
typedef MonoReflectionMethod * (* mono_method_get_object_def) (MonoDomain * domain, MonoMethod * method, MonoClass * refclass);
typedef MonoReflectionType * (* mono_type_get_object_def) (MonoDomain * domain, MonoType * type);
typedef MonoType * (* mono_reflection_type_get_type_def) (MonoReflectionType * reftype);
typedef MonoType * (* mono_signature_get_params_def) (MonoMethodSignature * sig, void ** iter);
typedef mono_bool (* mono_type_is_byref_def) (MonoType * type);
typedef MonoType * (* mono_signature_get_return_type_def) (MonoMethodSignature * sig);
typedef void (* mono_debug_init_def) (MonoDebugFormat format);
typedef mono_bool (* mono_is_debugger_attached_def) (void);
typedef void (* mono_config_parse_memory_def) (const char * buffer);
typedef MonoGHashTable * (* mono_g_hash_table_new_type_def) (GHashFunc hash_func, GEqualFunc key_equal_func, MonoGHashGCType type);
typedef gpointer (* mono_g_hash_table_lookup_def) (MonoGHashTable * hash, gconstpointer key);
typedef void (* mono_g_hash_table_insert_def) (MonoGHashTable * hash, gpointer k, gpointer v);
typedef MonoException * (* mono_exception_from_name_msg_def) (MonoImage * image, const char * name_space, const char * name, const char * msg);
typedef MonoException * (* mono_get_exception_out_of_memory_def) (void);
typedef void (* mono_gc_register_finalizer_callbacks_def) (MonoGCFinalizerCallbacks * callbacks);
typedef void (* mono_gc_toggleref_add_def) (MonoObject * object, mono_bool strong_ref);
typedef void (* mono_gc_toggleref_register_callback_def) (MonoToggleRefCallback process_toggleref);
typedef void (* mono_free_def) (void * ptr);
typedef char * (* mono_get_runtime_build_info_def) (void);
typedef MonoDomain * (* mono_jit_init_def) (const char * file);
typedef MonoDomain * (* mono_jit_cleanup_def) (MonoDomain * domain);
typedef int (* mono_jit_exec_def) (MonoDomain * domain, MonoAssembly * assembly, int argc, const char** argv);
typedef void (* mono_jit_set_aot_mode_def) (MonoAotMode mode);
typedef void (* mono_set_signal_chaining_def) (mono_bool chain_signals);
typedef void (* mono_set_crash_chaining_def) (mono_bool chain_signals);
typedef void (* mono_jit_set_trace_options_def) (const char * option);
typedef void* (* mono_jit_thread_attach_def) (MonoDomain * domain);
typedef void (* mono_install_unhandled_exception_hook_def) (MonoUnhandledExceptionFunc func, gpointer user_data);
typedef int (* mono_main_def) (int argc, char ** argv);
typedef void (* mono_install_load_aot_data_hook_def) (MonoLoadAotDataFunc load_func, MonoFreeAotDataFunc free_func, gpointer user_data);
typedef gboolean (* mini_parse_debug_option_def) (const char * option);
typedef void (* mono_trace_set_log_handler_def) (MonoLogCallback callback, void * user_data);
typedef void (* mono_trace_set_print_handler_def) (MonoPrintCallback callback);
typedef void (* mono_trace_set_printerr_handler_def) (MonoPrintCallback callback);
typedef void* (* mono_threads_enter_gc_unsafe_region_def) (void ** stackdata);
typedef void (* mono_threads_exit_gc_unsafe_region_def) (void * cookie, void ** stackdata);
typedef void* (* mono_threads_enter_gc_safe_region_def) (void ** stackdata);
typedef void (* mono_threads_exit_gc_safe_region_def) (void * cookie, void ** stackdata);
typedef void (* mono_threads_assert_gc_safe_region_def) (void);
typedef void (* mono_threads_assert_gc_unsafe_region_def) (void);
typedef void (* mono_threads_assert_gc_starting_region_def) (void);
typedef void* (* mono_thread_info_current_unchecked_def) (void);
typedef void (* mono_install_ftnptr_eh_callback_def) (MonoFtnPtrEHCallback callback);
typedef void (* mono_debugger_agent_parse_options_def) (const char * options);
typedef gboolean (* mono_debugger_agent_transport_handshake_def) (void);
typedef void (* mono_debugger_agent_register_transport_def) (DebuggerTransport * trans);
typedef void (* mono_debugger_agent_unhandled_exception_def) (MonoException * e);
typedef int (* monovm_initialize_def) (int propertyCount, const char ** propertyKeys, const char ** propertyValues);
typedef int (* monovm_runtimeconfig_initialize_def) (MonovmRuntimeConfigArguments * arg, MonovmRuntimeConfigArgumentsCleanup cleanup_fn, void * user_data);

mono_class_from_name_def mono_class_from_name_func = NULL;
mono_class_get_method_from_name_def mono_class_get_method_from_name_func = NULL;
mono_class_is_assignable_from_def mono_class_is_assignable_from_func = NULL;
mono_class_from_mono_type_def mono_class_from_mono_type_func = NULL;
mono_class_is_delegate_def mono_class_is_delegate_func = NULL;
mono_class_get_element_class_def mono_class_get_element_class_func = NULL;
mono_class_get_namespace_def mono_class_get_namespace_func = NULL;
mono_class_get_name_def mono_class_get_name_func = NULL;
mono_class_is_subclass_of_def mono_class_is_subclass_of_func = NULL;
mono_class_is_valuetype_def mono_class_is_valuetype_func = NULL;
mono_class_is_enum_def mono_class_is_enum_func = NULL;
mono_class_enum_basetype_def mono_class_enum_basetype_func = NULL;
mono_class_value_size_def mono_class_value_size_func = NULL;
mono_class_get_type_def mono_class_get_type_func = NULL;
mono_class_is_nullable_def mono_class_is_nullable_func = NULL;
mono_class_get_nullable_param_def mono_class_get_nullable_param_func = NULL;
mono_method_full_name_def mono_method_full_name_func = NULL;
mono_runtime_invoke_def mono_runtime_invoke_func = NULL;
mono_gchandle_new_v2_def mono_gchandle_new_v2_func = NULL;
mono_gchandle_get_target_v2_def mono_gchandle_get_target_v2_func = NULL;
mono_gchandle_free_v2_def mono_gchandle_free_v2_func = NULL;
mono_gchandle_new_weakref_v2_def mono_gchandle_new_weakref_v2_func = NULL;
mono_raise_exception_def mono_raise_exception_func = NULL;
mono_unhandled_exception_def mono_unhandled_exception_func = NULL;
mono_array_addr_with_size_def mono_array_addr_with_size_func = NULL;
mono_string_new_def mono_string_new_func = NULL;
mono_array_new_def mono_array_new_func = NULL;
mono_object_unbox_def mono_object_unbox_func = NULL;
mono_string_to_utf8_def mono_string_to_utf8_func = NULL;
mono_array_length_def mono_array_length_func = NULL;
mono_object_isinst_def mono_object_isinst_func = NULL;
mono_object_get_class_def mono_object_get_class_func = NULL;
mono_value_box_def mono_value_box_func = NULL;
mono_gc_wbarrier_set_arrayref_def mono_gc_wbarrier_set_arrayref_func = NULL;
mono_profiler_install_def mono_profiler_install_func = NULL;
mono_profiler_install_gc_def mono_profiler_install_gc_func = NULL;
mono_profiler_load_def mono_profiler_load_func = NULL;
mono_thread_is_foreign_def mono_thread_is_foreign_func = NULL;
mono_thread_current_def mono_thread_current_func = NULL;
mono_thread_attach_def mono_thread_attach_func = NULL;
mono_thread_detach_if_exiting_def mono_thread_detach_if_exiting_func = NULL;
mono_runtime_set_pending_exception_def mono_runtime_set_pending_exception_func = NULL;
mono_set_assemblies_path_def mono_set_assemblies_path_func = NULL;
mono_assembly_open_def mono_assembly_open_func = NULL;
mono_assembly_get_image_def mono_assembly_get_image_func = NULL;
mono_assembly_name_new_def mono_assembly_name_new_func = NULL;
mono_assembly_name_free_def mono_assembly_name_free_func = NULL;
mono_assembly_loaded_def mono_assembly_loaded_func = NULL;
mono_register_machine_config_def mono_register_machine_config_func = NULL;
mono_set_dirs_def mono_set_dirs_func = NULL;
mono_assembly_name_get_name_def mono_assembly_name_get_name_func = NULL;
mono_assembly_name_get_culture_def mono_assembly_name_get_culture_func = NULL;
mono_install_assembly_preload_hook_def mono_install_assembly_preload_hook_func = NULL;
mono_assembly_get_name_def mono_assembly_get_name_func = NULL;
mono_add_internal_call_def mono_add_internal_call_func = NULL;
mono_method_signature_def mono_method_signature_func = NULL;
mono_method_get_class_def mono_method_get_class_func = NULL;
mono_domain_get_def mono_domain_get_func = NULL;
mono_get_intptr_class_def mono_get_intptr_class_func = NULL;
mono_get_string_class_def mono_get_string_class_func = NULL;
mono_get_corlib_def mono_get_corlib_func = NULL;
mono_get_array_class_def mono_get_array_class_func = NULL;
mono_get_root_domain_def mono_get_root_domain_func = NULL;
mono_assembly_get_object_def mono_assembly_get_object_func = NULL;
mono_method_get_object_def mono_method_get_object_func = NULL;
mono_type_get_object_def mono_type_get_object_func = NULL;
mono_reflection_type_get_type_def mono_reflection_type_get_type_func = NULL;
mono_signature_get_params_def mono_signature_get_params_func = NULL;
mono_type_is_byref_def mono_type_is_byref_func = NULL;
mono_signature_get_return_type_def mono_signature_get_return_type_func = NULL;
mono_debug_init_def mono_debug_init_func = NULL;
mono_is_debugger_attached_def mono_is_debugger_attached_func = NULL;
mono_config_parse_memory_def mono_config_parse_memory_func = NULL;
mono_g_hash_table_new_type_def mono_g_hash_table_new_type_func = NULL;
mono_g_hash_table_lookup_def mono_g_hash_table_lookup_func = NULL;
mono_g_hash_table_insert_def mono_g_hash_table_insert_func = NULL;
mono_exception_from_name_msg_def mono_exception_from_name_msg_func = NULL;
mono_get_exception_out_of_memory_def mono_get_exception_out_of_memory_func = NULL;
mono_gc_register_finalizer_callbacks_def mono_gc_register_finalizer_callbacks_func = NULL;
mono_gc_toggleref_add_def mono_gc_toggleref_add_func = NULL;
mono_gc_toggleref_register_callback_def mono_gc_toggleref_register_callback_func = NULL;
mono_free_def mono_free_func = NULL;
mono_get_runtime_build_info_def mono_get_runtime_build_info_func = NULL;
mono_jit_init_def mono_jit_init_func = NULL;
mono_jit_cleanup_def mono_jit_cleanup_func = NULL;
mono_jit_exec_def mono_jit_exec_func = NULL;
mono_jit_set_aot_mode_def mono_jit_set_aot_mode_func = NULL;
mono_set_signal_chaining_def mono_set_signal_chaining_func = NULL;
mono_set_crash_chaining_def mono_set_crash_chaining_func = NULL;
mono_jit_set_trace_options_def mono_jit_set_trace_options_func = NULL;
mono_jit_thread_attach_def mono_jit_thread_attach_func = NULL;
mono_install_unhandled_exception_hook_def mono_install_unhandled_exception_hook_func = NULL;
mono_main_def mono_main_func = NULL;
mono_install_load_aot_data_hook_def mono_install_load_aot_data_hook_func = NULL;
mini_parse_debug_option_def mini_parse_debug_option_func = NULL;
mono_trace_set_log_handler_def mono_trace_set_log_handler_func = NULL;
mono_trace_set_print_handler_def mono_trace_set_print_handler_func = NULL;
mono_trace_set_printerr_handler_def mono_trace_set_printerr_handler_func = NULL;
mono_threads_enter_gc_unsafe_region_def mono_threads_enter_gc_unsafe_region_func = NULL;
mono_threads_exit_gc_unsafe_region_def mono_threads_exit_gc_unsafe_region_func = NULL;
mono_threads_enter_gc_safe_region_def mono_threads_enter_gc_safe_region_func = NULL;
mono_threads_exit_gc_safe_region_def mono_threads_exit_gc_safe_region_func = NULL;
mono_threads_assert_gc_safe_region_def mono_threads_assert_gc_safe_region_func = NULL;
mono_threads_assert_gc_unsafe_region_def mono_threads_assert_gc_unsafe_region_func = NULL;
mono_threads_assert_gc_starting_region_def mono_threads_assert_gc_starting_region_func = NULL;
mono_thread_info_current_unchecked_def mono_thread_info_current_unchecked_func = NULL;
mono_install_ftnptr_eh_callback_def mono_install_ftnptr_eh_callback_func = NULL;
mono_debugger_agent_parse_options_def mono_debugger_agent_parse_options_func = NULL;
mono_debugger_agent_transport_handshake_def mono_debugger_agent_transport_handshake_func = NULL;
mono_debugger_agent_register_transport_def mono_debugger_agent_register_transport_func = NULL;
mono_debugger_agent_unhandled_exception_def mono_debugger_agent_unhandled_exception_func = NULL;
monovm_initialize_def monovm_initialize_func = NULL;
monovm_runtimeconfig_initialize_def monovm_runtimeconfig_initialize_func = NULL;

char *
xamarin_get_mono_runtime_build_info ()
{
	if (!mono_get_runtime_build_info_func)
		return NULL;
	return mono_get_runtime_build_info_func ();
}

static char xamarin_dynamic_runtime_initialized = 0;

const char *
xamarin_initialize_dynamic_runtime (const char *mono_runtime_prefix)
{
	if (xamarin_dynamic_runtime_initialized)
		return NULL;

	void *libmono = NULL;
	const char *errmsg = NULL;

	// Check if we already have mono available.
	// RTLD_DEFAULT is the catch-all, but it's slow (according to the dlsym man page),
	// so try MAIN_ONLY and SELF first to see if we can optimize a bit.
	if (dlsym (RTLD_MAIN_ONLY, "mono_get_runtime_build_info")) {
		libmono = RTLD_MAIN_ONLY;
	} else if (dlsym (RTLD_SELF, "mono_get_runtime_build_info")) {
		libmono = RTLD_SELF;
	} else if (dlsym (RTLD_DEFAULT, "mono_get_runtime_build_info")) {
		libmono = RTLD_DEFAULT;
	} else if (mono_runtime_prefix != NULL) {
		NSString *dylib = [[NSString stringWithUTF8String: mono_runtime_prefix] stringByAppendingPathComponent: @"/lib/libmonosgen-2.0.dylib"];
		libmono = dlopen ([dylib UTF8String], RTLD_LAZY);
	}

	if (libmono == NULL) {
		const char *dlopen_err = dlerror ();
		if (dlopen_err != NULL)
			PRINT (PRODUCT ": dlopen error: %s", dlopen_err);
		return "This application requires the Mono framework.";
	}


#if !defined (CORECLR_RUNTIME)
	mono_class_from_name_func = (mono_class_from_name_def) dlsym (libmono, "mono_class_from_name");
	if (mono_class_from_name_func == NULL) {
		fprintf (stderr, "Could not load mono_class_from_name\n");
		errmsg = "Failed to load the Mono framework.";
	}
#endif

#if !defined (CORECLR_RUNTIME)
	mono_class_get_method_from_name_func = (mono_class_get_method_from_name_def) dlsym (libmono, "mono_class_get_method_from_name");
	if (mono_class_get_method_from_name_func == NULL) {
		fprintf (stderr, "Could not load mono_class_get_method_from_name\n");
		errmsg = "Failed to load the Mono framework.";
	}
#endif

#if !defined (CORECLR_RUNTIME)
	mono_class_is_assignable_from_func = (mono_class_is_assignable_from_def) dlsym (libmono, "mono_class_is_assignable_from");
	if (mono_class_is_assignable_from_func == NULL) {
		fprintf (stderr, "Could not load mono_class_is_assignable_from\n");
		errmsg = "Failed to load the Mono framework.";
	}
#endif

	mono_class_from_mono_type_func = (mono_class_from_mono_type_def) dlsym (libmono, "mono_class_from_mono_type");
	if (mono_class_from_mono_type_func == NULL) {
		fprintf (stderr, "Could not load mono_class_from_mono_type\n");
		errmsg = "Failed to load the Mono framework.";
	}

	mono_class_is_delegate_func = (mono_class_is_delegate_def) dlsym (libmono, "mono_class_is_delegate");
	if (mono_class_is_delegate_func == NULL) {
		fprintf (stderr, "Could not load mono_class_is_delegate\n");
		errmsg = "Failed to load the Mono framework.";
	}

	mono_class_get_element_class_func = (mono_class_get_element_class_def) dlsym (libmono, "mono_class_get_element_class");
	if (mono_class_get_element_class_func == NULL) {
		fprintf (stderr, "Could not load mono_class_get_element_class\n");
		errmsg = "Failed to load the Mono framework.";
	}

	mono_class_get_namespace_func = (mono_class_get_namespace_def) dlsym (libmono, "mono_class_get_namespace");
	if (mono_class_get_namespace_func == NULL) {
		fprintf (stderr, "Could not load mono_class_get_namespace\n");
		errmsg = "Failed to load the Mono framework.";
	}

	mono_class_get_name_func = (mono_class_get_name_def) dlsym (libmono, "mono_class_get_name");
	if (mono_class_get_name_func == NULL) {
		fprintf (stderr, "Could not load mono_class_get_name\n");
		errmsg = "Failed to load the Mono framework.";
	}

#if !defined (CORECLR_RUNTIME)
	mono_class_is_subclass_of_func = (mono_class_is_subclass_of_def) dlsym (libmono, "mono_class_is_subclass_of");
	if (mono_class_is_subclass_of_func == NULL) {
		fprintf (stderr, "Could not load mono_class_is_subclass_of\n");
		errmsg = "Failed to load the Mono framework.";
	}
#endif

	mono_class_is_valuetype_func = (mono_class_is_valuetype_def) dlsym (libmono, "mono_class_is_valuetype");
	if (mono_class_is_valuetype_func == NULL) {
		fprintf (stderr, "Could not load mono_class_is_valuetype\n");
		errmsg = "Failed to load the Mono framework.";
	}

	mono_class_is_enum_func = (mono_class_is_enum_def) dlsym (libmono, "mono_class_is_enum");
	if (mono_class_is_enum_func == NULL) {
		fprintf (stderr, "Could not load mono_class_is_enum\n");
		errmsg = "Failed to load the Mono framework.";
	}

	mono_class_enum_basetype_func = (mono_class_enum_basetype_def) dlsym (libmono, "mono_class_enum_basetype");
	if (mono_class_enum_basetype_func == NULL) {
		fprintf (stderr, "Could not load mono_class_enum_basetype\n");
		errmsg = "Failed to load the Mono framework.";
	}

	mono_class_value_size_func = (mono_class_value_size_def) dlsym (libmono, "mono_class_value_size");
	if (mono_class_value_size_func == NULL) {
		fprintf (stderr, "Could not load mono_class_value_size\n");
		errmsg = "Failed to load the Mono framework.";
	}

	mono_class_get_type_func = (mono_class_get_type_def) dlsym (libmono, "mono_class_get_type");
	if (mono_class_get_type_func == NULL) {
		fprintf (stderr, "Could not load mono_class_get_type\n");
		errmsg = "Failed to load the Mono framework.";
	}

	mono_class_is_nullable_func = (mono_class_is_nullable_def) dlsym (libmono, "mono_class_is_nullable");

	mono_class_get_nullable_param_func = (mono_class_get_nullable_param_def) dlsym (libmono, "mono_class_get_nullable_param");

	mono_method_full_name_func = (mono_method_full_name_def) dlsym (libmono, "mono_method_full_name");
	if (mono_method_full_name_func == NULL) {
		fprintf (stderr, "Could not load mono_method_full_name\n");
		errmsg = "Failed to load the Mono framework.";
	}

	mono_runtime_invoke_func = (mono_runtime_invoke_def) dlsym (libmono, "mono_runtime_invoke");
	if (mono_runtime_invoke_func == NULL) {
		fprintf (stderr, "Could not load mono_runtime_invoke\n");
		errmsg = "Failed to load the Mono framework.";
	}

#if !defined (CORECLR_RUNTIME)
	mono_gchandle_new_v2_func = (mono_gchandle_new_v2_def) dlsym (libmono, "mono_gchandle_new_v2");
	if (mono_gchandle_new_v2_func == NULL) {
		fprintf (stderr, "Could not load mono_gchandle_new_v2\n");
		errmsg = "Failed to load the Mono framework.";
	}
#endif

#if !defined (CORECLR_RUNTIME)
	mono_gchandle_get_target_v2_func = (mono_gchandle_get_target_v2_def) dlsym (libmono, "mono_gchandle_get_target_v2");
	if (mono_gchandle_get_target_v2_func == NULL) {
		fprintf (stderr, "Could not load mono_gchandle_get_target_v2\n");
		errmsg = "Failed to load the Mono framework.";
	}
#endif

#if !defined (CORECLR_RUNTIME)
	mono_gchandle_free_v2_func = (mono_gchandle_free_v2_def) dlsym (libmono, "mono_gchandle_free_v2");
	if (mono_gchandle_free_v2_func == NULL) {
		fprintf (stderr, "Could not load mono_gchandle_free_v2\n");
		errmsg = "Failed to load the Mono framework.";
	}
#endif

#if !defined (CORECLR_RUNTIME)
	mono_gchandle_new_weakref_v2_func = (mono_gchandle_new_weakref_v2_def) dlsym (libmono, "mono_gchandle_new_weakref_v2");
	if (mono_gchandle_new_weakref_v2_func == NULL) {
		fprintf (stderr, "Could not load mono_gchandle_new_weakref_v2\n");
		errmsg = "Failed to load the Mono framework.";
	}
#endif

#if !defined (CORECLR_RUNTIME)
	mono_raise_exception_func = (mono_raise_exception_def) dlsym (libmono, "mono_raise_exception");
	if (mono_raise_exception_func == NULL) {
		fprintf (stderr, "Could not load mono_raise_exception\n");
		errmsg = "Failed to load the Mono framework.";
	}
#endif

#if !defined (CORECLR_RUNTIME)
	mono_unhandled_exception_func = (mono_unhandled_exception_def) dlsym (libmono, "mono_unhandled_exception");
	if (mono_unhandled_exception_func == NULL) {
		fprintf (stderr, "Could not load mono_unhandled_exception\n");
		errmsg = "Failed to load the Mono framework.";
	}
#endif

#if !defined (CORECLR_RUNTIME)
	mono_array_addr_with_size_func = (mono_array_addr_with_size_def) dlsym (libmono, "mono_array_addr_with_size");
	if (mono_array_addr_with_size_func == NULL) {
		fprintf (stderr, "Could not load mono_array_addr_with_size\n");
		errmsg = "Failed to load the Mono framework.";
	}
#endif

	mono_string_new_func = (mono_string_new_def) dlsym (libmono, "mono_string_new");
	if (mono_string_new_func == NULL) {
		fprintf (stderr, "Could not load mono_string_new\n");
		errmsg = "Failed to load the Mono framework.";
	}

	mono_array_new_func = (mono_array_new_def) dlsym (libmono, "mono_array_new");
	if (mono_array_new_func == NULL) {
		fprintf (stderr, "Could not load mono_array_new\n");
		errmsg = "Failed to load the Mono framework.";
	}

	mono_object_unbox_func = (mono_object_unbox_def) dlsym (libmono, "mono_object_unbox");
	if (mono_object_unbox_func == NULL) {
		fprintf (stderr, "Could not load mono_object_unbox\n");
		errmsg = "Failed to load the Mono framework.";
	}

	mono_string_to_utf8_func = (mono_string_to_utf8_def) dlsym (libmono, "mono_string_to_utf8");
	if (mono_string_to_utf8_func == NULL) {
		fprintf (stderr, "Could not load mono_string_to_utf8\n");
		errmsg = "Failed to load the Mono framework.";
	}

	mono_array_length_func = (mono_array_length_def) dlsym (libmono, "mono_array_length");
	if (mono_array_length_func == NULL) {
		fprintf (stderr, "Could not load mono_array_length\n");
		errmsg = "Failed to load the Mono framework.";
	}

	mono_object_isinst_func = (mono_object_isinst_def) dlsym (libmono, "mono_object_isinst");
	if (mono_object_isinst_func == NULL) {
		fprintf (stderr, "Could not load mono_object_isinst\n");
		errmsg = "Failed to load the Mono framework.";
	}

	mono_object_get_class_func = (mono_object_get_class_def) dlsym (libmono, "mono_object_get_class");
	if (mono_object_get_class_func == NULL) {
		fprintf (stderr, "Could not load mono_object_get_class\n");
		errmsg = "Failed to load the Mono framework.";
	}

	mono_value_box_func = (mono_value_box_def) dlsym (libmono, "mono_value_box");
	if (mono_value_box_func == NULL) {
		fprintf (stderr, "Could not load mono_value_box\n");
		errmsg = "Failed to load the Mono framework.";
	}

#if !defined (CORECLR_RUNTIME)
	mono_gc_wbarrier_set_arrayref_func = (mono_gc_wbarrier_set_arrayref_def) dlsym (libmono, "mono_gc_wbarrier_set_arrayref");
	if (mono_gc_wbarrier_set_arrayref_func == NULL) {
		fprintf (stderr, "Could not load mono_gc_wbarrier_set_arrayref\n");
		errmsg = "Failed to load the Mono framework.";
	}
#endif

#if !defined (CORECLR_RUNTIME)
	mono_profiler_install_func = (mono_profiler_install_def) dlsym (libmono, "mono_profiler_install");
	if (mono_profiler_install_func == NULL) {
		fprintf (stderr, "Could not load mono_profiler_install\n");
		errmsg = "Failed to load the Mono framework.";
	}
#endif

#if !defined (CORECLR_RUNTIME)
	mono_profiler_install_gc_func = (mono_profiler_install_gc_def) dlsym (libmono, "mono_profiler_install_gc");
	if (mono_profiler_install_gc_func == NULL) {
		fprintf (stderr, "Could not load mono_profiler_install_gc\n");
		errmsg = "Failed to load the Mono framework.";
	}
#endif

#if !defined (CORECLR_RUNTIME)
	mono_profiler_load_func = (mono_profiler_load_def) dlsym (libmono, "mono_profiler_load");
	if (mono_profiler_load_func == NULL) {
		fprintf (stderr, "Could not load mono_profiler_load\n");
		errmsg = "Failed to load the Mono framework.";
	}
#endif

#if !defined (CORECLR_RUNTIME)
	mono_thread_is_foreign_func = (mono_thread_is_foreign_def) dlsym (libmono, "mono_thread_is_foreign");
	if (mono_thread_is_foreign_func == NULL) {
		fprintf (stderr, "Could not load mono_thread_is_foreign\n");
		errmsg = "Failed to load the Mono framework.";
	}
#endif

#if !defined (CORECLR_RUNTIME)
	mono_thread_current_func = (mono_thread_current_def) dlsym (libmono, "mono_thread_current");
	if (mono_thread_current_func == NULL) {
		fprintf (stderr, "Could not load mono_thread_current\n");
		errmsg = "Failed to load the Mono framework.";
	}
#endif

#if !defined (CORECLR_RUNTIME)
	mono_thread_attach_func = (mono_thread_attach_def) dlsym (libmono, "mono_thread_attach");
	if (mono_thread_attach_func == NULL) {
		fprintf (stderr, "Could not load mono_thread_attach\n");
		errmsg = "Failed to load the Mono framework.";
	}
#endif

	mono_thread_detach_if_exiting_func = (mono_thread_detach_if_exiting_def) dlsym (libmono, "mono_thread_detach_if_exiting");
	if (mono_thread_detach_if_exiting_func == NULL) {
		fprintf (stderr, "Could not load mono_thread_detach_if_exiting\n");
		errmsg = "Failed to load the Mono framework.";
	}

	mono_runtime_set_pending_exception_func = (mono_runtime_set_pending_exception_def) dlsym (libmono, "mono_runtime_set_pending_exception");

#if !defined (CORECLR_RUNTIME)
	mono_set_assemblies_path_func = (mono_set_assemblies_path_def) dlsym (libmono, "mono_set_assemblies_path");
	if (mono_set_assemblies_path_func == NULL) {
		fprintf (stderr, "Could not load mono_set_assemblies_path\n");
		errmsg = "Failed to load the Mono framework.";
	}
#endif

	mono_assembly_open_func = (mono_assembly_open_def) dlsym (libmono, "mono_assembly_open");
	if (mono_assembly_open_func == NULL) {
		fprintf (stderr, "Could not load mono_assembly_open\n");
		errmsg = "Failed to load the Mono framework.";
	}

#if !defined (CORECLR_RUNTIME)
	mono_assembly_get_image_func = (mono_assembly_get_image_def) dlsym (libmono, "mono_assembly_get_image");
	if (mono_assembly_get_image_func == NULL) {
		fprintf (stderr, "Could not load mono_assembly_get_image\n");
		errmsg = "Failed to load the Mono framework.";
	}
#endif

#if !defined (CORECLR_RUNTIME)
	mono_assembly_name_new_func = (mono_assembly_name_new_def) dlsym (libmono, "mono_assembly_name_new");
	if (mono_assembly_name_new_func == NULL) {
		fprintf (stderr, "Could not load mono_assembly_name_new\n");
		errmsg = "Failed to load the Mono framework.";
	}
#endif

#if !defined (CORECLR_RUNTIME)
	mono_assembly_name_free_func = (mono_assembly_name_free_def) dlsym (libmono, "mono_assembly_name_free");
	if (mono_assembly_name_free_func == NULL) {
		fprintf (stderr, "Could not load mono_assembly_name_free\n");
		errmsg = "Failed to load the Mono framework.";
	}
#endif

#if !defined (CORECLR_RUNTIME)
	mono_assembly_loaded_func = (mono_assembly_loaded_def) dlsym (libmono, "mono_assembly_loaded");
	if (mono_assembly_loaded_func == NULL) {
		fprintf (stderr, "Could not load mono_assembly_loaded\n");
		errmsg = "Failed to load the Mono framework.";
	}
#endif

#if !defined (CORECLR_RUNTIME)
	mono_register_machine_config_func = (mono_register_machine_config_def) dlsym (libmono, "mono_register_machine_config");
	if (mono_register_machine_config_func == NULL) {
		fprintf (stderr, "Could not load mono_register_machine_config\n");
		errmsg = "Failed to load the Mono framework.";
	}
#endif

#if !defined (CORECLR_RUNTIME)
	mono_set_dirs_func = (mono_set_dirs_def) dlsym (libmono, "mono_set_dirs");
	if (mono_set_dirs_func == NULL) {
		fprintf (stderr, "Could not load mono_set_dirs\n");
		errmsg = "Failed to load the Mono framework.";
	}
#endif

#if !defined (CORECLR_RUNTIME)
	mono_assembly_name_get_name_func = (mono_assembly_name_get_name_def) dlsym (libmono, "mono_assembly_name_get_name");
	if (mono_assembly_name_get_name_func == NULL) {
		fprintf (stderr, "Could not load mono_assembly_name_get_name\n");
		errmsg = "Failed to load the Mono framework.";
	}
#endif

#if !defined (CORECLR_RUNTIME)
	mono_assembly_name_get_culture_func = (mono_assembly_name_get_culture_def) dlsym (libmono, "mono_assembly_name_get_culture");
	if (mono_assembly_name_get_culture_func == NULL) {
		fprintf (stderr, "Could not load mono_assembly_name_get_culture\n");
		errmsg = "Failed to load the Mono framework.";
	}
#endif

#if !defined (CORECLR_RUNTIME)
	mono_install_assembly_preload_hook_func = (mono_install_assembly_preload_hook_def) dlsym (libmono, "mono_install_assembly_preload_hook");
	if (mono_install_assembly_preload_hook_func == NULL) {
		fprintf (stderr, "Could not load mono_install_assembly_preload_hook\n");
		errmsg = "Failed to load the Mono framework.";
	}
#endif

#if !defined (CORECLR_RUNTIME)
	mono_assembly_get_name_func = (mono_assembly_get_name_def) dlsym (libmono, "mono_assembly_get_name");
	if (mono_assembly_get_name_func == NULL) {
		fprintf (stderr, "Could not load mono_assembly_get_name\n");
		errmsg = "Failed to load the Mono framework.";
	}
#endif

#if !defined (CORECLR_RUNTIME)
	mono_add_internal_call_func = (mono_add_internal_call_def) dlsym (libmono, "mono_add_internal_call");
	if (mono_add_internal_call_func == NULL) {
		fprintf (stderr, "Could not load mono_add_internal_call\n");
		errmsg = "Failed to load the Mono framework.";
	}
#endif

	mono_method_signature_func = (mono_method_signature_def) dlsym (libmono, "mono_method_signature");
	if (mono_method_signature_func == NULL) {
		fprintf (stderr, "Could not load mono_method_signature\n");
		errmsg = "Failed to load the Mono framework.";
	}

	mono_method_get_class_func = (mono_method_get_class_def) dlsym (libmono, "mono_method_get_class");
	if (mono_method_get_class_func == NULL) {
		fprintf (stderr, "Could not load mono_method_get_class\n");
		errmsg = "Failed to load the Mono framework.";
	}

	mono_domain_get_func = (mono_domain_get_def) dlsym (libmono, "mono_domain_get");
	if (mono_domain_get_func == NULL) {
		fprintf (stderr, "Could not load mono_domain_get\n");
		errmsg = "Failed to load the Mono framework.";
	}

#if !defined (CORECLR_RUNTIME)
	mono_get_intptr_class_func = (mono_get_intptr_class_def) dlsym (libmono, "mono_get_intptr_class");
	if (mono_get_intptr_class_func == NULL) {
		fprintf (stderr, "Could not load mono_get_intptr_class\n");
		errmsg = "Failed to load the Mono framework.";
	}
#endif

	mono_get_string_class_func = (mono_get_string_class_def) dlsym (libmono, "mono_get_string_class");
	if (mono_get_string_class_func == NULL) {
		fprintf (stderr, "Could not load mono_get_string_class\n");
		errmsg = "Failed to load the Mono framework.";
	}

#if !defined (CORECLR_RUNTIME)
	mono_get_corlib_func = (mono_get_corlib_def) dlsym (libmono, "mono_get_corlib");
	if (mono_get_corlib_func == NULL) {
		fprintf (stderr, "Could not load mono_get_corlib\n");
		errmsg = "Failed to load the Mono framework.";
	}
#endif

#if !defined (CORECLR_RUNTIME)
	mono_get_array_class_func = (mono_get_array_class_def) dlsym (libmono, "mono_get_array_class");
	if (mono_get_array_class_func == NULL) {
		fprintf (stderr, "Could not load mono_get_array_class\n");
		errmsg = "Failed to load the Mono framework.";
	}
#endif

#if !defined (CORECLR_RUNTIME)
	mono_get_root_domain_func = (mono_get_root_domain_def) dlsym (libmono, "mono_get_root_domain");
	if (mono_get_root_domain_func == NULL) {
		fprintf (stderr, "Could not load mono_get_root_domain\n");
		errmsg = "Failed to load the Mono framework.";
	}
#endif

	mono_assembly_get_object_func = (mono_assembly_get_object_def) dlsym (libmono, "mono_assembly_get_object");
	if (mono_assembly_get_object_func == NULL) {
		fprintf (stderr, "Could not load mono_assembly_get_object\n");
		errmsg = "Failed to load the Mono framework.";
	}

	mono_method_get_object_func = (mono_method_get_object_def) dlsym (libmono, "mono_method_get_object");
	if (mono_method_get_object_func == NULL) {
		fprintf (stderr, "Could not load mono_method_get_object\n");
		errmsg = "Failed to load the Mono framework.";
	}

	mono_type_get_object_func = (mono_type_get_object_def) dlsym (libmono, "mono_type_get_object");
	if (mono_type_get_object_func == NULL) {
		fprintf (stderr, "Could not load mono_type_get_object\n");
		errmsg = "Failed to load the Mono framework.";
	}

	mono_reflection_type_get_type_func = (mono_reflection_type_get_type_def) dlsym (libmono, "mono_reflection_type_get_type");
	if (mono_reflection_type_get_type_func == NULL) {
		fprintf (stderr, "Could not load mono_reflection_type_get_type\n");
		errmsg = "Failed to load the Mono framework.";
	}

	mono_signature_get_params_func = (mono_signature_get_params_def) dlsym (libmono, "mono_signature_get_params");
	if (mono_signature_get_params_func == NULL) {
		fprintf (stderr, "Could not load mono_signature_get_params\n");
		errmsg = "Failed to load the Mono framework.";
	}

	mono_type_is_byref_func = (mono_type_is_byref_def) dlsym (libmono, "mono_type_is_byref");
	if (mono_type_is_byref_func == NULL) {
		fprintf (stderr, "Could not load mono_type_is_byref\n");
		errmsg = "Failed to load the Mono framework.";
	}

	mono_signature_get_return_type_func = (mono_signature_get_return_type_def) dlsym (libmono, "mono_signature_get_return_type");
	if (mono_signature_get_return_type_func == NULL) {
		fprintf (stderr, "Could not load mono_signature_get_return_type\n");
		errmsg = "Failed to load the Mono framework.";
	}

#if !defined (CORECLR_RUNTIME)
	mono_debug_init_func = (mono_debug_init_def) dlsym (libmono, "mono_debug_init");
	if (mono_debug_init_func == NULL) {
		fprintf (stderr, "Could not load mono_debug_init\n");
		errmsg = "Failed to load the Mono framework.";
	}
#endif

#if !defined (CORECLR_RUNTIME)
	mono_is_debugger_attached_func = (mono_is_debugger_attached_def) dlsym (libmono, "mono_is_debugger_attached");
	if (mono_is_debugger_attached_func == NULL) {
		fprintf (stderr, "Could not load mono_is_debugger_attached\n");
		errmsg = "Failed to load the Mono framework.";
	}
#endif

#if !defined (CORECLR_RUNTIME)
	mono_config_parse_memory_func = (mono_config_parse_memory_def) dlsym (libmono, "mono_config_parse_memory");
	if (mono_config_parse_memory_func == NULL) {
		fprintf (stderr, "Could not load mono_config_parse_memory\n");
		errmsg = "Failed to load the Mono framework.";
	}
#endif

	mono_g_hash_table_new_type_func = (mono_g_hash_table_new_type_def) dlsym (libmono, "mono_g_hash_table_new_type");
	if (mono_g_hash_table_new_type_func == NULL) {
		fprintf (stderr, "Could not load mono_g_hash_table_new_type\n");
		errmsg = "Failed to load the Mono framework.";
	}

	mono_g_hash_table_lookup_func = (mono_g_hash_table_lookup_def) dlsym (libmono, "mono_g_hash_table_lookup");
	if (mono_g_hash_table_lookup_func == NULL) {
		fprintf (stderr, "Could not load mono_g_hash_table_lookup\n");
		errmsg = "Failed to load the Mono framework.";
	}

	mono_g_hash_table_insert_func = (mono_g_hash_table_insert_def) dlsym (libmono, "mono_g_hash_table_insert");
	if (mono_g_hash_table_insert_func == NULL) {
		fprintf (stderr, "Could not load mono_g_hash_table_insert\n");
		errmsg = "Failed to load the Mono framework.";
	}

#if !defined (CORECLR_RUNTIME)
	mono_exception_from_name_msg_func = (mono_exception_from_name_msg_def) dlsym (libmono, "mono_exception_from_name_msg");
	if (mono_exception_from_name_msg_func == NULL) {
		fprintf (stderr, "Could not load mono_exception_from_name_msg\n");
		errmsg = "Failed to load the Mono framework.";
	}
#endif

	mono_get_exception_out_of_memory_func = (mono_get_exception_out_of_memory_def) dlsym (libmono, "mono_get_exception_out_of_memory");
	if (mono_get_exception_out_of_memory_func == NULL) {
		fprintf (stderr, "Could not load mono_get_exception_out_of_memory\n");
		errmsg = "Failed to load the Mono framework.";
	}

#if !defined (CORECLR_RUNTIME)
	mono_gc_register_finalizer_callbacks_func = (mono_gc_register_finalizer_callbacks_def) dlsym (libmono, "mono_gc_register_finalizer_callbacks");
	if (mono_gc_register_finalizer_callbacks_func == NULL) {
		fprintf (stderr, "Could not load mono_gc_register_finalizer_callbacks\n");
		errmsg = "Failed to load the Mono framework.";
	}
#endif

#if !defined (CORECLR_RUNTIME)
	mono_gc_toggleref_add_func = (mono_gc_toggleref_add_def) dlsym (libmono, "mono_gc_toggleref_add");
	if (mono_gc_toggleref_add_func == NULL) {
		fprintf (stderr, "Could not load mono_gc_toggleref_add\n");
		errmsg = "Failed to load the Mono framework.";
	}
#endif

#if !defined (CORECLR_RUNTIME)
	mono_gc_toggleref_register_callback_func = (mono_gc_toggleref_register_callback_def) dlsym (libmono, "mono_gc_toggleref_register_callback");
	if (mono_gc_toggleref_register_callback_func == NULL) {
		fprintf (stderr, "Could not load mono_gc_toggleref_register_callback\n");
		errmsg = "Failed to load the Mono framework.";
	}
#endif

	mono_free_func = (mono_free_def) dlsym (libmono, "mono_free");
	if (mono_free_func == NULL) {
		fprintf (stderr, "Could not load mono_free\n");
		errmsg = "Failed to load the Mono framework.";
	}

#if !defined (CORECLR_RUNTIME)
	mono_get_runtime_build_info_func = (mono_get_runtime_build_info_def) dlsym (libmono, "mono_get_runtime_build_info");
	if (mono_get_runtime_build_info_func == NULL) {
		fprintf (stderr, "Could not load mono_get_runtime_build_info\n");
		errmsg = "Failed to load the Mono framework.";
	}
#endif

#if !defined (CORECLR_RUNTIME)
	mono_jit_init_func = (mono_jit_init_def) dlsym (libmono, "mono_jit_init");
	if (mono_jit_init_func == NULL) {
		fprintf (stderr, "Could not load mono_jit_init\n");
		errmsg = "Failed to load the Mono framework.";
	}
#endif

#if !defined (CORECLR_RUNTIME)
	mono_jit_cleanup_func = (mono_jit_cleanup_def) dlsym (libmono, "mono_jit_cleanup");
	if (mono_jit_cleanup_func == NULL) {
		fprintf (stderr, "Could not load mono_jit_cleanup\n");
		errmsg = "Failed to load the Mono framework.";
	}
#endif

	mono_jit_exec_func = (mono_jit_exec_def) dlsym (libmono, "mono_jit_exec");
	if (mono_jit_exec_func == NULL) {
		fprintf (stderr, "Could not load mono_jit_exec\n");
		errmsg = "Failed to load the Mono framework.";
	}

#if !defined (CORECLR_RUNTIME)
	mono_jit_set_aot_mode_func = (mono_jit_set_aot_mode_def) dlsym (libmono, "mono_jit_set_aot_mode");
	if (mono_jit_set_aot_mode_func == NULL) {
		fprintf (stderr, "Could not load mono_jit_set_aot_mode\n");
		errmsg = "Failed to load the Mono framework.";
	}
#endif

#if !defined (CORECLR_RUNTIME)
	mono_set_signal_chaining_func = (mono_set_signal_chaining_def) dlsym (libmono, "mono_set_signal_chaining");
	if (mono_set_signal_chaining_func == NULL) {
		fprintf (stderr, "Could not load mono_set_signal_chaining\n");
		errmsg = "Failed to load the Mono framework.";
	}
#endif

#if !defined (CORECLR_RUNTIME)
	mono_set_crash_chaining_func = (mono_set_crash_chaining_def) dlsym (libmono, "mono_set_crash_chaining");
	if (mono_set_crash_chaining_func == NULL) {
		fprintf (stderr, "Could not load mono_set_crash_chaining\n");
		errmsg = "Failed to load the Mono framework.";
	}
#endif

#if !defined (CORECLR_RUNTIME)
	mono_jit_set_trace_options_func = (mono_jit_set_trace_options_def) dlsym (libmono, "mono_jit_set_trace_options");
	if (mono_jit_set_trace_options_func == NULL) {
		fprintf (stderr, "Could not load mono_jit_set_trace_options\n");
		errmsg = "Failed to load the Mono framework.";
	}
#endif

#if !defined (CORECLR_RUNTIME)
	mono_jit_thread_attach_func = (mono_jit_thread_attach_def) dlsym (libmono, "mono_jit_thread_attach");
	if (mono_jit_thread_attach_func == NULL) {
		fprintf (stderr, "Could not load mono_jit_thread_attach\n");
		errmsg = "Failed to load the Mono framework.";
	}
#endif

#if !defined (CORECLR_RUNTIME)
	mono_install_unhandled_exception_hook_func = (mono_install_unhandled_exception_hook_def) dlsym (libmono, "mono_install_unhandled_exception_hook");
	if (mono_install_unhandled_exception_hook_func == NULL) {
		fprintf (stderr, "Could not load mono_install_unhandled_exception_hook\n");
		errmsg = "Failed to load the Mono framework.";
	}
#endif

#if !defined (CORECLR_RUNTIME)
	mono_main_func = (mono_main_def) dlsym (libmono, "mono_main");
	if (mono_main_func == NULL) {
		fprintf (stderr, "Could not load mono_main\n");
		errmsg = "Failed to load the Mono framework.";
	}
#endif

#if !defined (CORECLR_RUNTIME)
	mono_install_load_aot_data_hook_func = (mono_install_load_aot_data_hook_def) dlsym (libmono, "mono_install_load_aot_data_hook");
#endif

#if !defined (CORECLR_RUNTIME)
	mini_parse_debug_option_func = (mini_parse_debug_option_def) dlsym (libmono, "mini_parse_debug_option");
#endif

#if !defined (CORECLR_RUNTIME)
	mono_trace_set_log_handler_func = (mono_trace_set_log_handler_def) dlsym (libmono, "mono_trace_set_log_handler");
	if (mono_trace_set_log_handler_func == NULL) {
		fprintf (stderr, "Could not load mono_trace_set_log_handler\n");
		errmsg = "Failed to load the Mono framework.";
	}
#endif

#if !defined (CORECLR_RUNTIME)
	mono_trace_set_print_handler_func = (mono_trace_set_print_handler_def) dlsym (libmono, "mono_trace_set_print_handler");
	if (mono_trace_set_print_handler_func == NULL) {
		fprintf (stderr, "Could not load mono_trace_set_print_handler\n");
		errmsg = "Failed to load the Mono framework.";
	}
#endif

#if !defined (CORECLR_RUNTIME)
	mono_trace_set_printerr_handler_func = (mono_trace_set_printerr_handler_def) dlsym (libmono, "mono_trace_set_printerr_handler");
	if (mono_trace_set_printerr_handler_func == NULL) {
		fprintf (stderr, "Could not load mono_trace_set_printerr_handler\n");
		errmsg = "Failed to load the Mono framework.";
	}
#endif

#if !defined (CORECLR_RUNTIME)
	mono_threads_enter_gc_unsafe_region_func = (mono_threads_enter_gc_unsafe_region_def) dlsym (libmono, "mono_threads_enter_gc_unsafe_region");
#endif

#if !defined (CORECLR_RUNTIME)
	mono_threads_exit_gc_unsafe_region_func = (mono_threads_exit_gc_unsafe_region_def) dlsym (libmono, "mono_threads_exit_gc_unsafe_region");
#endif

#if !defined (CORECLR_RUNTIME)
	mono_threads_enter_gc_safe_region_func = (mono_threads_enter_gc_safe_region_def) dlsym (libmono, "mono_threads_enter_gc_safe_region");
#endif

#if !defined (CORECLR_RUNTIME)
	mono_threads_exit_gc_safe_region_func = (mono_threads_exit_gc_safe_region_def) dlsym (libmono, "mono_threads_exit_gc_safe_region");
#endif

#if !defined (CORECLR_RUNTIME)
	mono_threads_assert_gc_safe_region_func = (mono_threads_assert_gc_safe_region_def) dlsym (libmono, "mono_threads_assert_gc_safe_region");
#endif

#if !defined (CORECLR_RUNTIME)
	mono_threads_assert_gc_unsafe_region_func = (mono_threads_assert_gc_unsafe_region_def) dlsym (libmono, "mono_threads_assert_gc_unsafe_region");
#endif

#if !defined (CORECLR_RUNTIME)
	mono_threads_assert_gc_starting_region_func = (mono_threads_assert_gc_starting_region_def) dlsym (libmono, "mono_threads_assert_gc_starting_region");
#endif

#if !defined (CORECLR_RUNTIME)
	mono_thread_info_current_unchecked_func = (mono_thread_info_current_unchecked_def) dlsym (libmono, "mono_thread_info_current_unchecked");
#endif

#if !defined (CORECLR_RUNTIME)
	mono_install_ftnptr_eh_callback_func = (mono_install_ftnptr_eh_callback_def) dlsym (libmono, "mono_install_ftnptr_eh_callback");
#endif

#if !defined (CORECLR_RUNTIME)
	mono_debugger_agent_parse_options_func = (mono_debugger_agent_parse_options_def) dlsym (libmono, "mono_debugger_agent_parse_options");
	if (mono_debugger_agent_parse_options_func == NULL) {
		fprintf (stderr, "Could not load mono_debugger_agent_parse_options\n");
		errmsg = "Failed to load the Mono framework.";
	}
#endif

#if !defined (CORECLR_RUNTIME)
	mono_debugger_agent_transport_handshake_func = (mono_debugger_agent_transport_handshake_def) dlsym (libmono, "mono_debugger_agent_transport_handshake");
	if (mono_debugger_agent_transport_handshake_func == NULL) {
		fprintf (stderr, "Could not load mono_debugger_agent_transport_handshake\n");
		errmsg = "Failed to load the Mono framework.";
	}
#endif

#if !defined (CORECLR_RUNTIME)
	mono_debugger_agent_register_transport_func = (mono_debugger_agent_register_transport_def) dlsym (libmono, "mono_debugger_agent_register_transport");
	if (mono_debugger_agent_register_transport_func == NULL) {
		fprintf (stderr, "Could not load mono_debugger_agent_register_transport\n");
		errmsg = "Failed to load the Mono framework.";
	}
#endif

#if !defined (CORECLR_RUNTIME)
	mono_debugger_agent_unhandled_exception_func = (mono_debugger_agent_unhandled_exception_def) dlsym (libmono, "mono_debugger_agent_unhandled_exception");
	if (mono_debugger_agent_unhandled_exception_func == NULL) {
		fprintf (stderr, "Could not load mono_debugger_agent_unhandled_exception\n");
		errmsg = "Failed to load the Mono framework.";
	}
#endif

	monovm_initialize_func = (monovm_initialize_def) dlsym (libmono, "monovm_initialize");
	if (monovm_initialize_func == NULL) {
		fprintf (stderr, "Could not load monovm_initialize\n");
		errmsg = "Failed to load the Mono framework.";
	}

	monovm_runtimeconfig_initialize_func = (monovm_runtimeconfig_initialize_def) dlsym (libmono, "monovm_runtimeconfig_initialize");
	if (monovm_runtimeconfig_initialize_func == NULL) {
		fprintf (stderr, "Could not load monovm_runtimeconfig_initialize\n");
		errmsg = "Failed to load the Mono framework.";
	}

	xamarin_dynamic_runtime_initialized = 1;

	return errmsg;
}

MONO_API MonoClass *
mono_class_from_name (MonoImage * image, const char * name_space, const char * name)
{
	return mono_class_from_name_func (image, name_space, name);
}
MONO_API MonoMethod *
mono_class_get_method_from_name (MonoClass * klass, const char * name, int param_count)
{
	return mono_class_get_method_from_name_func (klass, name, param_count);
}
MONO_API mono_bool
mono_class_is_assignable_from (MonoClass * klass, MonoClass * oklass)
{
	return mono_class_is_assignable_from_func (klass, oklass);
}
MONO_API MonoClass *
mono_class_from_mono_type (MonoType * type)
{
	return mono_class_from_mono_type_func (type);
}
MONO_API mono_bool
mono_class_is_delegate (MonoClass * klass)
{
	return mono_class_is_delegate_func (klass);
}
MONO_API MonoClass *
mono_class_get_element_class (MonoClass * klass)
{
	return mono_class_get_element_class_func (klass);
}
MONO_API const char *
mono_class_get_namespace (MonoClass * klass)
{
	return mono_class_get_namespace_func (klass);
}
MONO_API const char *
mono_class_get_name (MonoClass * klass)
{
	return mono_class_get_name_func (klass);
}
MONO_API mono_bool
mono_class_is_subclass_of (MonoClass * klass, MonoClass * klassc, mono_bool check_interfaces)
{
	return mono_class_is_subclass_of_func (klass, klassc, check_interfaces);
}
MONO_API mono_bool
mono_class_is_valuetype (MonoClass * klass)
{
	return mono_class_is_valuetype_func (klass);
}
MONO_API mono_bool
mono_class_is_enum (MonoClass * klass)
{
	return mono_class_is_enum_func (klass);
}
MONO_API MonoType *
mono_class_enum_basetype (MonoClass * klass)
{
	return mono_class_enum_basetype_func (klass);
}
MONO_API int32_t
mono_class_value_size (MonoClass * klass, uint32_t * align)
{
	return mono_class_value_size_func (klass, align);
}
MONO_API MonoType *
mono_class_get_type (MonoClass * klass)
{
	return mono_class_get_type_func (klass);
}
MONO_API gboolean
mono_class_is_nullable (MonoClass * klass)
{
	if (mono_class_is_nullable_func == NULL)
		xamarin_assertion_message ("Could not load mono_class_is_nullable\n");

	return mono_class_is_nullable_func (klass);
}
MONO_API MonoClass *
mono_class_get_nullable_param (MonoClass * klass)
{
	if (mono_class_get_nullable_param_func == NULL)
		xamarin_assertion_message ("Could not load mono_class_get_nullable_param\n");

	return mono_class_get_nullable_param_func (klass);
}
MONO_API char *
mono_method_full_name (MonoMethod * method, mono_bool signature)
{
	return mono_method_full_name_func (method, signature);
}
MONO_API MonoObject *
mono_runtime_invoke (MonoMethod * method, void * obj, void ** params, MonoObject ** exc)
{
	return mono_runtime_invoke_func (method, obj, params, exc);
}
MONO_API GCHandle
mono_gchandle_new_v2 (MonoObject * obj, mono_bool pinned)
{
	return mono_gchandle_new_v2_func (obj, pinned);
}
MONO_API MonoObject *
mono_gchandle_get_target_v2 (GCHandle gchandle)
{
	return mono_gchandle_get_target_v2_func (gchandle);
}
MONO_API void
mono_gchandle_free_v2 (GCHandle gchandle)
{
	return mono_gchandle_free_v2_func (gchandle);
}
MONO_API GCHandle
mono_gchandle_new_weakref_v2 (MonoObject * obj, mono_bool track_resurrection)
{
	return mono_gchandle_new_weakref_v2_func (obj, track_resurrection);
}
MONO_API void
mono_raise_exception (MonoException * ex)
{
	return mono_raise_exception_func (ex);
}
MONO_API void
mono_unhandled_exception (MonoObject * ex)
{
	return mono_unhandled_exception_func (ex);
}
MONO_API char*
mono_array_addr_with_size (MonoArray * array, int size, uintptr_t idx)
{
	return mono_array_addr_with_size_func (array, size, idx);
}
MONO_API MonoString *
mono_string_new (MonoDomain * domain, const char * text)
{
	return mono_string_new_func (domain, text);
}
MONO_API MonoArray *
mono_array_new (MonoDomain * domain, MonoClass * eclass, uintptr_t n)
{
	return mono_array_new_func (domain, eclass, n);
}
MONO_API void *
mono_object_unbox (MonoObject * obj)
{
	return mono_object_unbox_func (obj);
}
MONO_API char *
mono_string_to_utf8 (MonoString * string_obj)
{
	return mono_string_to_utf8_func (string_obj);
}
MONO_API uintptr_t
mono_array_length (MonoArray * array)
{
	return mono_array_length_func (array);
}
MONO_API MonoObject *
mono_object_isinst (MonoObject * obj, MonoClass * klass)
{
	return mono_object_isinst_func (obj, klass);
}
MONO_API MonoClass *
mono_object_get_class (MonoObject * obj)
{
	return mono_object_get_class_func (obj);
}
MONO_API MonoObject *
mono_value_box (MonoDomain * domain, MonoClass * klass, void * val)
{
	return mono_value_box_func (domain, klass, val);
}
MONO_API void
mono_gc_wbarrier_set_arrayref (MonoArray * arr, void * slot_ptr, MonoObject * value)
{
	return mono_gc_wbarrier_set_arrayref_func (arr, slot_ptr, value);
}
MONO_API void
mono_profiler_install (MonoProfiler * prof, MonoProfileFunc shutdown_callback)
{
	return mono_profiler_install_func (prof, shutdown_callback);
}
MONO_API void
mono_profiler_install_gc (MonoProfileGCFunc callback, MonoProfileGCResizeFunc heap_resize_callback)
{
	return mono_profiler_install_gc_func (callback, heap_resize_callback);
}
MONO_API void
mono_profiler_load (const char * desc)
{
	return mono_profiler_load_func (desc);
}
MONO_API mono_bool
mono_thread_is_foreign (MonoThread * thread)
{
	return mono_thread_is_foreign_func (thread);
}
MONO_API MonoThread * 
mono_thread_current (void)
{
	return mono_thread_current_func ();
}
MONO_API MonoThread *
mono_thread_attach (MonoDomain * domain)
{
	return mono_thread_attach_func (domain);
}
MONO_API mono_bool
mono_thread_detach_if_exiting (void)
{
	return mono_thread_detach_if_exiting_func ();
}
MONO_API void
mono_runtime_set_pending_exception (MonoException * exc, mono_bool overwrite)
{
	if (mono_runtime_set_pending_exception_func == NULL)
		return mono_raise_exception (exc);

	return mono_runtime_set_pending_exception_func (exc, overwrite);
}
MONO_API void
mono_set_assemblies_path (const char * path)
{
	return mono_set_assemblies_path_func (path);
}
MONO_API MonoAssembly *
mono_assembly_open (const char * filename, MonoImageOpenStatus * status)
{
	return mono_assembly_open_func (filename, status);
}
MONO_API MonoImage *
mono_assembly_get_image (MonoAssembly * assembly)
{
	return mono_assembly_get_image_func (assembly);
}
MONO_API MonoAssemblyName *
mono_assembly_name_new (const char * name)
{
	return mono_assembly_name_new_func (name);
}
MONO_API void
mono_assembly_name_free (MonoAssemblyName * aname)
{
	return mono_assembly_name_free_func (aname);
}
MONO_API MonoAssembly *
mono_assembly_loaded (MonoAssemblyName * aname)
{
	return mono_assembly_loaded_func (aname);
}
MONO_API void
mono_register_machine_config (const char * config_xml)
{
	return mono_register_machine_config_func (config_xml);
}
MONO_API void
mono_set_dirs (const char * assembly_dir, const char * config_dir)
{
	return mono_set_dirs_func (assembly_dir, config_dir);
}
MONO_API const char *
mono_assembly_name_get_name (MonoAssemblyName * aname)
{
	return mono_assembly_name_get_name_func (aname);
}
MONO_API const char *
mono_assembly_name_get_culture (MonoAssemblyName * aname)
{
	return mono_assembly_name_get_culture_func (aname);
}
MONO_API void
mono_install_assembly_preload_hook (MonoAssemblyPreLoadFunc func, void * user_data)
{
	return mono_install_assembly_preload_hook_func (func, user_data);
}
MONO_API MonoAssemblyName *
mono_assembly_get_name (MonoAssembly * assembly)
{
	return mono_assembly_get_name_func (assembly);
}
MONO_API void
mono_add_internal_call (const char * name, const void * method)
{
	return mono_add_internal_call_func (name, method);
}
MONO_API MonoMethodSignature *
mono_method_signature (MonoMethod * method)
{
	return mono_method_signature_func (method);
}
MONO_API MonoClass *
mono_method_get_class (MonoMethod * method)
{
	return mono_method_get_class_func (method);
}
MONO_API MonoDomain *
mono_domain_get (void)
{
	return mono_domain_get_func ();
}
MONO_API MonoClass *
mono_get_intptr_class (void)
{
	return mono_get_intptr_class_func ();
}
MONO_API MonoClass *
mono_get_string_class (void)
{
	return mono_get_string_class_func ();
}
MONO_API MonoImage *
mono_get_corlib (void)
{
	return mono_get_corlib_func ();
}
MONO_API MonoClass *
mono_get_array_class (void)
{
	return mono_get_array_class_func ();
}
MONO_API MonoDomain *
mono_get_root_domain (void)
{
	return mono_get_root_domain_func ();
}
MONO_API MonoReflectionAssembly *
mono_assembly_get_object (MonoDomain * domain, MonoAssembly * assembly)
{
	return mono_assembly_get_object_func (domain, assembly);
}
MONO_API MonoReflectionMethod *
mono_method_get_object (MonoDomain * domain, MonoMethod * method, MonoClass * refclass)
{
	return mono_method_get_object_func (domain, method, refclass);
}
MONO_API MonoReflectionType *
mono_type_get_object (MonoDomain * domain, MonoType * type)
{
	return mono_type_get_object_func (domain, type);
}
MONO_API MonoType *
mono_reflection_type_get_type (MonoReflectionType * reftype)
{
	return mono_reflection_type_get_type_func (reftype);
}
MONO_API MonoType *
mono_signature_get_params (MonoMethodSignature * sig, void ** iter)
{
	return mono_signature_get_params_func (sig, iter);
}
MONO_API mono_bool
mono_type_is_byref (MonoType * type)
{
	return mono_type_is_byref_func (type);
}
MONO_API MonoType *
mono_signature_get_return_type (MonoMethodSignature * sig)
{
	return mono_signature_get_return_type_func (sig);
}
MONO_API void
mono_debug_init (MonoDebugFormat format)
{
	return mono_debug_init_func (format);
}
MONO_API mono_bool
mono_is_debugger_attached (void)
{
	return mono_is_debugger_attached_func ();
}
MONO_API void
mono_config_parse_memory (const char * buffer)
{
	return mono_config_parse_memory_func (buffer);
}
MONO_API MonoGHashTable *
mono_g_hash_table_new_type (GHashFunc hash_func, GEqualFunc key_equal_func, MonoGHashGCType type)
{
	return mono_g_hash_table_new_type_func (hash_func, key_equal_func, type);
}
MONO_API gpointer
mono_g_hash_table_lookup (MonoGHashTable * hash, gconstpointer key)
{
	return mono_g_hash_table_lookup_func (hash, key);
}
MONO_API void
mono_g_hash_table_insert (MonoGHashTable * hash, gpointer k, gpointer v)
{
	return mono_g_hash_table_insert_func (hash, k, v);
}
MONO_API MonoException *
mono_exception_from_name_msg (MonoImage * image, const char * name_space, const char * name, const char * msg)
{
	return mono_exception_from_name_msg_func (image, name_space, name, msg);
}
MONO_API MonoException *
mono_get_exception_out_of_memory (void)
{
	return mono_get_exception_out_of_memory_func ();
}
MONO_API void
mono_gc_register_finalizer_callbacks (MonoGCFinalizerCallbacks * callbacks)
{
	return mono_gc_register_finalizer_callbacks_func (callbacks);
}
MONO_API void
mono_gc_toggleref_add (MonoObject * object, mono_bool strong_ref)
{
	return mono_gc_toggleref_add_func (object, strong_ref);
}
MONO_API void
mono_gc_toggleref_register_callback (MonoToggleRefCallback process_toggleref)
{
	return mono_gc_toggleref_register_callback_func (process_toggleref);
}
MONO_API void
mono_free (void * ptr)
{
	return mono_free_func (ptr);
}
MONO_API char *
mono_get_runtime_build_info (void)
{
	return mono_get_runtime_build_info_func ();
}
MONO_API MonoDomain *
mono_jit_init (const char * file)
{
	return mono_jit_init_func (file);
}
MONO_API MonoDomain *
mono_jit_cleanup (MonoDomain * domain)
{
	return mono_jit_cleanup_func (domain);
}
MONO_API int
mono_jit_exec (MonoDomain * domain, MonoAssembly * assembly, int argc, const char** argv)
{
	return mono_jit_exec_func (domain, assembly, argc, argv);
}
MONO_API void
mono_jit_set_aot_mode (MonoAotMode mode)
{
	return mono_jit_set_aot_mode_func (mode);
}
MONO_API void
mono_set_signal_chaining (mono_bool chain_signals)
{
	return mono_set_signal_chaining_func (chain_signals);
}
MONO_API void
mono_set_crash_chaining (mono_bool chain_signals)
{
	return mono_set_crash_chaining_func (chain_signals);
}
MONO_API void
mono_jit_set_trace_options (const char * option)
{
	return mono_jit_set_trace_options_func (option);
}
MONO_API void*
mono_jit_thread_attach (MonoDomain * domain)
{
	return mono_jit_thread_attach_func (domain);
}
MONO_API void
mono_install_unhandled_exception_hook (MonoUnhandledExceptionFunc func, gpointer user_data)
{
	return mono_install_unhandled_exception_hook_func (func, user_data);
}
MONO_API int
mono_main (int argc, char ** argv)
{
	return mono_main_func (argc, argv);
}
MONO_API void
mono_install_load_aot_data_hook (MonoLoadAotDataFunc load_func, MonoFreeAotDataFunc free_func, gpointer user_data)
{
	if (mono_install_load_aot_data_hook_func == NULL)
		xamarin_assertion_message ("Could not load mono_install_load_aot_data_hook\n");

	return mono_install_load_aot_data_hook_func (load_func, free_func, user_data);
}
MONO_API gboolean
mini_parse_debug_option (const char * option)
{
	if (mini_parse_debug_option_func == NULL)
		xamarin_assertion_message ("Could not load mini_parse_debug_option\n");

	return mini_parse_debug_option_func (option);
}
MONO_API void
mono_trace_set_log_handler (MonoLogCallback callback, void * user_data)
{
	return mono_trace_set_log_handler_func (callback, user_data);
}
MONO_API void
mono_trace_set_print_handler (MonoPrintCallback callback)
{
	return mono_trace_set_print_handler_func (callback);
}
MONO_API void
mono_trace_set_printerr_handler (MonoPrintCallback callback)
{
	return mono_trace_set_printerr_handler_func (callback);
}
MONO_API void*
mono_threads_enter_gc_unsafe_region (void ** stackdata)
{
	if (mono_threads_enter_gc_unsafe_region_func == NULL)
		xamarin_assertion_message ("Could not load mono_threads_enter_gc_unsafe_region\n");

	return mono_threads_enter_gc_unsafe_region_func (stackdata);
}
MONO_API void
mono_threads_exit_gc_unsafe_region (void * cookie, void ** stackdata)
{
	if (mono_threads_exit_gc_unsafe_region_func == NULL)
		xamarin_assertion_message ("Could not load mono_threads_exit_gc_unsafe_region\n");

	return mono_threads_exit_gc_unsafe_region_func (cookie, stackdata);
}
MONO_API void*
mono_threads_enter_gc_safe_region (void ** stackdata)
{
	if (mono_threads_enter_gc_safe_region_func == NULL)
		xamarin_assertion_message ("Could not load mono_threads_enter_gc_safe_region\n");

	return mono_threads_enter_gc_safe_region_func (stackdata);
}
MONO_API void
mono_threads_exit_gc_safe_region (void * cookie, void ** stackdata)
{
	if (mono_threads_exit_gc_safe_region_func == NULL)
		xamarin_assertion_message ("Could not load mono_threads_exit_gc_safe_region\n");

	return mono_threads_exit_gc_safe_region_func (cookie, stackdata);
}
MONO_API void
mono_threads_assert_gc_safe_region (void)
{
	if (mono_threads_assert_gc_safe_region_func == NULL)
		xamarin_assertion_message ("Could not load mono_threads_assert_gc_safe_region\n");

	return mono_threads_assert_gc_safe_region_func ();
}
MONO_API void
mono_threads_assert_gc_unsafe_region (void)
{
	if (mono_threads_assert_gc_unsafe_region_func == NULL)
		xamarin_assertion_message ("Could not load mono_threads_assert_gc_unsafe_region\n");

	return mono_threads_assert_gc_unsafe_region_func ();
}
MONO_API void
mono_threads_assert_gc_starting_region (void)
{
	if (mono_threads_assert_gc_starting_region_func == NULL)
		xamarin_assertion_message ("Could not load mono_threads_assert_gc_starting_region\n");

	return mono_threads_assert_gc_starting_region_func ();
}
MONO_API void*
mono_thread_info_current_unchecked (void)
{
	if (mono_thread_info_current_unchecked_func == NULL)
		xamarin_assertion_message ("Could not load mono_thread_info_current_unchecked\n");

	return mono_thread_info_current_unchecked_func ();
}
MONO_API void
mono_install_ftnptr_eh_callback (MonoFtnPtrEHCallback callback)
{
	if (mono_install_ftnptr_eh_callback_func == NULL)
		xamarin_assertion_message ("Could not load mono_install_ftnptr_eh_callback\n");

	return mono_install_ftnptr_eh_callback_func (callback);
}
MONO_API void
mono_debugger_agent_parse_options (const char * options)
{
	return mono_debugger_agent_parse_options_func (options);
}
MONO_API gboolean
mono_debugger_agent_transport_handshake (void)
{
	return mono_debugger_agent_transport_handshake_func ();
}
MONO_API void
mono_debugger_agent_register_transport (DebuggerTransport * trans)
{
	return mono_debugger_agent_register_transport_func (trans);
}
MONO_API void
mono_debugger_agent_unhandled_exception (MonoException * e)
{
	return mono_debugger_agent_unhandled_exception_func (e);
}
MONO_API int
monovm_initialize (int propertyCount, const char ** propertyKeys, const char ** propertyValues)
{
	return monovm_initialize_func (propertyCount, propertyKeys, propertyValues);
}
MONO_API int
monovm_runtimeconfig_initialize (MonovmRuntimeConfigArguments * arg, MonovmRuntimeConfigArgumentsCleanup cleanup_fn, void * user_data)
{
	return monovm_runtimeconfig_initialize_func (arg, cleanup_fn, user_data);
}

bool
mono_class_is_nullable_exists ()
{
	return mono_class_is_nullable_func != NULL;
}

bool
mono_class_get_nullable_param_exists ()
{
	return mono_class_get_nullable_param_func != NULL;
}

bool
mono_runtime_set_pending_exception_exists ()
{
	return mono_runtime_set_pending_exception_func != NULL;
}

bool
mono_install_load_aot_data_hook_exists ()
{
	return mono_install_load_aot_data_hook_func != NULL;
}

bool
mini_parse_debug_option_exists ()
{
	return mini_parse_debug_option_func != NULL;
}

bool
mono_threads_enter_gc_unsafe_region_exists ()
{
	return mono_threads_enter_gc_unsafe_region_func != NULL;
}

bool
mono_threads_exit_gc_unsafe_region_exists ()
{
	return mono_threads_exit_gc_unsafe_region_func != NULL;
}

bool
mono_threads_enter_gc_safe_region_exists ()
{
	return mono_threads_enter_gc_safe_region_func != NULL;
}

bool
mono_threads_exit_gc_safe_region_exists ()
{
	return mono_threads_exit_gc_safe_region_func != NULL;
}

bool
mono_threads_assert_gc_safe_region_exists ()
{
	return mono_threads_assert_gc_safe_region_func != NULL;
}

bool
mono_threads_assert_gc_unsafe_region_exists ()
{
	return mono_threads_assert_gc_unsafe_region_func != NULL;
}

bool
mono_threads_assert_gc_starting_region_exists ()
{
	return mono_threads_assert_gc_starting_region_func != NULL;
}

bool
mono_thread_info_current_unchecked_exists ()
{
	return mono_thread_info_current_unchecked_func != NULL;
}

bool
mono_install_ftnptr_eh_callback_exists ()
{
	return mono_install_ftnptr_eh_callback_func != NULL;
}

#else
int xamarin_fix_ranlib_warning_about_no_symbols;
#endif /* DYNAMIC_MONO_RUNTIME */
