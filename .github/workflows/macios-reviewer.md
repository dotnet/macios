---
on:
  slash_command:
    name: review
    events: [pull_request_comment]
  roles: [admin, maintain, write]

# ###############################################################
# Select a PAT from the pool and override COPILOT_GITHUB_TOKEN.
# Run agentic jobs in the existing `gh-aw-environment` environment.
#
# When org-level billing is available, this will be removed.
# See `shared/pat_pool.README.md` for more information.
# ###############################################################
imports:
  - uses: shared/pat_pool.md
    with:
      environment: gh-aw-environment

environment: gh-aw-environment
concurrency:
  group: "macios-reviewer-${{ github.event.issue.number || github.event.pull_request.number || github.run_id }}"
  cancel-in-progress: false
permissions:
  contents: read
  pull-requests: read
engine:
  id: copilot
  model: claude-sonnet-4.5
  env:
    COPILOT_GITHUB_TOKEN: |
      ${{ case(
        needs.pat_pool.outputs.pat_number == '0', secrets.COPILOT_PAT_0,
        needs.pat_pool.outputs.pat_number == '1', secrets.COPILOT_PAT_1,
        needs.pat_pool.outputs.pat_number == '2', secrets.COPILOT_PAT_2,
        needs.pat_pool.outputs.pat_number == '3', secrets.COPILOT_PAT_3,
        needs.pat_pool.outputs.pat_number == '4', secrets.COPILOT_PAT_4,
        needs.pat_pool.outputs.pat_number == '5', secrets.COPILOT_PAT_5,
        needs.pat_pool.outputs.pat_number == '6', secrets.COPILOT_PAT_6,
        needs.pat_pool.outputs.pat_number == '7', secrets.COPILOT_PAT_7,
        needs.pat_pool.outputs.pat_number == '8', secrets.COPILOT_PAT_8,
        needs.pat_pool.outputs.pat_number == '9', secrets.COPILOT_PAT_9,
        'NO COPILOT PAT AVAILABLE')
      }}
network:
  allowed:
    - defaults
    - dotnet
    - github
    - "aka.ms"
    - "dev.azure.com"
    - "microsoft.com"
    - "vsassets.io"
tools:
  github:
    github-token: ${{ secrets.GITHUB_TOKEN }}
    toolsets: [pull_requests, repos]
    min-integrity: approved
safe-outputs:
  github-token: ${{ secrets.GITHUB_TOKEN }}
  create-pull-request-review-comment:
    max: 50
  submit-pull-request-review:
    max: 1
    allowed-events: [COMMENT]
    supersede-older-reviews: true
---

# .NET for Apple Platforms PR Reviewer

A maintainer commented `/review` on this pull request. Perform a thorough code review following the dotnet/macios review guidelines.

## Instructions

1. Read the review rules from `.github/skills/macios-reviewer/references/review-rules.md` — these contain the detailed patterns and anti-patterns to check for.
2. Read the review methodology from `.github/skills/macios-reviewer/SKILL.md` — this defines the review workflow, mindset, severity levels, and comment format.
3. Follow the skill's workflow to analyze the pull request:
   - Gather context: read the diff and changed files
   - For each changed file, read the **full source file** to understand surrounding context
   - Form an independent assessment before reading the PR description
   - Read the PR title and description — treat claims as things to verify
   - Check CI status
   - Analyze the diff against the review rules
4. Post your findings as inline review comments and a review summary.

## Constraints

- Only comment on added/modified lines visible in the diff.
- One issue per inline comment.
- If the same issue appears many times, flag it once listing all affected files.
- Don't flag what CI catches (compiler errors, linter issues).
- Don't review C# code formatting — it is handled automatically.
- Avoid false positives — verify concerns given the full file context.
- **Never submit an APPROVE event.** Always use COMMENT — never REQUEST_CHANGES.
- Prioritize: bugs > breaking changes > binding correctness > safety > performance > missing tests > duplication > consistency > documentation.
- Ignore comments from the user 'vs-mobiletools-engineering-service2'.
