// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

#include <errno.h>
#include <signal.h>
#include <spawn.h>
#include <stdbool.h>
#include <stdio.h>
#include <string.h>

errno_t responsibility_spawnattrs_setdisclaim (posix_spawnattr_t *attrs, bool disclaim);

int main (int argc, char** argv, char** envp)
{
	if (argc < 2) {
		fprintf (stderr,
			"spawner: launch a subprocess, disclaiming all responsibilities with regards to TCC:\n"
			"usage: spawner <command> [arguments]\n");
		return 1;
	}

	int rv;
	// Behave as exec
	short flags = POSIX_SPAWN_SETEXEC;
	posix_spawnattr_t spawnattr;
	sigset_t sigset;

	rv = posix_spawnattr_init (&spawnattr);
	if (rv) {
		fprintf (stderr, "Failed to execute 'posix_spawnattr_init': %i (%s)\n", rv, strerror (rv));
		return 1;
	}

	// Reset the signal mask
	sigemptyset (&sigset);
	rv = posix_spawnattr_setsigmask (&spawnattr, &sigset);
	if (rv) {
		fprintf (stderr, "Failed to execute 'posix_spawnattr_setsigmask': %i (%s)\n", rv, strerror (rv));
		return 1;
	}
	flags |= POSIX_SPAWN_SETSIGMASK;

	// Reset all signals to their default handlers
	sigfillset (&sigset);
	rv = posix_spawnattr_setsigdefault (&spawnattr, &sigset);
	if (rv) {
		fprintf (stderr, "Failed to execute 'posix_spawnattr_setsigdefault': %i (%s)\n", rv, strerror (rv));
		return 1;
	}
	flags |= POSIX_SPAWN_SETSIGDEF;

	rv = posix_spawnattr_setflags (&spawnattr, flags);
	if (rv) {
		fprintf (stderr, "Failed to execute 'posix_spawnattr_setflags': %i (%s)\n", rv, strerror (rv));
		return 1;
	}

	rv = responsibility_spawnattrs_setdisclaim (&spawnattr, 1);
	if (rv) {
		fprintf (stderr, "Failed to execute 'responsibility_spawnattrs_setdisclaim': %i (%s)\n", rv, strerror (rv));
		return 1;
	}

	pid_t pid = 0;
	rv = posix_spawnp (&pid, argv [1], NULL, &spawnattr, argv + 1, envp);
	posix_spawnattr_destroy (&spawnattr);

	// posix_spawnp shouldn't return (because we set the POSIX_SPAWN_SETEXEC flag)
	// so if it did, something went wrong
	fprintf (stderr, "Failed to execute '%s': %i (%s)\n", argv [1], rv, strerror (rv));
	return 1;
}
