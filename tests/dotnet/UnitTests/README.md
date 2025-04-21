This directory contains .NET-related unit tests.

We run these tests under a few configurations:

1. On macOS, in a single-platform mode.
2. On macOS, in a mode with all platforms enabled.
3. On Windows, while connected to a remote Mac.
4. On Windows, while not connected to a remote Mac.

By default, tests are executed when in the first configuration only, but
categories can be used to change the default:

Categories:

* Multiplatform: indicates that this test needs all platforms enabled to be able to run successfully (i.e. configuration 2 from above).
* RemoteWindows: exclusively executed on Windows, while connected to a remote Mac (i.e. configuration 3 from above).
* Windows: exclusively executed on Windows, while *not* connected to a remote Mac (i.e. configuration 4 from above).
* RemoteWindowsInclusive: executed on Windows, while connected to a remote Mac (i.e. configuration 3 from above), but tests with this category will also be executed in any other applicable configuration (the default configuration + any other "*Inclusive" categories).
* WindowsInclusive: executed on Windows, while *not* connected to a remote Mac (i.e. configuration 4 from above), but tests with this category will also be executed in any other applicable configuration (the default configuration + any other "*Inclusive" categories).
