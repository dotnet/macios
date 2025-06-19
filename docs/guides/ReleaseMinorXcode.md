# Minor Xcode release

The process for releasing a minor Xcode release is typically like this:

1. Apple releases the first beta version of the new Xcode, say Xcode 16.4 beta 1.
2. We create a new `xcode16.4` branch, where we do all our Xcode 16.4 work, through all the betas and release candidates.
3. Apple eventually releases a stable version of the new Xcode.
4. We merge the `xcode16.4` branch into `main`.
5. We merge `main` into the release branch, `release/9.0.1xx` in this case: but we discard any existing changes in `release/9.0.1xx`, basically overwriting the release branch with the contents from main.
	* Note that there might be exceptions to this rule, the diff still needs to be verified to make sure we didn't accidentally overwrite changes in `release/9.0.1xx` we want.
	* There's a script next to this document (`./ReleaseMinorXcode.sh`) which can do the merge correctly.
