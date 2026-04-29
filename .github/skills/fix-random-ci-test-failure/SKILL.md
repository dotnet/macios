---
name: fix-random-ci-test-failure
description: >-
  Investigate and fix flaky/random CI test failures in dotnet/macios. Trigger on
  GitHub issues describing intermittent test failures, CI postmortem issues, or when
  asked to fix a flaky test. Analyzes test code, identifies root causes
  (shared state, environment dependencies, race conditions), and applies fixes.
---

# Fix Random CI Test Failure

Investigate and fix flaky CI test failures reported in GitHub issues for dotnet/macios.

## When to Use

- User shares a GitHub issue about a flaky/intermittent test failure
- Issue has the `ci-postmortem` label
- User asks to fix a randomly failing test
- Test failures appear across unrelated PRs (indicating environment-dependent flakiness)

## Workflow

### 1. Gather Failure Context

Read the GitHub issue to extract:
- **Test name** and **test suite** (e.g., monotouch-test)
- **Error message** and **assertion failure details**
- **Platform** (iOS, macOS, tvOS, Mac Catalyst)
- **Affected builds** — check if failures span unrelated PRs (confirms flakiness vs. regression)

If build URLs are provided, inspect logs for additional context (stack traces, timing info).

### 2. Locate and Analyze the Test

Find the test source file:
```
grep -r "TestMethodName" tests/
```

Read the **full test method** and all helper methods it calls. Understand:
- What external resources does it use? (keychain, file system, network, simulators)
- Does it use shared/hardcoded identifiers that could collide across parallel runs?
- Does it properly clean up before and after execution?
- Are there error paths that silently swallow failures?

### 3. Identify Root Cause Category

Common flaky test root causes in this repo:

#### Shared State / Resource Conflicts
- **Symptom**: Test uses hardcoded identifiers (e.g., fixed keychain entries, file paths, port numbers)
- **Fix**: Use process-unique identifiers (PID, GUID, bundle ID + test name)
- **Reference**: `tests/monotouch-test/Security/KeyChainTest.cs` uses per-process unique IDs

#### Environment-Dependent State
- **Symptom**: Test depends on OS-level state (keychain, permissions, network availability)
- **Fix**: Add robust setup/teardown, handle unexpected initial states, add retry logic for transient errors

#### Unhandled Error Codes
- **Symptom**: Code only handles expected success/failure codes, silently fails on unexpected ones
- **Fix**: Add fallback handling for unexpected status codes, log diagnostic info

#### Race Conditions / Timing
- **Symptom**: Test passes locally but fails intermittently in CI
- **Fix**: Add proper synchronization, increase timeouts, avoid timing-dependent assertions

#### LAContext / Authentication Issues (Security tests)
- **Symptom**: `InvalidRecord` or authentication-related errors on keychain operations
- **Fix**: Only attach `LAContext` where actually needed (not on plain query/delete operations)

### 4. Apply the Fix

When fixing:

1. **Prefer unique identifiers over shared ones.** Use `Process.GetCurrentProcess ().Id`, `Guid.NewGuid ()`, or `{bundleId}-{testType}-{pid}` patterns for resource identifiers.

2. **Create minimal query records.** For search/delete operations, don't attach unnecessary attributes (like `LAContext`) that can cause intermittent errors.

3. **Handle all status codes.** Never silently return `false` for unexpected error codes. Either handle them with a fallback path or fail with a descriptive assertion message.

4. **Add diagnostic logging.** Use `TestContext.Out.WriteLine` to log operation results that would help diagnose future failures.

5. **Clean up legacy state.** If renaming identifiers, also clean up entries from old names that may linger on CI agents.

6. **Always clean up in `finally` blocks.** Ensure test resources are released even on failure.

### 5. If the Fix is Unclear

If you cannot determine the root cause with available information:

1. **Add diagnostic logging** to capture operation results (status codes, error details) in future failures.
2. **Log the initial state** at test start (e.g., does the resource already exist before the test runs?).
3. **Include all relevant status codes** in assertion failure messages.
4. Explain to the user what additional information the logging will provide and what hypotheses it will help test.

### 6. Create the PR

- Branch naming: `dev/{username}/fix-{test-name}` or similar
- Commit message should explain all root causes addressed
- Reference the GitHub issue with `Fixes #NNNN` if the change actually fixes the problem, or `Ref #NNNN` if it only adds logging/diagnostics
- Add the `copilot` label to the PR

## Key Patterns in This Repo

- Keychain tests in `tests/monotouch-test/Security/` are particularly prone to flakiness due to shared macOS keychain state on CI agents
- `InitSecRecord` in `RecordTest.cs` attaches `LAContext` to all `SecRecord` instances — this is sometimes unnecessary and can cause `InvalidRecord` errors
- `KeyChainTest.cs` demonstrates the recommended pattern: per-process unique identifiers using bundle ID + test type + PID
- Tests run on shared CI agent machines where leftover state from previous runs can cause interference
