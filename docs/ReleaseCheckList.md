# Release Check List

This is a checklist for what to do for every release.

Copy this list into a new issue and then check off as the release progresses.

# (release version)

* [ ] Create new milestone in GitHub.
* [ ] Create a new issue with a copy of this list, assign it to the milestone.
  
## (for each Xcode beta/rc/stable release; copy this section as many times as needed for each Xcode version)
  
* [ ] Update binding status in wiki.
* [ ] Create a new `xcodeX[.Y]` branch (only for the first Xcode bump) - this needs deciding where to branch from (which is outside the scope of this document). Push this branch to `origin` with no changes.
* [ ] Create PR with an Xcode bump.
  * Update the following values in `Make.versions` (only for the first Xcode bump):
    * \*_NUGET_OS_VERSION
  * Update the following values in `Make.config`:
    * XCODE_VERSION
    * XCODE_URL
    * XCODE_DEVELOPER_ROOT
  * Update any `xcodeChannel` values in tools/devops. Change to `Beta` if using an Xcode beta or release candidate, and switch back to `Stable` with the final (stable) Xcode release.
    ```shell
    $ cd tools/devops
    $ git grep -nE -e 'xcodeChannel: (Stable|Beta)' -i
      [...] # these are the matches that may need fixing.
    ```
  * Add the new OS versions to the `builds/Version-*.plist.in` files.
  * Build and fix any issues.
  * Run the following tests locally, and fix any issues (these are the ones that typically fail in Xcode bumps, but there may be other failing tests as well - CI will report any such failures once the PR is created):
    * xtro
    * monotouch-test
    * introspection
    * cecil
    * bgen
* [ ] Update any existing API bindings PRs if needed.
* [ ] Bind APIs!

## Pre-release process

This happens after the stable version of Xcode has been released and the `xcodeX[.Y]` branch has been updated according to the previous section(s).

* [ ] Make sure all the PRs targeting the `xcodeX[.Y]` branch have been merged and no more changes are desired for the release.
* [ ] Merge the `xcodeX[.Y]` branch into `main`.
* [ ] Merge `main` into the target release branch (for instance this was `release/9.0.1xx` for the Xcode 16.4 release).
* [ ] Review the API diff to make sure all api differences are expected.
* [ ] Make sure the release branch created packages after the merge and pushed them to maestro.
* [ ] Create VS insertion (this may not happen for all releases, sometimes we release only to NuGet).

## Release process
  
* [ ] Write release notes.
* [ ] Publish NuGets to nuget.org.
* [ ] Publish release notes.
* [ ] Update https://github.com/dotnet/maui/wiki/Release-Versions
* [ ] Update API diff (the `STABLE_NUGET_VERSION_*` variables in `Make.config`). Can only be done after the NuGets have been published to nuget.org.
* [ ] Update docs by executing `docs/sync-mobile-docs.sh`. Beware if docs were modified in the [docs-mobile](https://github.com/dotnet/docs-mobile) repository by somebody else, any such changes will have to be copied back first.
* [ ] Make sure all items in the milestone for the current release have been closed.
* [ ] Close this issue & close the milestone.
* [ ] Wonder about life as you consider the impossibility of actually reaching this point, because you can't close the milestone before all issues have been closed, and you can't close this issue until checking off all items, which you can't do until you've closed the milestone. Decide to schedule yet another viewing of [Life of Brian](https://en.wikipedia.org/wiki/Monty_Python%27s_Life_of_Brian) to ~cope~ celebrate the release 🍾.
