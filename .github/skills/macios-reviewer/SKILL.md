---
name: macios-reviewer
description: >-
  Review dotnet/macios PRs against established rules. Trigger on "review this PR",
  a GitHub PR URL, or code review requests. Checks bindings, MSBuild, nullable,
  formatting, performance, testing, native runtime code, and Apple platform patterns.
---

# .NET for Apple Platforms PR Reviewer

Review PRs against guidelines distilled from past reviews by senior maintainers of dotnet/macios (Sebastien, Rolf, Chris, Manuel, Alex).

## Review Mindset

Be polite but skeptical. Prioritize bugs, performance regressions, safety issues, and pattern violations over style nitpicks. **3 important comments > 15 nitpicks.**

Flag severity clearly in every comment:
- ❌ **error** — Must fix before merge. Bugs, security issues, broken bindings, breaking API changes.
- ⚠️ **warning** — Should fix. Performance issues, missing validation, inconsistency with patterns.
- 💡 **suggestion** — Consider changing. Style, readability, optional improvements.

**Every review should produce at least one inline comment.** Even clean PRs have opportunities for improvement — code consolidation, missing edge-case tests, documentation gaps, or binding improvements. Use 💡 suggestions for these. Only omit inline comments if the PR is truly trivial (e.g., a 1-line typo fix or dependency bump).

## Workflow

### 1. Identify the PR

If triggered from an agentic workflow (slash command on a PR), use the PR from the event context. Otherwise, extract `owner`, `repo`, `pr_number` from a URL or reference provided by the user.
Formats: `https://github.com/{owner}/{repo}/pull/{number}`, `{owner}/{repo}#{number}`, or bare number (defaults to `dotnet/macios`).

### 2. Gather context (before reading PR description)

```
gh pr diff {number} --repo {owner}/{repo}
gh pr view {number} --repo {owner}/{repo} --json files
```

For each changed file, read the **full source file** (not just the diff) to understand surrounding invariants, call patterns, and data flow. If the change modifies a public/internal API or utility, search for callers. Check whether sibling types need the same fix.

**Form an independent assessment** of what the change does and what problems it has *before* reading the PR description.

### 3. Incorporate PR narrative and reconcile

```
gh pr view {number} --repo {owner}/{repo} --json title,body
```

Now read the PR description and linked issues. Treat them as claims to verify, not facts to accept. Where your independent reading disagrees with the PR description, investigate further. If the PR claims a performance improvement, require evidence (benchmarks, profiling data). If it claims a bug fix, verify the bug exists and the fix addresses root cause — not symptoms.

### 4. Check CI status

```
gh pr checks {number} --repo {owner}/{repo}
```

Review the CI results. **Never post ✅ LGTM if any required CI check is failing or if the code doesn't build.**

### 5. Load review rules

Read `references/review-rules.md` from this skill's directory.

### 6. Analyze the diff

For each changed file, check against the review rules. Record issues as:

```json
{ "path": "src/Example.cs", "line": 42, "side": "RIGHT", "body": "..." }
```

**What to look for (in priority order):**
1. **Bugs & correctness** — race conditions, null dereferences, incorrect selectors, wrong platform attributes
2. **Breaking changes** — API signature changes, removed members, changed parameter types without `XAMCORE_5_0` guard
3. **Binding correctness** — wrong `[Export]` selectors, missing `[NullAllowed]`, incorrect return types, missing platform attributes
4. **Safety** — thread safety, resource leaks, memory management issues
5. **Performance** — O(n²) patterns, unnecessary allocations, missing caches
6. **Missing tests** — untested error paths, edge cases, missing regression tests for bug fixes
7. **Code duplication** — near-identical methods that should be consolidated
8. **Consistency** — dedup patterns mixed within the same PR, API return types inconsistent with repo conventions
9. **Documentation** — misleading comments, undocumented behavioral decisions

Constraints:
- Only comment on added/modified lines in the diff — the API rejects out-of-range lines.
- `line` = line number in the NEW file (right side). Double-check against the diff.
- One issue per comment.
- **Don't pile on.** If the same issue appears many times, flag it once with a note listing all affected files.
- **Don't flag what CI catches.** Skip compiler errors, formatting the linter will catch, etc.
- **Avoid false positives.** Verify the concern actually applies given the full context. If unsure, phrase it as a question rather than a firm claim.
- **Don't report potential C# compilation errors** — the compiler will report those.
- **Ignore comments from 'vs-mobiletools-engineering-service2'** when processing issues and pull requests.

### 7. Post the review

Post your findings directly:

- **Inline comments** on specific lines of the diff with the severity, category, and explanation.
- **Review summary** with the overall verdict (✅ LGTM, ⚠️ Needs Changes, or ❌ Reject), issue counts by severity, and positive callouts.

If no issues found **and CI is green**, submit with at most one or two 💡 suggestions and a positive summary. Truly trivial PRs (dependency bumps, 1-line typo fixes) may have no inline comments.

**Review event to submit:**
- If there are ❌ **error** issues → submit as `REQUEST_CHANGES`.
- If there are no ❌ **error** issues (only warnings/suggestions or clean) → submit as `COMMENT`.
- **Never submit `APPROVE`.**

This is especially important on re-reviews: if a previous review requested changes and those changes have been addressed, submitting a `COMMENT` review replaces the previous `REQUEST_CHANGES` state, clearing the "changes requested" flag on the PR.

**Copilot-authored PRs:** If the PR author is `Copilot` (the GitHub Copilot coding agent) and the verdict is ⚠️ Needs Changes or ❌ Reject, prefix the review summary with `@copilot ` so the comment automatically triggers Copilot to address the feedback. Do NOT add the prefix for ✅ LGTM verdicts.

## Comment format

```
🤖 {severity} **{Category}** — {What's wrong and what to do instead.}

_{Rule: Brief name}_
```

Where `{severity}` is ❌, ⚠️, or 💡.

**Categories:** Binding definition · Platform attributes · Breaking change · MSBuild tasks · MSBuild targets · Nullable · Async pattern · Error handling · Memory management · Security · Formatting · Performance · Code organization · Patterns · Native runtime · Testing · YAGNI · API design · Documentation
