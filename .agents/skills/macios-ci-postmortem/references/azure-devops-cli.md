# Azure DevOps CLI Reference for macios CI

## Authentication

The `az devops` CLI must be authenticated. Typically this is done via:
```bash
az devops configure --defaults organization=https://devdiv.visualstudio.com project=DevDiv
```

Or by passing `--org` and `--project` on each command.

## Key Commands

### Build metadata
```bash
az pipelines build show --id <buildId> -o json
```
Returns: result, status, sourceBranch, definition, requestedFor, startTime, finishTime.

### Build timeline (jobs and tasks)
```bash
az devops invoke --area build --resource timeline \
  --route-parameters project=DevDiv buildId=<buildId> \
  --org https://devdiv.visualstudio.com -o json
```
Returns: records array with type (Stage/Job/Task), name, result, state, log.id, parentId.

**Important:** `az pipelines build log list` is NOT a valid command. Use the `az devops invoke` approach above.

### Task logs
```bash
az devops invoke --area build --resource logs \
  --route-parameters project=DevDiv buildId=<buildId> logId=<logId> \
  --org https://devdiv.visualstudio.com -o json
```
Returns: value array of log line strings.

### Artifact listing
```bash
az pipelines runs artifact list --run-id <buildId> -o json
```

### Artifact download
```bash
az pipelines runs artifact download \
  --artifact-name "<name>" \
  --path /tmp/ci-artifacts/ \
  --run-id <buildId>
```

## Common Pipeline Names

- `xamarin-macios-sim-pr-tests` — PR validation with simulator tests
- Other pipeline names may vary; check `definition.name` from build show.

## Common Job Names in Timeline

- `T: monotouch_ios` — iOS monotouch tests
- `T: monotouch_tvos` — tvOS monotouch tests
- `macOS tests` — macOS and Mac Catalyst tests
- `Reserve macOS bot for tests` — bot provisioning
- Various build/packaging jobs

## JSON Parsing Caveat

`az devops invoke` output may include trailing non-JSON text. Always parse with:
```python
import json
with open('file.json', 'r') as f:
    content = f.read()
data = json.JSONDecoder().raw_decode(content)[0]
```

Do NOT use `json.loads(content)` directly — it will fail on the trailing text.

## Test Artifact Names

TestSummary and HtmlReport artifacts follow a naming convention:
- `TestSummary-simulator_tests<jobname>-1` — Markdown summary with pass/fail counts and failure details
- `HtmlReport-simulator_tests<jobname>-1` — ZIP containing HTML report and NUnit XML files

Common job names:
- `monotouch_ios`, `monotouch_tvos`, `monotouch_macos`, `monotouch_maccatalyst`
- `dotnettests_ios`, `dotnettests_tvos`, `dotnettests_macos`, `dotnettests_maccatalyst`
- `cecil`, `framework`, `xtro`, `msbuild`, `generator`, `sharpie`, `fsharp`, `linker`
- `introspection`, `xcframework`, `interdependent_binding_projects`

**Important:** Each artifact download overwrites `TestSummary.md` in the target directory. Always download to separate subdirectories named after the artifact.

## Key Investigation Strategy

1. **Start with TestSummary artifacts** — they are the fastest way to identify what failed and why. Raw task logs are 40K+ lines and don't contain standard NUnit patterns inline.
2. **For test failures (not build failures)**, download HtmlReport artifacts and parse the NUnit XML files inside for exact test names, assertions, and stack traces.
3. **Only use raw task logs** when you need build error details (MSB/CS/NU errors) or infrastructure error context.
4. **Map timeline logIds to jobs** using the `parentId` field to trace task → job relationships.
