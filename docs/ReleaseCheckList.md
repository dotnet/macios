# Release Check List

This is a checklist for what to do for every release.

Copy this list into a new issue and then check off as the release progresses.

# ✅ Checklist 

## Xcode X.Y

* [ ] Create new milestone in GitHub.
* [ ] Create a new issue with a copy of this list, assign it to the milestone.
    
### Beta 1
  
* [ ] Update binding status in wiki (read instructions in the Makefile in the wiki's git repository)
* [ ] Create a new `xcodeX[.Y]` branch - this needs deciding where to branch from (which is outside the scope of this document). Push this branch to `origin` with no changes.
* [ ] Create PR with an Xcode bump.
  * Update the following values in `Make.versions`:
    * \*_NUGET\_OS\_VERSION
  * Update the following values in `Make.config`:
    * XCODE\_VERSION
    * XCODE\_URL
    * XCODE\_DEVELOPER\_ROOT
  * Update the `xcodeChannel` value in `tools/devops/automation/templates/variables/common.yml`. Change to `Beta` if using an Xcode beta or release candidate, and switch back to `Stable` with the final (stable) Xcode release.
  * Add the new OS versions to the `builds/Version-*.plist.in` files.
  * Build and fix any issues.
  * Run the following tests locally, and fix any issues (these are the ones that typically fail in Xcode bumps, but there may be other failing tests as well - CI will report any such failures once the PR is created):
    * xtro
    * monotouch-test
    * introspection
    * cecil
    * bgen
* [ ] Bind APIs!

### Beta X
  
* [ ] Update binding status in wiki.
* [ ] Create PR with an Xcode bump.
  * Build and fix any issues.
  * Run the following tests locally, and fix any issues (these are the ones that typically fail in Xcode bumps, but there may be other failing tests as well - CI will report any such failures once the PR is created):
    * xtro
    * monotouch-test
    * introspection
    * cecil
    * bgen
* [ ] Update any existing API bindings PRs if needed.
* [ ] Bind APIs!

### Pre-release process

This happens after the stable version of Xcode has been released and the `xcodeX[.Y]` branch has been updated according to the previous section(s).

* [ ] Make sure all the PRs targeting the `xcodeX[.Y]` branch have been merged and no more changes are desired for the release.
* [ ] Merge the `xcodeX[.Y]` branch into `main`.
* [ ] Merge `main` into the target release branch (for instance this was `release/9.0.1xx` for the Xcode 16.4 release).
* [ ] Review the API diff to make sure all api differences are expected.
* [ ] Make sure the release branch created packages after the merge and pushed them to maestro.
* [ ] Create VS insertion (this may not happen for all releases, sometimes we release only to NuGet).
* [ ] Let MAUI know about our new package names, so they can update their darc dependencies. For instance, these dependencies: https://github.com/dotnet/maui/blob/408a1cc69d6a120cafa4e75ae30411f1ec48fa3a/eng/Version.Details.xml#L15-L30 would have to be updated to `net9.0_18.5` or `net9.0_15.5` for our Xcode 16.4 release.

## Release process
  
* [ ] Write release notes.
* [ ] Publish NuGets to nuget.org.
* [ ] Publish release notes.
* [ ] Update API diff (the `STABLE_NUGET_VERSION_*` variables in `Make.config`) for the `main` branch. Can only be done after the NuGets have been published to nuget.org.
* [ ] Update docs by executing `docs/sync-mobile-docs.sh`. Beware if docs were modified in the [docs-mobile](https://github.com/dotnet/docs-mobile) repository by somebody else, any such changes will have to be copied back first.
* [ ] Update API / reference docs. See [update-api-docs.md](https://github.com/dotnet/macios/blob/main/docs/update-api-docs.md) for instructions.
* [ ] Make sure all items in the milestone for the current release have been closed.
* [ ] Close this issue & close the milestone.
* [ ] Wonder about life as you consider the impossibility of actually reaching this point, because you can't close the milestone before all issues have been closed, and you can't close this issue until checking off all items, which you can't do until you've closed the milestone. Decide to schedule yet another viewing of [Life of Brian](https://en.wikipedia.org/wiki/Monty_Python%27s_Life_of_Brian) to ~cope~ celebrate the release 🍾.
