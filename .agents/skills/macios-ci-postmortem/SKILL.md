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

### Step 2.3: Download TestSummary artifacts (fast triage)

TestSummary artifacts are small and quick to download. Use them first to identify which jobs failed:

```bash
artifact="TestSummary-simulator_tests<jobname>-1"
mkdir -p "/tmp/postmortem/${build_id}/${artifact}"
az pipelines runs artifact download \
  --artifact-name "$artifact" \
  --path "/tmp/postmortem/${build_id}/${artifact}" \
  --run-id <buildId> \
  --org https://devdiv.visualstudio.com --project DevDiv
```

Parse the TestSummary.md to determine which jobs have test failures. This is the first-pass filter.

### Step 2.4: Download HtmlReport artifacts (deep analysis)

**This is the most time-consuming step — minimize downloads aggressively.**

Each test run produces an HtmlReport artifact (60-140MB zip) containing:
- `tests/index.html` — Main report with all test configurations, pass/fail, inline failure details
- `tests/<suite>/<num>/test-<platform>-<timestamp>.xml` — NUnit XML with individual test-case results
- `tests/<suite>/<num>/results-<timestamp>.xml` — NUnit results for dotnettests

**CRITICAL: Only download HtmlReport zips for jobs where TestSummary shows TEST failures (❌ markers).** Do NOT download HtmlReports for:
- Build failures (no test results exist — the build didn't get far enough to run tests)
- Infrastructure failures (bot provisioning, timeout, etc.)
- Jobs where TestSummary shows all tests passed (the job may have failed for other reasons)

To find exact artifact names, first list artifacts for the build:
```bash
az pipelines runs artifact list --run-id <buildId> \
  --org https://devdiv.visualstudio.com --project DevDiv -o json
```

Then download only matching HtmlReport artifacts for jobs with test failures:
```bash
az pipelines runs artifact download \
  --artifact-name "HtmlReport-<exact-name>-1" \
  --path "/tmp/postmortem_deep/" \
  --run-id <buildId> \
  --org https://devdiv.visualstudio.com --project DevDiv
```

**Performance note:** Each download takes 1-3 minutes (sequential, no parallelism in az CLI). Downloading 500 artifacts takes ~2 hours. By filtering with TestSummary first, you can typically reduce this to 50-100 artifacts.

### Step 2.5: Parse NUnit XML for individual test failures

Extract individual test failures from the NUnit XML files inside the HtmlReport zips:

```python
import zipfile, xml.etree.ElementTree as ET, html

def extract_failures_from_nunit_xml(xml_content):
    """Parse NUnit XML to extract individual failing test cases."""
    root = ET.fromstring(xml_content)
    failures = []
    for tc in root.iter('test-case'):
        if tc.get('result') == 'Failed':
            name = tc.get('fullname', 'Unknown')
            msg_el = tc.find('.//failure/message')
            stack_el = tc.find('.//failure/stack-trace')
            failures.append({
                'test': name,
                'message': msg_el.text if msg_el is not None else '',
                'stack': stack_el.text[:500] if stack_el is not None else '',
            })
    return failures

# Process a zip file
with zipfile.ZipFile('/tmp/postmortem_deep/html_BUILDID_JOB.zip') as zf:
    for name in zf.namelist():
        if name.endswith('.xml') and 'test-' in name and '-clean' not in name:
            xml_content = zf.read(name).decode('utf-8', errors='replace')
            failures = extract_failures_from_nunit_xml(xml_content)
```

**Important**: Skip files ending in `-clean.xml` (these are filtered versions). The root XML tag is `TouchUnitTestRun` (not standard NUnit format, but `test-case` elements follow standard structure).

For **dotnettests**, individual test failures are listed inline in `<li>` tags in the HTML (not in separate XML). Parse these from `tests/index.html`:

```python
import re
# Pattern for inline test failures in dotnettests HTML
failures_in_html = re.findall(r'<li[^>]*>([^<]*(?:Failed|Error)[^<]*)</li>', html_content)
```

### Step 2.6: Handle crashes and build failures

When a test runner crashes (exit code 134, etc.) or a build fails before tests run, there will be **no NUnit XML results**. These appear in the HTML as:
- `Test run crashed (exit code: NNN)` 
- `BuildFailure`

Capture these from the HTML and record them as separate failure types (CRASH, BUILD_FAILURE).

### Step 2.6a: Collect detailed information for build failures

For any failure that involves a **build error** (a test suite that fails to build, or a unit test that builds something and the build fails), collect as much detail as possible:

#### 1. Extract specific build error messages

When the NUnit failure message says something like `'dotnet build' failed with exit code 1`, that alone is not useful. Look for the actual compiler/linker/MSBuild errors in:
- The NUnit failure `message` and `stack-trace` elements (sometimes the full build output is captured there)
- The HtmlReport `index.html` — build errors are often shown inline
- The build step log (download via `az devops invoke --area build --resource logs`)

Look for patterns like:
- `error CS####:` (C# compiler errors)
- `error MT####:` / `error MM####:` (mtouch/mmp errors)
- `error MSB####:` (MSBuild errors)
- `error IL####:` (ILLink/trimmer errors)
- `error NETSDK####:` (SDK errors)

#### 2. Collect binlogs when available

Binlog files (`.binlog`) contain the full MSBuild log and are invaluable for diagnosing build failures. They are often available as build artifacts.

```bash
# List artifacts to find binlog-related ones
az pipelines runs artifact list --run-id <buildId> \
  --org https://devdiv.visualstudio.com --project DevDiv -o json \
  | python3 -c "import json,sys; [print(a['name']) for a in json.load(sys.stdin) if 'binlog' in a['name'].lower() or 'Binlog' in a['name']]"
```

If binlogs are inside the HtmlReport zip (common path: `tests/<suite>/<num>/*.binlog` or referenced in test output), extract them.

Binlogs may also be embedded in test failure messages or stack traces as file paths — note these paths for reference.

#### 3. Attach binlogs to issues

When filing an issue for a build failure:
- Download the relevant binlog file
- Zip it (GitHub doesn't allow `.binlog` attachments, but `.zip` is fine)
- Attach the zipped binlog to the issue using `gh issue comment` with the `--attach` flag, or by uploading via the GitHub API

```bash
# Download a binlog artifact
az pipelines runs artifact download \
  --artifact-name "<binlog-artifact-name>" \
  --path "/tmp/postmortem_binlogs/" \
  --run-id <buildId> \
  --org https://devdiv.visualstudio.com --project DevDiv

# Zip it for attachment
zip /tmp/postmortem_binlogs/build_<buildId>.binlog.zip /tmp/postmortem_binlogs/*.binlog

# Attach to issue (if the gh CLI version supports --attach, otherwise note the link)
```

#### 4. Include build errors in the issue body

Always include the specific build error messages in the issue body. Example:

```markdown
### Build Errors

The `dotnet build` step failed with the following errors:

```
error CS8602: Dereference of a possibly null reference. [src/Foo/Bar.csproj]
error MT0099: No platform assembly! [src/Baz/Qux.csproj]
```

**Binlog:** [build_14017033.binlog.zip](link-to-attachment) (attached)
```

This makes the issue actionable without requiring the reader to navigate through AzDO build logs.

### Step 2.7: For infrastructure/setup failures without TestSummary

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

### Step 2.8: Normalize failure signatures

Create a normalized signature for deduplication. **Important:** HTML entities in test names (e.g., `&quot;` vs `"`) must be normalized to avoid duplicate entries:

```python
import html as html_lib

def normalize_signature(failure_type, test_fullname, error_msg, platform):
    """Create a stable key for grouping the same logical failure."""
    # Normalize HTML entities
    if test_fullname:
        test_fullname = html_lib.unescape(test_fullname)
        return f"{failure_type}|{platform}|{test_fullname}"
    elif error_msg:
        error_msg = html_lib.unescape(error_msg)
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

### Step 3.3: Identify infrastructure failures and bot-specific issues

#### 3.3a: Extract worker/bot info from timelines

The timeline records contain `workerName` for each Job. Extract this to correlate failures with specific bots:

```python
for record in timeline['records']:
    if record['type'] == 'Job':
        worker = record.get('workerName', '')
        # Windows bots: "VSM-XAM-126" (no dot suffix)
        # macOS bots: "VSM-XAM-56.Sequoia.arm64", "VSCXSDKs-MINI-042.Tahoe.arm64"
```

#### 3.3b: Identify bot-specific failures

Group failures by worker and compute failure rates. A bot is problematic if:
- It has a disproportionate failure rate compared to other bots running the same job type
- The same error message appears on the same bot across multiple unrelated PRs

```python
# Example: if VSM-XAM-126 has 8/18 failed jobs (44%) while other bots average 5-10%,
# that bot has a specific problem worth filing an issue for.
```

#### 3.3c: Windows integration stage — identify the macOS bot

The 'Windows integration' stage has three jobs that work together:
1. **Reserve macOS bot for tests** — reserves a macOS bot and records its name
2. **Dotnet tests** — runs on a Windows bot, connecting to the reserved macOS bot via ssh
3. **Re-enable macOS bot for tests** — releases the macOS bot

If **any** job in this stage fails, always extract the macOS bot name from the 'Reserve macOS bot for tests' job's `workerName` and include it in the issue. This is critical because:
- A 'Verify ssh connection' failure on the Windows bot is really a problem with the **macOS bot** it's trying to reach
- A 'Download secrets' failure on the macOS bot is specific to that bot
- Correlating the macOS bot name across issues reveals patterns (e.g., VSM-XAM-13 having persistent problems)

```python
# For any failure in the Windows integration stage:
# 1. Find the 'Reserve macOS bot for tests' job in the timeline
# 2. Extract its workerName — this is the macOS bot
# 3. Include "macOS bot: <workerName>" in the issue, even if the
#    failure is in the 'Dotnet tests' job running on a Windows bot
for record in timeline['records']:
    if record['type'] == 'Job' and 'Reserve' in record.get('name', '') and 'macOS' in record.get('name', ''):
        macos_bot = record.get('workerName', 'unknown')
        break
```

#### 3.3d: Identify infrastructure failure patterns

Also look for cross-bot patterns that affect many PRs:
- **Timeouts**: jobs that time out on multiple different bots across unrelated PRs
- **REST API failures**: `Intermittent failure attempting to call the restapis` across many PRs
- **Provisioning failures**: `Reserve bot`, `provision` errors
- **Workload install failures**: `Install dotnet workloads` failing

**CRITICAL: Always identify the FIRST failed step as the root cause.** In any failed job, only the first step with `result == 'failed'` (and without `continueOnError: true`) is the root cause. All subsequent failures in the same job are cascading effects and must NOT be reported as separate issues. Common cascading patterns:
- `Publish Artifact: TestSummary/HtmlReport` → reports `Path does not exist` because tests never ran
- `Prepare tests results and Html Report` → fails because earlier steps didn't produce results
- Any step after a failed `Checkout`, `Verify ssh connection`, `Download secrets`, or `Install dotnet workloads`

To find the actual root cause in a failed job:
1. List all Task records under the job sorted by execution order
2. Find the **first** task with `result == 'failed'`
3. Verify this task does NOT have `continueOnError: true` — if it does, skip it and check the next failed task
4. That task is the root cause; all later failures in the same job are cascading
5. **Never file an issue for a cascading failure** — always file for the root cause step

```sql
SELECT error_signature, failure_type, raw_message,
       COUNT(DISTINCT build_id) as occurrences
FROM ci_failures
WHERE failure_type = 'Infrastructure'
   OR raw_message LIKE '%provision%'
   OR raw_message LIKE '%reserve bot%'
   OR raw_message LIKE '%timeout%'
   OR raw_message LIKE '%Intermittent failure%'
   OR raw_message LIKE '%Path does not exist%'
GROUP BY error_signature
ORDER BY occurrences DESC;
```

### Step 3.4: Exclude known-noisy and PR-specific failures

**Always exclude these tests** — they are expected to fail across many PRs and are not actionable:
- `Xamarin.Tests.AppSizeTest.*` — sensitive to any API change, expected cross-PR failures

A failure is PR-specific if:
- It appears in only 1 PR
- It persists across commits within that PR (not a rerun flake)
- It is consistent (never passes on rerun)

These should be **excluded** from issue filing — they are the PR author's problem.

### Step 3.5: File one issue per test

**Always create separate issues for separate unit tests.** It is easier to merge issues than to split them up. Do not group multiple unrelated test failures into a single issue.

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
  --label "ci-postmortem" --json number,title,labels,url
```

Also search closed issues (may need reopening):

```bash
gh issue list --repo dotnet/macios --state closed \
  --search "<test_fullname or key error phrase>" \
  --label "ci-postmortem" --json number,title,labels,url
```

### Step 4.2: Decide whether to reopen closed issues

When a matching **closed** issue is found, apply these rules to decide whether to reopen it:

1. **Check the close reason.** Read the issue body/comments to determine *why* it was closed:
   - **Fix merged** — a code change was merged to fix the problem.
   - **Lack of information** — closed because there wasn't enough data to act on.
   - **Debug instrumentation merged** — a PR was merged to gather more diagnostic info.

2. **If closed because a fix was merged:**
   - **Do NOT reopen if the issue was closed less than 2 weeks ago.** The failing builds in the analysis window likely predate the fix. Comment on the closed issue with the analysis results and note why it's not being reopened.
   - **Do NOT reopen unless the new failing build is from the `main` branch** (or targets `main` via a PR that incorporates the fix commit). Builds from older branches or PRs that branched before the fix don't count.
   - **After 2 weeks**, if the failure is still appearing in builds that incorporate the fix, reopen the issue.

3. **If closed for lack of information:** reopen if the new analysis provides that missing information.

4. **If closed because debug instrumentation was merged:** reopen if any of the failing builds provide the additional diagnostic data that was being collected.

5. **Always OK to comment** on a closed issue with analysis data, even if not reopening. Include a note explaining why the issue is not being reopened (e.g., "Not reopening — the fix in #NNNN was merged on DATE, and all failing builds predate that fix.").

### Step 4.3: Propose actions to the user

Present a list of proposed actions **before executing any**. Use `ask_user` to get confirmation.

For each failure, propose one of:
- **Create new issue** — no existing issue found
- **Comment on existing issue** — matching open issue found, add recent occurrence data
- **Reopen issue** — matching closed issue found, failure confirmed post-fix (see Step 4.2)
- **Comment on closed issue (no reopen)** — matching closed issue found, but reopen criteria not met
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
  --label "bug,CI,ci-postmortem,copilot,flaky-test" \
  --body "$(cat <<'EOF'
## Flaky Test Report (automated)

**Test:** `<test_fullname>`
**Platform:** <platform>
**Category:** Flaky / Shared Regression / Infrastructure
**Period:** <start_date> to <end_date>

### Occurrence Summary

| PR | Build | Bot | Direct Link |
|----|-------|-----|-------------|
| #<pr> | <buildId> | <workerName> | [<job_name> → <task_name>](<deep_link_url>) |

**Total:** Failed in <N> builds across <M> PRs

**Deep links:** Always link to the specific job and step/task, not just the build. Use the AzDO URL format:
`https://devdiv.visualstudio.com/DevDiv/_build/results?buildId=BUILD_ID&view=logs&j=JOB_RECORD_ID&t=TASK_RECORD_ID`

The `j=` (job) and `t=` (task) parameters are the `id` fields from the timeline records. This takes the reader directly to the failing log rather than requiring them to click through multiple jobs.

### Error Details

Include the **specific error messages** from the NUnit XML failure messages. If the failure is a build error, include the actual compiler/linker error codes and messages. If different PRs/builds show different error messages for the same test, list them separately — they may be different root causes.

For **build failures** specifically, always include:
1. The actual build error messages (error codes like CS####, MT####, IL####, MSB####)
2. Links to or attachments of binlog files (zipped) when available
3. The full `dotnet build` command that failed (from the test failure message)

```
<Specific error message from NUnit XML failure/message element>
<Include compiler errors like "error CS8602: ..." or linker errors like "error MT0099: ...">
<Include assertion failures like "Expected: True But was: False">
```

If different builds have different errors for the same test, show each variant:

**Variant A** (builds <list>):
```
<error message A>
```

**Variant B** (builds <list>):
```
<error message B>
```

**Important:** If different PRs show different error messages for the "same" test failure, they are likely **different root causes** and should be investigated separately. Consider splitting into separate issues or noting that the grouping may be incorrect.

### Classification

This failure was identified as **flaky** because:
- It appeared across <M> unrelated PRs
- It disappeared on rerun in <K> cases

---
*This issue was automatically generated by CI post-mortem analysis.*
EOF
)"
```

All issues **must** have the `ci-postmortem` and `copilot` labels. Additionally use `flaky-test` for flaky tests and `infrastructure` for infra issues.

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
