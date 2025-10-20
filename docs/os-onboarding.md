# Onboarding Guide for New Operating System Versions

Adding support for new operating systems versions is a frequent need. This guide describes how we do that, including the policies we use.

TLDR: [ReleaseCheckList.md](ReleaseCheckList.md)

## Context

There are multiple sides to adding support for new OS versions:

* Make sure existing products / code continue to work.
* Bind new APIs.

### Making sure existing products continue to work

This involves:

1. Testing the current stable version(s) to see if everything still works. If something doesn't work anymore, we may consider backporting any fixes and release them in a service release before the new OS is released.
2. Building and running tests using the new Xcode / OS.

### Bind new APIs

This is technically optional, because we can release support for a new OS version even if we bind any of the new APIs.

Yet we try to bind most of new APIs, because it's hard to predict what users will need.

However, for some frameworks that are rarely used and/or very complex to bind, we've decided to skip them. We file an issue for each such framework.

## Removal of support for older OS versions

We typically follow whatever the .NET runtime supports (we can't support anything lower than they do, and so far we've never had a reason to not support their lowest OS version).

Typically we're rather conservative when removing support for older OS versions, however we have limited testing on older OS versions, in particular for mobile platforms.

## References:

* https://github.com/dotnet/runtime/blob/main/docs/project/os-onboarding.md
