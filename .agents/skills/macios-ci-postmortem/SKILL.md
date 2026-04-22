---
name: macios-ci-postmortem
description: Post-mortem analysis of CI failures across recent PRs in dotnet/macios. Identifies flaky tests, infrastructure issues, and shared regressions by analyzing builds from the last week. Files or updates GitHub issues for failures unrelated to any specific PR. Use when asked to "find flaky tests", "CI post-mortem", "what's been failing in CI", or "file issues for flaky failures".
---

# macios CI Post-Mortem

Analyze CI failures across recent PRs to identify flaky tests, infrastructure issues, and shared regressions that are not caused by any specific PR. File or update GitHub issues for these.

## References

Read these as needed during investigation:

- `references/azure-devops-cli.md` — az CLI commands, artifact naming conventions, and JSON parsing caveats.

## Overview

This skill operates in four phases:

1. **Discovery** — collect all recent PR-validation builds from AzDO
2. **Extraction** — for failed builds, extract normalized failure records
3. **Classification** — categorize failures as flaky, infrastructure, shared regression, or PR-specific
4. **Issue Actions** — propose GitHub issues, get user confirmation, then file/update

## Phase 1: Discovery — Collect Recent Builds

**Start from builds, not PRs.** This is faster, gives access to commit SHAs for rerun detection, and captures builds for PRs that may already be closed.

### Step 1.1: List recent PR-validation builds

Use the `az` CLI to get builds from the last 7 days. The macios CI runs on `devdiv.visualstudio.com/DevDiv`.

```bash
# Get the date 7 days ago in ISO format
SINCE=$(python3 -c "from datetime import datetime, timedelta; print((datetime.utcnow() - timedelta(days=7)).strftime('%Y-%m-%dT%H:%M:%SZ'))")

# List recent builds for the PR pipeline
az pipelines build list \
  --org https://devdiv.visualstudio.com \
  --project DevDiv \
  --reason pullRequest \
  --result failed \
  --top 200 \
  --query-order finishTimeDescending \
  -o json > /tmp/postmortem_builds.json
```

Also fetch partially succeeded builds (these contain test failures):

```bash
az pipelines build list \
  --org https://devdiv.visualstudio.com \
  --project DevDiv \
  --reason pullRequest \
  --result partiallySucceeded \
  --top 200 \
  --query-order finishTimeDescending \
  -o json > /tmp/postmortem_builds_partial.json
```

### Step 1.2: Parse and filter builds

```python
import json
from datetime import datetime, timedelta, timezone

since = datetime.now(timezone.utc) - timedelta(days=7)

def load_builds(path):
    with open(path) as f:
        content = f.read()
    return json.JSONDecoder().raw_decode(content)[0]

builds = load_builds('/tmp/postmortem_builds.json') + load_builds('/tmp/postmortem_builds_partial.json')

# Filter to last 7 days and macios pipelines
recent = []
for b in builds:
    finish = b.get('finishTime', '')
    if not finish:
        continue
    ft = datetime.fromisoformat(finish.replace('Z', '+00:00'))
    if ft < since:
        continue
    # Only include macios pipelines
    defn = b.get('definition', {}).get('name', '')
    if 'macios' not in defn.lower() and 'xamarin-macios' not in defn.lower():
        continue
    recent.append({
        'id': b['id'],
        'result': b['result'],
        'pr': b.get('triggerInfo', {}).get('pr.number', ''),
        'sourceBranch': b.get('sourceBranch', ''),
        'sourceVersion': b.get('sourceVersion', ''),  # commit SHA — critical for rerun detection
        'pipeline': defn,
        'finishTime': finish,
    })

print(f"Found {len(recent)} builds from {len(set(b['pr'] for b in recent if b['pr']))} PRs")
```

### Step 1.3: Group builds for rerun detection

Group by `(pr, pipeline, sourceVersion)`. Multiple builds with the same commit SHA for the same PR/pipeline are reruns.

```python
from collections import defaultdict

# Group: (pr, pipeline, commitSHA) -> [builds]
groups = defaultdict(list)
for b in recent:
    key = (b['pr'], b['pipeline'], b['sourceVersion'])
    groups[key].append(b)

# Also group by just (pr, pipeline) to see if new commits fixed things
pr_pipeline = defaultdict(list)
for b in recent:
    key = (b['pr'], b['pipeline'])
    pr_pipeline[key].append(b)
```

## Phase 2: Extraction — Get Failure Details

For each failed/partiallySucceeded build, extract failure information. Use a SQL database to track failures across builds.

### Step 2.1: Set up failure tracking

```sql
CREATE TABLE IF NOT EXISTS ci_failures (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    build_id INTEGER,
    pr TEXT,
    pipeline TEXT,
    commit_sha TEXT,
    finish_time TEXT,
    job_name TEXT,
    failure_type TEXT,     -- 'TestFailure', 'BuildFailure', 'TimedOut', 'Crashed', 'Infrastructure'
    test_fullname TEXT,    -- e.g. 'MonoTouchFixtures.SomeTest.TestMethod'
    platform TEXT,         -- e.g. 'ios', 'tvos', 'macos', 'maccatalyst'
    config TEXT,           -- e.g. 'Debug (ARM64)', 'Release (x64)'
    error_signature TEXT,  -- normalized error message / top stack frame
    raw_message TEXT
);
```

### Step 2.2: For each build, get the timeline and TestSummary artifacts

Only process builds with failures. For efficiency, first check the timeline for failed jobs, then only download artifacts for those jobs.

```bash
# Get timeline
az devops invoke --area build --resource timeline \
  --route-parameters project=DevDiv buildId=<buildId> \
  --org https://devdiv.visualstudio.com -o json > /tmp/timeline_<buildId>.json
```

Parse the timeline to find failed jobs:

```python
import json

with open(f'/tmp/timeline_{build_id}.json') as f:
    data = json.JSONDecoder().raw_decode(f.read())[0]

failed_jobs = []
for r in data.get('records', []):
    if r.get('type') == 'Job' and r.get('result') == 'failed':
        failed_jobs.append({
            'name': r['name'],
            'id': r['id'],
            'logId': r.get('log', {}).get('id'),
        })
```

### Step 2.3: Download and parse TestSummary artifacts

For each failed job, download the TestSummary artifact:

```bash
artifact="TestSummary-simulator_tests<jobname>-1"
mkdir -p "/tmp/postmortem/${build_id}/${artifact}"
az pipelines runs artifact download \
  --artifact-name "$artifact" \
  --path "/tmp/postmortem/${build_id}/${artifact}" \
  --run-id <buildId> \
  --org https://devdiv.visualstudio.com --project DevDiv
```

Parse the TestSummary.md for individual failures and insert into the SQL database.

### Step 2.4: For infrastructure/setup failures without TestSummary

Check the timeline for failed tasks in setup/provisioning stages. Extract error info from task log lines:

```bash
az devops invoke --area build --resource logs \
  --route-parameters project=DevDiv buildId=<buildId> logId=<logId> \
  --org https://devdiv.visualstudio.com -o json > /tmp/log_<buildId>_<logId>.json
```

Search for infrastructure-related errors:
- "Provision" failures
- "Reserve bot" failures
- Network/timeout errors
- Xcode installation issues

### Step 2.5: Normalize failure signatures

Create a normalized signature for deduplication:

```python
def normalize_signature(failure_type, test_fullname, error_msg, platform):
    """Create a stable key for grouping the same logical failure."""
    if test_fullname:
        # For test failures, the test name + platform is the key
        return f"{failure_type}|{platform}|{test_fullname}"
    elif error_msg:
        # For build/infra failures, normalize the error message
        # Strip file paths, line numbers, timestamps
        import re
        normalized = re.sub(r'/[^\s:]+/', '.../', error_msg)
        normalized = re.sub(r'line \d+', 'line N', normalized)
        normalized = re.sub(r'\d{4}-\d{2}-\d{2}T[\d:.]+Z?', 'TIMESTAMP', normalized)
        return f"{failure_type}|{platform}|{normalized[:200]}"
    return f"{failure_type}|{platform}|unknown"
```

## Phase 3: Classification

Query the failure database to classify each unique failure.

### Step 3.1: Identify flaky tests (same commit, different outcomes)

A failure is **flaky** if the same PR + pipeline + commit SHA has both failing and succeeding builds, OR if a rerun of the exact same configuration passes.

```sql
-- Find failures where the same commit had a passing build too
-- (builds that aren't in our failure DB were successful)
SELECT DISTINCT error_signature, test_fullname, platform,
       COUNT(DISTINCT build_id) as fail_count,
       COUNT(DISTINCT pr) as pr_count,
       GROUP_CONCAT(DISTINCT pr) as prs
FROM ci_failures
GROUP BY error_signature
HAVING COUNT(DISTINCT build_id) > 0;
```

Cross-reference with the build groups from Phase 1: if a `(pr, pipeline, commitSHA)` group has multiple builds and at least one succeeded (not in the failure DB), then failures in the failing builds for that group are flaky.

### Step 3.2: Identify shared regressions (same failure across unrelated PRs)

```sql
-- Failures appearing across 2+ unrelated PRs
SELECT error_signature, test_fullname, platform, failure_type,
       COUNT(DISTINCT pr) as pr_count,
       COUNT(DISTINCT build_id) as build_count,
       GROUP_CONCAT(DISTINCT pr) as affected_prs
FROM ci_failures
WHERE pr != ''
GROUP BY error_signature
HAVING COUNT(DISTINCT pr) >= 2
ORDER BY pr_count DESC;
```

If the failure is NOT also identified as flaky (i.e., it doesn't go away on rerun), classify it as a **shared regression**.

### Step 3.3: Identify infrastructure failures

Look for patterns in failure_type and error messages:

```sql
SELECT error_signature, failure_type, raw_message,
       COUNT(DISTINCT build_id) as occurrences
FROM ci_failures
WHERE failure_type = 'Infrastructure'
   OR raw_message LIKE '%provision%'
   OR raw_message LIKE '%reserve bot%'
   OR raw_message LIKE '%timeout%waiting%'
   OR raw_message LIKE '%network%'
   OR raw_message LIKE '%Could not find simulator%'
GROUP BY error_signature
ORDER BY occurrences DESC;
```

### Step 3.4: Exclude PR-specific failures

A failure is PR-specific if:
- It appears in only 1 PR
- It persists across commits within that PR (not a rerun flake)
- It is consistent (never passes on rerun)

These should be **excluded** from issue filing — they are the PR author's problem.

### Step 3.5: Produce classification summary

Create a summary table for user review:

```
| Category           | Signature (truncated)          | Test/Error          | Platform    | PRs Affected | Occurrences |
|--------------------|--------------------------------|---------------------|-------------|-------------- |-------------|
| Flaky              | TestFailure|ios|Mono...Test    | SomeTest.Method     | ios         | 5            | 8           |
| Shared Regression  | BuildFailure|macos|error CS... | (build error)       | macos       | 3            | 3           |
| Infrastructure     | Infrastructure|*|provision...  | Bot provisioning    | all         | 4            | 4           |
```

## Phase 4: Issue Actions

### Step 4.1: Search for existing issues

For each classified failure, search for an existing GitHub issue:

```bash
# Search by test name or error signature in issue title
gh issue list --repo dotnet/macios --state open \
  --search "<test_fullname or key error phrase>" \
  --label "bug" --json number,title,labels,url
```

Also search closed issues (may need reopening):

```bash
gh issue list --repo dotnet/macios --state closed \
  --search "<test_fullname or key error phrase>" \
  --label "bug" --json number,title,labels,url
```

### Step 4.2: Propose actions to the user

Present a list of proposed actions **before executing any**. Use `ask_user` to get confirmation.

For each failure, propose one of:
- **Create new issue** — no existing issue found
- **Comment on existing issue** — matching open issue found, add recent occurrence data
- **Reopen issue** — matching closed issue found, failure has recurred
- **Skip** — user decides this isn't worth tracking

Format the proposal clearly:

```
## Proposed Issue Actions

### 1. Flaky: MonoTouchFixtures.NetworkTest.TestReachability (iOS)
   - Seen in 5 PRs, 8 builds over the past week
   - Disappears on rerun → flaky
   - Existing issue: #12345 (open) — will add comment with recent data
   - **Proposed action:** Comment on #12345

### 2. Shared Regression: error CS1234 in SomeFile.cs (macOS)
   - Seen in 3 PRs, consistent (no rerun recovery)
   - No existing issue found
   - **Proposed action:** Create new issue

### 3. Infrastructure: Bot provisioning timeout
   - Seen in 4 builds across 4 PRs
   - Existing issue: #11111 (closed) — last closed 2 months ago
   - **Proposed action:** Reopen #11111

Proceed with these actions? [Confirm / Edit / Skip]
```

### Step 4.3: Execute confirmed actions

#### Create new issue

```bash
gh issue create --repo dotnet/macios \
  --title "[CI] Flaky: <test_fullname> on <platform>" \
  --label "bug,CI,flaky-test" \
  --body "$(cat <<'EOF'
## Flaky Test Report (automated)

**Test:** `<test_fullname>`
**Platform:** <platform>
**Category:** Flaky / Shared Regression / Infrastructure
**Period:** <start_date> to <end_date>

### Occurrence Summary

| PR | Build | Commit | Date | Result |
|----|-------|--------|------|--------|
| #<pr> | [<buildId>](<url>) | <sha7> | <date> | Failed |
| #<pr> | [<buildId>](<url>) | <sha7> | <date> | Passed on rerun |

**Total:** Failed in <N> builds across <M> PRs

### Error Details

```
<error message or assertion failure>
```

### Classification

This failure was identified as **flaky** because:
- It appeared across <M> unrelated PRs
- It disappeared on rerun in <K> cases

---
*This issue was automatically generated by CI post-mortem analysis.*
EOF
)"
```

Use the label `flaky-test` for flaky tests, `infrastructure` for infra issues, and `CI` for all.

#### Comment on existing issue

```bash
gh issue comment <issue_number> --repo dotnet/macios --body "$(cat <<'EOF'
## CI Post-Mortem Update (<date range>)

This failure was seen again in the past week:

| PR | Build | Date | Outcome |
|----|-------|------|---------|
| #<pr> | <buildId> | <date> | Failed |
...

Total: <N> occurrences across <M> PRs this week.
EOF
)"
```

#### Reopen closed issue

```bash
gh issue reopen <issue_number> --repo dotnet/macios
gh issue comment <issue_number> --repo dotnet/macios --body "Reopening — this failure recurred in <N> builds this week. See details below.
..."
```

## Important Notes

### Efficiency

- Process builds in batches. Don't download artifacts for every build — first check the timeline for failed jobs.
- Use the SQL database to accumulate results incrementally. You can query it between phases.
- Skip builds older than 7 days early in the pipeline.

### Accuracy

- **Rerun detection requires matching commit SHA.** A newer commit on the same PR that passes does NOT prove flakiness — the new commit may have fixed the issue.
- **Verify the same job/config ran** before concluding a failure "went away." The test matrix can vary between runs.
- **Don't conflate platforms.** A test failing on iOS and macOS should be tracked separately unless the error signature is identical.

### Rate Limiting

- AzDO API calls are subject to rate limits. Add small delays between artifact downloads if processing many builds.
- `gh` CLI may also rate-limit. Batch issue searches where possible.

### Confirmation

- **Never file or modify issues without user confirmation.** Always present the classification summary and proposed actions first.
- Let the user edit the proposals (e.g., skip certain failures, change labels, adjust titles).
