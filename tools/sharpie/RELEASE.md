# Release

This document describes how to make a new sharpie release.

We have a single release branch for Objective-Sharpie: `release/sharpie`.

1. Merge `main` into `release/sharpie`

    The first step to make a release is to merge `main` into `release/sharpie`,
    and then push that change. The only difference between the `main` and
    `release/sharpie` branches should be the value of the `NUGET_RELEASE_BRANCH`
    variable in `Make.config`.

2. Create a tag for the commit to be released.

    Run `make show-versions` to get the sharpie version. Then:
    
    ```
    $ git tag sharpie-<version>
    $ git push origin sharpie-<version>:sharpie-<version>
    ```

3. Write release notes

4. Run the release pipeline

    Note: this can only be done once our pipeline has built the sharpie nupkg, and
    published it to a maestro channel (which is done automatically when anything
    is pushed to the `release/sharpie` branch).
    
    Run our [release pipeline](https://dev.azure.com/dnceng/internal/_build?definitionId=1445)
    
    Set:
    
    * GitHub repository name: `macios`
    * Commit hash to download nupkgs from: the hash of the `release/sharpie`
      branch
    * Push workload set channel stage: unchecked
    * Semi-colon (';') separated list of nugets packages to include: the package
      name of the sharpie package. Example: `Sharpie.Bind.Tool.26.3.0.6.nupkg`.
      The exact package name will be in the GitHub comment the CI adds to the
      commit in question. You can also run `make show-versions` in the root
      directory to get the version to use.

5. Approve the 'Release packs' review / validation for the pipeline.

    You can reject the "Wait to push manifests" review / validation, it's not used for sharpie releases.
