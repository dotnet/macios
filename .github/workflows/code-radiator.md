---
on:
  schedule:
    cron: "0 0 * * *"
  workflow_dispatch:
  roles: [admin, maintainer, write]
permissions:
  contents: write
  pull-requests: write
engine:
  id: copilot
  model: claude-sonnet-4.5
network:
  allowed:
    - defaults
    - github
tools:
  github:
    toolsets: [pull_requests, repos]
    min-integrity: none
  bash:
    enabled: true
---

# Code Radiator

Flow code from `main` into active target branches by creating merge PRs.

## Instructions

1. Read the detailed workflow from `.github/skills/code-radiator/SKILL.md`.
2. Follow the skill's workflow to:
   - Fetch remote refs and identify target branches with recent activity (last 30 days).
   - For each target branch, merge `main` in and create or update a pull request.
   - Resolve `eng/Version.Details.props` and `eng/Version.Details.xml` conflicts by picking the highest version for each dependency.
3. Report a summary of what was done.

## Constraints

- Target branches match: `net[0-9]*.0`, `xcode[0-9]*`, or `xcode[0-9]*.[0-9]*`.
- Only process branches with commits in the last 30 days.
- Local branch name: `merge/main-to-<target>-<yyyyMMdd>`.
- PR title: `🤖 Merge 'main' => '<target>'`.
- If an existing non-draft PR exists for a target, update it (push to its head branch).
- If an existing draft PR exists, add a comment and skip.
- Enable automerge (merge strategy) on newly created PRs.
- Never force push.
