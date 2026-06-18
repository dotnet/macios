---
on:
  workflow_dispatch:
permissions:
  contents: read
  issues: read
engine:
  id: copilot
  model: claude-sonnet-4.5
network:
  allowed:
    - defaults
    - dotnet
    - github
    - "aka.ms"
    - "dev.azure.com"
    - "devdiv.visualstudio.com"
    - "microsoft.com"
    - "vsassets.io"
tools:
  github:
    toolsets: [issues, repos]
    min-integrity: none
safe-outputs:
  create-issue:
    max: 20
  add-comment:
    max: 20
  update-issue:
    max: 20
---

# CI Post-Mortem Analysis

Perform a weekly post-mortem analysis of CI failures across recent PRs in dotnet/macios to identify flaky tests, infrastructure issues, and shared regressions that are not caused by any specific PR.

## Instructions

1. Read the skill definition from `.agents/skills/macios-ci-postmortem/SKILL.md` — this contains the full 4-phase workflow.
2. Read the Azure DevOps CLI reference from `.agents/skills/macios-ci-postmortem/references/azure-devops-cli.md`.
3. Execute all four phases of the workflow:
   - **Phase 1: Discovery** — collect all PR-validation builds from the last 7 days
   - **Phase 2: Extraction** — download TestSummary artifacts for triage, then HtmlReport artifacts only for jobs with test failures, and parse NUnit XML for individual test-level failures
   - **Phase 3: Classification** — categorize failures as flaky (cross-PR or rerun-recovered), infrastructure (bot-specific or cross-bot), or PR-specific (exclude these). Also exclude `AppSizeTest` failures.
   - **Phase 4: Issue Actions** — search for existing `ci-postmortem` issues, then file new issues or comment on existing ones
4. All issues must have the `ci-postmortem` and `copilot` labels.
5. File one issue per distinct test failure — do not group unrelated test failures together.
6. For infrastructure issues, check if failures are concentrated on specific bots by extracting `workerName` from build timelines.

## Constraints

- Only file issues for failures that appear across 2+ unrelated PRs, or that are confirmed flaky by rerun recovery (same commit, different outcome).
- Never file issues for PR-specific failures — those are the PR author's responsibility.
- Always search for existing `ci-postmortem` issues before creating new ones. Comment on existing issues if the failure is already tracked.
- Always exclude `AppSizeTest` failures — they are expected to fail across PRs.
