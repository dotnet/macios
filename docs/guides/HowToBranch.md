# How to branch for .NET releases

Context: [dotnet/maui#589][0]
Context: [dotnet/android: HowToBranch][4]

Microsoft employees can review the [.NET schedule][3] for upcoming releases.
We normally branch when we get a new version of the .NET runtime in a Maestro
update (to the dotnet/dotnet dependency). For instance: we branched .NET 10
preview 5 just before merging the pull request that updates the dotnet/dotnet
dependency from preview 5 to preview 6. This happens during the "Code
complete, release branch snap and build" window.

Let's say that it's time for a hypothetical ".NET 10 Preview 42". The
sequence of events would be:

1. [dotnet/dotnet][1] branches `release/10.0.1xx-preview42`, and their `main` branch becomes Preview 43.

2. Builds are available on Maestro for both the `release/10.0.1xx-preview42` and `main` branches.

3. `dotnet/macios` branches `release/10.0.1xx-preview42` from `net10.0`:

	```shell
	$ dotnet checkout net10.0
	$ dotnet checkout -b release/10.0.1xx-preview42
	```

	Note that release candidates will use values such as `rc.1`, `rc.2`, etc.

2. Change the value `NUGET_HARDCODED_PRERELEASE_BRANCH` in `Make.config` to the new branch name (`release/10.0.1xx-preview42`) and commit it.

	```shell
	$ sed -i '' 's@^NUGET_HARDCODED_PRERELEASE_BRANCH=.*@NUGET_HARDCODED_PRERELEASE_BRANCH=release/10.0.1xx-preview42@' Make.config
	$ git add Make.config
	$ git commit -m "[release/10.0.1xx-preview42] Make this the .NET 10 P42 release branch."
	```

3. Push the new branch to `origin`.

	```shell
	$ git push origin release/10.0.1xx-preview42:release/10.0.1xx-preview42
	```

4. Go back to `net10.0`, and change the value `NUGET_HARDCODED_PRERELEASE_IDENTIFIER` in `Make.config` to the next preview/release version and commit it. Example: `net10-p43`.

	```shell
	$ git checkout net10.0
	$ sed -i '' 's@^NUGET_HARDCODED_PRERELEASE_IDENTIFIER=.*@NUGET_HARDCODED_PRERELEASE_IDENTIFIER=net10-p43@' Make.config
	```

5. Push this change directly to the `net10.0` branch without going through a PR (it's important to get this change in immediately, because it prevents multiple builds from having the same version number).

	```shell
	$ git push origin net10.0:net10.0
	```

6. Create a new default channel in maestro:

	```shell
	$ darc add-default-channel --repo https://github.com/dotnet/macios --channel '.NET 10.0.1xx SDK Preview 42' --branch release/10.0.1xx-preview42
	```

>[!NOTE]
>See [eng/README.md][2] for details on `darc` commands.

7. Add a subscription to the .NET channel in maestro:

	```shell
	$ darc add-subscription --source-repo https://github.com/dotnet/dotnet --channel ".NET 10.0.1xx SDK Preview 42" --target-repo https://github.com/dotnet/macios --update-frequency everyDay --target-branch 'release/10.0.1xx-preview42
	```

8. When the build is complete, verify that builds show up on https://maestro.dot.net/ in the appropriate "channel" for dotnet/maui to consume.

[0]: https://github.com/dotnet/maui/issues/598
[1]: https://github.com/dotnet/dotnet
[2]: ../../eng/README.md
[3]: https://aka.ms/net10-schedule
[4]: https://github.com/dotnet/android/blob/main/Documentation/guides/HowToBranch.md
