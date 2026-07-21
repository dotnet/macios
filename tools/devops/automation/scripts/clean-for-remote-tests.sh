#!/bin/bash -eux

# Delete a directory tree, even if it contains read-only directories.
#
# Some builds copy read-only directories into their output (by mistake,
# they're PR builds after all - for instance the readonly macOS timezone
# database at /var/db/timezone, which is mode 0555, could be copied around if
# MSBuild globs are incorrect). BSD/macOS 'rm -rf' won't remove entries inside
# a directory that lacks the write bit, and it doesn't chmod such directories
# to gain access (unlike GNU rm), so it fails with "Permission denied" and
# leaves the tree behind. That would make this cleanup step fail on every
# subsequent build. So make sure everything is writable before deleting.
#
# Use 'u+wX' (capital X) so that directories also get the execute bit they
# need to be traversable and deletable, without setting execute on regular
# files that don't already have it.
safe_rm () {
	chmod -R u+wX "$@" || true
	rm -rf "$@"
}

# I've seen machines with more than 1gb of Xamarin.Messaging logs, so clean that up.
if du -hs ~/Library/Logs/Xamarin.Messaging*; then
	safe_rm ~/Library/Logs/Xamarin.Messaging*
fi

# Make sure we don't have any old stuff installed
if du -hs ~/Library/Caches/Xamarin; then
	safe_rm ~/Library/Caches/Xamarin
fi

# Make sure we don't have any old stuff installed
if du -hs ~/Library/Caches/maui; then
	safe_rm ~/Library/Caches/maui
fi

# Clean up temporary logs
rm -rf /tmp/com.xamarin.*

# Make sure we don't have stuff from earlier builds.
rm -rf ~/remote_build_testing

# Kill any existing brokers and builders
ps auxww || true
pkill -6 -f Broker.exe || true
pkill -6 -f Build.exe || true
pkill -6 -f Broker.dll || true
pkill -6 -f Build.dll || true
ps auxww || true
