# Introspection Tests

Introspection tests are executed on specific target (both simulator and device
for iOS and tvOS) or a specific version of macOS or Mac Catalyst. The
application proceed to analyze itself using:

* `System.Reflection` for managed code; and
* the Objective-C runtime library for native code

and compare the results. E.g. if using .NET reflection it can see a binding
for a `NSBundle` type then it should be able to find a native `NSBundle` 
type using the ObjC runtime functions. Otherwise an error is raised...

Since the application analyze itself it must contains everything we wish
to test. That's why the introspection tests needs to be built with the
managed linker disabled, i.e. **"Don't link"**.

Pros

* The tests always tell the truth, which can differ from documentation or header files;

Cons

* Incomplete - Not everything is encoded in the metadata / executable;
* Too complete - Not every truth is good to be known (or published), which requires creating special cases in the tests

## API-name spelling

When enabled, `ApiTypoTest.TypoTest` splits public API names into individual
words and checks each unique word with `NSSpellChecker.CheckSpelling` on macOS
or `UITextChecker.RangeOfMisspelledWordInString` on UIKit platforms. Both paths
explicitly select `en_US`. Technical terms and existing API spellings are
handled by the platform-specific allowlist.

### Grammar-checking evaluation

[Issue #25895](https://github.com/dotnet/macios/issues/25895) considers using
`UITextChecker.RequestGrammarChecking`. Retain the spelling checks rather than
replacing them or requiring grammar checking to confirm their findings:

* The Xcode 27 `UITextChecker.h` contract permits `NSTextCheckingTypeGrammar`
  and `NSTextCheckingTypeCorrection` results, not `NSTextCheckingTypeSpelling`.
  Corrections can overlap spelling errors, but a suggested correction is not
  equivalent to reporting a misspelled word.
* Identifier fragments do not provide sentence context. Joining unrelated API
  words would invent context rather than check the original identifiers.
* The grammar request has no language parameter, so it does not provide the
  existing explicit `en_US` selection. It is also a completion-handler API
  introduced in iOS, tvOS, and Mac Catalyst 27, not a replacement for the
  synchronous macOS spelling path.

Reconsider an additional grammar pass only with evidence of new, actionable
typos in the actual identifier-word corpus, without losing spelling findings
or introducing false positives. Compare both `waitForAllResults` settings and
record runtime versions and language preferences. Include misspelled words
and deliberately incorrect sentences as controls: empty grammar results,
especially when the sentence controls also return nothing, do not establish
that grammar analysis is working or that the API can never help.
