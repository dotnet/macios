---
name: macios-ci-failure-inspector
description: Investigate and triage CI failures for dotnet/macios from Azure DevOps build URLs. Use this skill whenever the user shares a DevOps build link, asks about CI failures, wants to understand why a build failed, or asks to investigate test failures on any platform (iOS, tvOS, macOS, Mac Catalyst). Also use when the user says things like "CI is red", "tests are failing", "build broke", or "what happened in CI".
---

# macios CI Failure Inspector

Investigate Azure DevOps CI failures for the dotnet/macios repository, extract root causes, and report findings.

## References

Read these as needed during investigation:

- `references/azure-devops-cli.md` — az CLI commands, artifact naming conventions, and JSON parsing caveats. Read this when you need to construct `az` commands or download artifacts.

## Inputs

Collect from the user:

- **Build URL** — an Azure DevOps build results link, e.g. `https://devdiv.visualstudio.com/DevDiv/_build/results?buildId=<ID>&view=results`
- **Scope** — whether to investigate only, or also attempt fixes (default: investigate only)

Extract the `buildId` from the URL query parameter.

## Investigation workflow

### Phase 1: Build overview

Fetch the build metadata to understand the big picture:

```bash
az pipelines build show --id <buildId> --org https://devdiv.visualstudio.com --project DevDiv -o json > /tmp/build_show.json
```

Extract from the output:
- `result` (succeeded, failed, partiallySucceeded, canceled)
- `sourceBranch` — what branch triggered the build
- `definition.name` — which pipeline ran
- `triggerInfo` or `reason` — what triggered it (PR, push, schedule)

If the build succeeded, tell the user and stop.

### Phase 2: Timeline — identify failing jobs and tasks

The timeline gives you every job and task in the build with its result:

```bash
az devops invoke --area build --resource timeline --route-parameters project=DevDiv buildId=<buildId> --org https://devdiv.visualstudio.com -o json > /tmp/build_timeline.json
```

Parse the timeline to find failed records. Use Python for robust JSON parsing because `az devops invoke` output can include trailing non-JSON text:

```python
import json

with open('/tmp/build_timeline.json', 'r') as f:
    content = f.read()
data = json.JSONDecoder().raw_decode(content)[0]

failed = [r for r in data.get('records', []) if r.get('result') == 'failed']
for r in failed:
    print(f"  [{r['type']}] {r['name']} (id={r['id']}, logId={r.get('log', {}).get('id', 'N/A')})")
```

Group failures into categories:
- **Test failures** — tasks named "Run tests" or jobs like `T: monotouch_ios`, `T: monotouch_tvos`, `macOS tests`
- **Infrastructure failures** — tasks like "Provision Xcode", "Reserve bot", setup tasks
- **Build failures** — compilation or packaging tasks

### Phase 3: Download TestSummary artifacts (primary failure source)

The xharness test runner logs are 40K+ lines and don't contain standard NUnit failure patterns inline. **TestSummary artifacts are the fastest and most reliable way to identify failures.** Always start with these before digging into raw logs.

List all artifacts:

```bash
az pipelines runs artifact list --run-id <buildId> --org https://devdiv.visualstudio.com --project DevDiv -o json
```

Download TestSummary artifacts for each failing job. **Each artifact must go to a separate directory** to avoid overwriting (they all contain a file named `TestSummary.md`):

```bash
artifact="TestSummary-simulator_testsmonotouch_macos-1"
mkdir -p "/tmp/ci-artifacts/${artifact}"
az pipelines runs artifact download \
  --artifact-name "$artifact" \
  --path "/tmp/ci-artifacts/${artifact}" \
  --run-id <buildId> \
  --org https://devdiv.visualstudio.com --project DevDiv
cat "/tmp/ci-artifacts/${artifact}/TestSummary.md"
```

The TestSummary.md file contains a structured markdown report with:
- Count of passed/failed tests
- For each failure: test configuration name, failure type (BuildFailure, Failed, Crashed, TimedOut), and brief error message
- Build failures show the configuration variant (e.g. "monotouch-test/macOS/Debug (ARM64): BuildFailure")
- Test failures may include the failing test class and assertion message

Common artifact names map to timeline jobs:
- `TestSummary-simulator_testsmonotouch_ios-1` → monotouch_ios
- `TestSummary-simulator_testsmonotouch_tvos-1` → monotouch_tvos
- `TestSummary-simulator_testsmonotouch_macos-1` → monotouch_macos
- `TestSummary-simulator_testsmonotouch_maccatalyst-1` → monotouch_maccatalyst
- `TestSummary-simulator_testsdotnettests_ios-1` → dotnettests_ios
- `TestSummary-simulator_testsdotnettests_tvos-1` → dotnettests_tvos
- `TestSummary-simulator_testsdotnettests_macos-1` → dotnettests_macos
- `TestSummary-simulator_testsdotnettests_maccatalyst-1` → dotnettests_maccatalyst

Download these in parallel for all failing jobs to save time.

### Phase 4: Get detailed test failure info from HtmlReport artifacts

For test failures (not build failures), download the corresponding HtmlReport artifact to get NUnit XML with exact test names, assertion messages, and stack traces:

```bash
artifact="HtmlReport-simulator_testsmonotouch_tvos-1"
mkdir -p "/tmp/ci-artifacts/${artifact}"
az pipelines runs artifact download \
  --artifact-name "$artifact" \
  --path "/tmp/ci-artifacts/${artifact}" \
  --run-id <buildId> \
  --org https://devdiv.visualstudio.com --project DevDiv
cd "/tmp/ci-artifacts/${artifact}" && unzip -o HtmlReport.zip -d htmlreport
```

Parse the NUnit XML files inside for specific test failures:

```python
import xml.etree.ElementTree as ET
import glob

xml_files = glob.glob('htmlreport/tests/monotouch-test/*/test-ios-*.xml')
for xf in sorted(xml_files):
    tree = ET.parse(xf)
    for tc in tree.getroot().iter('test-case'):
        if tc.get('result') == 'Failed':
            fullname = tc.get('fullname', 'unknown')
            msg_el = tc.find('.//message')
            msg = msg_el.text[:200] if msg_el is not None and msg_el.text else ''
            trace_el = tc.find('.//stack-trace')
            trace = trace_el.text[:300] if trace_el is not None and trace_el.text else ''
            print(f"FAILED: {fullname}")
            print(f"  Message: {msg}")
            if trace:
                print(f"  Stack: {trace}")
```

### Phase 5: Extract build error details from raw logs (for BuildFailure cases)

Only use raw task logs when TestSummary shows BuildFailure and you need the specific compiler/build error. The logs are typically 40K+ lines — search narrowly:

```bash
az devops invoke --area build --resource logs \
  --route-parameters project=DevDiv buildId=<buildId> logId=<logId> \
  --org https://devdiv.visualstudio.com -o json > /tmp/build_log.json
```

Parse and search for build errors only:

```python
import json

with open('/tmp/build_log.json', 'r') as f:
    data = json.JSONDecoder().raw_decode(f.read())[0]
lines = data.get('value', [])

for i, line in enumerate(lines):
    s = line.strip()
    if ': error MSB' in s or ': error CS' in s or ': error NU' in s:
        print(f"L{i}: {s[:300]}")
    elif 'cannot execute tool' in s or 'MetalToolchain' in s:
        print(f"L{i}: {s[:300]}")
    elif 'Build FAILED' in s:
        print(f"L{i}: {s[:300]}")
```

The xharness summary section near the end of the log also provides a task-level overview. Search backwards from the end for `Summary:`:

```python
for i in range(len(lines)-1, -1, -1):
    if 'Summary:' in lines[i]:
        for j in range(i, min(len(lines), i+10)):
            print(lines[j].strip())
        break
```

This shows `Executed N tasks`, `Succeeded: N`, `Failed: N`, `Crashed: N`, etc.

### Phase 6: Categorize and report

Group all findings by category and severity. Use this report structure:

```
## CI Failure Report — Build <buildId>

**Pipeline:** <pipeline-name>
**Branch:** <branch>
**Result:** <result>

### Failing Jobs

#### <Job Name> (logId: <id>)
- **Category:** Test failure | Infrastructure | Build error
- **Failing tests:**
  - `<TestClass.TestMethod>` — <assertion message>
  - ...
- **Root cause:** <concise explanation>

### Infrastructure Issues
- <any provisioning/bot/setup failures>

### Summary
- Total failing jobs: N
- Test failures: N (list unique test names)
- Infrastructure failures: N
- Recommended actions: ...
```

## Troubleshooting the investigation

### `az devops invoke` returns non-JSON output
The output can contain trailing text after the JSON object. Always use `json.JSONDecoder().raw_decode()` for parsing rather than `json.loads()`.

### Timeline has many records
Filter by `result == 'failed'` first. If you need to understand job hierarchy, use the `parentId` field to trace task → job → stage relationships.

### Artifact download fails
Some artifacts may only be available for a limited time or may require specific permissions. If download fails, fall back to log-based analysis.

### Multiple build URLs provided
Investigate each build independently but cross-reference failures — if the same test fails across multiple builds, it's likely a real regression rather than flakiness.
