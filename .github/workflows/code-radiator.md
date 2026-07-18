---
on:
  schedule: daily
  workflow_dispatch:
  roles: [admin, maintain, write]
  permissions: {}

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

concurrency:
  group: "code-radiator-${{ github.ref || github.run_id }}"
  cancel-in-progress: true
permissions:
  contents: read
  pull-requests: read
environment: gh-aw-environment
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
    - github
tools:
  github:
    github-token: ${{ secrets.GITHUB_TOKEN }}
    toolsets: [pull_requests, repos]
    min-integrity: approved
  bash: true
checkout:
  github-token: ${{ secrets.GITHUB_TOKEN }}
  fetch: ["*"]
  fetch-depth: 0
pre-agent-steps:
  # The agent runs in a firewall sandbox where the `gh` CLI is NOT authenticated,
  # so it cannot list milestones itself. Fetch the closed milestones here (this
  # step runs on the runner, which has a token) and write them to a file in the
  # shared agent directory (/tmp/gh-aw is bind-mounted read-write into the sandbox).
  - name: Fetch closed milestones
    env:
      GH_TOKEN: ${{ secrets.GITHUB_TOKEN }}
      REPO: ${{ github.repository }}
    run: |
      set -euo pipefail
      # Fail hard on any error: the closed-milestone list is a safety gate that
      # prevents creating merge PRs for inactive branches. An empty or partial
      # list must never be silently accepted, so do NOT swallow errors here.
      mkdir -p /tmp/gh-aw/code-radiator
      gh api "repos/${REPO}/milestones?state=closed&per_page=100" --paginate -q '.[].title' \
        > /tmp/gh-aw/code-radiator/closed-milestones.txt
      echo "Closed milestones:"
      cat /tmp/gh-aw/code-radiator/closed-milestones.txt
safe-outputs:
  github-token: ${{ secrets.GITHUB_TOKEN }}
  max-patch-files: 1000
  max-patch-size: 10240 # this is the maximum, bigger PRs must be created manually
  create-pull-request:
    max: 10
    draft: false
    patch-format: bundle
    signed-commits: false
    allowed-base-branches:
      - "xcode*"
      - "xcode*.*"
  add-comment:
    max: 10
    target: "*"
  add-labels:
    max: 10
    target: "*"
  merge-pull-request:
    max: 10
  push-to-pull-request-branch:
    max: 10
    signed-commits: false
    target: "*"
    required-title-prefix: "🤖 Merge 'main' => '"
    protected-files: allowed
  update-pull-request:
    max: 10
  close-pull-request:
    max: 10
  create-issue:
    max: 10
---

# Code Radiator

Merge code from `main` into active target branches, creating pull requests for each.

## Target Branch Patterns

Only consider remote branches matching these patterns:
- `xcode*` (e.g., `xcode26`)
- `xcode*.*` (e.g., `xcode26.4`)

Only process branches that have had commits in the last 30 days.

## Workflow

### 1. Identify Target Branches

```bash
# List remote branches matching target patterns with recent activity
git fetch origin
git for-each-ref --sort=-committerdate --format='%(refname:short) %(committerdate:iso8601)' refs/remotes/origin/
```

Filter to branches matching the patterns above AND having a commit within the last month.

#### Milestone-based filtering

After identifying candidate branches, check whether each branch has a corresponding
closed milestone. If so, skip the branch — a closed milestone signals that the branch
is no longer actively developed.

The list of **closed** milestone titles has already been fetched for you by a trusted
`pre-agent-steps` step and written to a file. The agent sandbox cannot authenticate the
`gh` CLI, so **do not** call `gh api` yourself — just read the file:

```bash
cat /tmp/gh-aw/code-radiator/closed-milestones.txt
```

Each line is one closed milestone title (the file is empty if there are no closed
milestones).

> **Fail-safe (critical)**: This check exists to *prevent* creating PRs for inactive
> branches, so it must never be silently skipped. If the file
> `/tmp/gh-aw/code-radiator/closed-milestones.txt` does **not** exist or cannot be read,
> **abort milestone filtering and do not create any PRs for this run** — report the
> failure in the summary instead. (An empty file is valid and means there are no closed
> milestones.) It is better to create no PRs than to create a PR for a branch whose
> milestone is already closed.

Map each branch name to its milestone name:

| Branch pattern   | Milestone name                                                            |
|------------------|---------------------------------------------------------------------------|
| `xcode<version>` | `xcode<version>` (e.g., `xcode26.4` → `xcode26.4`, `xcode26` → `xcode26`) |

If the corresponding milestone is found in the closed list, skip the branch and include
it in the summary as "skipped (milestone closed)".

### 2. For Each Target Branch

#### a. Determine the local branch name

The local branch name is: `merge/main-to-<target>-<yyyyMMdd>` (e.g., `merge/main-to-net11.0-20260506`).

#### b. Check for existing pull requests

Search for an open PR with:
- Base: the target branch
- Title matching: `🤖 Merge 'main' => '<target>'`

If a matching PR exists:
- If it is a **draft**: check whether a manual merge from `main` into the target branch occurred after the PR was created (see below). If so, close the PR and proceed to create a new one. Otherwise, add a comment saying "⏭️ Skipping merge update: this PR is a draft. Convert to ready when you want automated updates to resume." and **skip** this target.
- If it is **not a draft**: check whether a manual merge from `main` into the target branch occurred after the PR was created (see below). If so, close the PR and proceed to create a new one. Otherwise, use its head branch name as the local branch name (to update the existing PR).

##### Detecting manual merges that supersede an existing PR

After finding an existing workflow-created PR, check if someone manually merged `main`
into the target branch after the PR was created. If so, the PR is stale and should be
replaced with a fresh one.

Detection logic:

```bash
# Find merge commits from main into the target branch that are newer than the PR creation date
git log "origin/<target>" --merges --first-parent --after="<pr-created-at>" --format="%H %s" |
  grep -iE "merge.* main " || true
```

The `|| true` prevents the command from failing when there are no matches.
The pattern matches "main" as a whole word to avoid false positives from branch names
like "maintenance".

If any such merge commits exist on the target branch:

1. Add a comment to the existing PR: "🔄 Closing this PR because a manual merge from `main` into `<target>` was done after this PR was created (commit `<sha>`). A fresh merge PR will be created."
2. Close the existing PR.
3. Proceed as if no existing PR was found (create a new branch and PR from scratch).

#### c. Update from target branch

If updating an existing PR, first merge the target branch into the PR branch to incorporate any new commits from the target:

```bash
git checkout -B "<local-branch>" "origin/<local-branch>"
git merge "origin/<target>" --no-edit -m "Merge branch '<target>' into '<local-branch>'"
```

If creating a new branch, start from the target:

```bash
git checkout -B "<local-branch>" "origin/<target>"
```

#### d. Merge main

```bash
git merge origin/main --no-edit -m "Merge branch 'main' into '<target>'"
```

#### e. Resolve merge conflicts

If there are merge conflicts:

**For files under `tests/dotnet/UnitTests/expected/`:**
- Do not include these files in the merge commit at all. Remove them from the index:
  ```bash
  git rm --cached <conflicting-file>
  ```

**For `eng/Version.Details.props` or `eng/Version.Details.xml`:**
- Parse both the `origin/main` and `origin/<target>` versions of the file as XML.
- For each `<Dependency>` element present in both files, keep the one with the **higher** `<Version>` value.
- Use semantic version comparison (split on `.`, `-`, `+` and compare numerically).
- Write the merged result and `git add` the file.

**For `NuGet.config`:**
- Include all package source feeds from both the source (main) and target branches.
- If a feed exists in both with the same key but different URL, keep both (rename the key from main to avoid collision).
- Write the merged result and `git add` the file.

**For any other conflicting files:**
- Do your best to resolve them using context and judgment.
- If you resolved any "other" conflicts (not covered by the rules above), mark the PR for human review:
  - Do **not** enable automerge (and disable it if already enabled).
  - Add the `do-not-merge` label.
  - Add a comment requesting human review of the conflict resolution, listing which files were manually resolved.

##### After resolving all conflicts

Complete the merge in a single step:

```bash
git commit --no-edit
```

> **Critical**: Stage ALL resolved files first, then run `git commit --no-edit` exactly once. Never commit one file at a time while in merge state — the first `git commit` would complete the merge and clear `MERGE_HEAD`, turning any subsequent commits into plain single-parent commits.

Verify the resulting commit is a proper merge commit with **two** parent SHAs:

```bash
git log --format="%P" -1
```

The output must contain **two** space-separated SHA hashes. If only one SHA is shown, `MERGE_HEAD` was lost during conflict resolution (e.g. due to a `git reset` or `git checkout`). The branch then contains a plain single-parent commit instead of a merge commit. A plain commit causes `git format-patch` to include all commits since the branch diverged from `main` — potentially tens of thousands — which will exceed the buffer limit and fail PR creation. Discard the branch and restart from step c.

#### e. Create or update the PR

Do **NOT** run `git push` manually. The safeoutputs tool handles pushing.

Use the `create_pull_request` safeoutput tool to push the branch and create/update the PR:
- `branch`: `<local-branch>` (e.g., `merge/main-to-net11.0-20260527`)
- `base`: `<target>` (e.g., `net11.0`) — **always provide this field**
- `title`: `🤖 Merge 'main' => '<target>'`
- `body`: `Automated merge of \`main\` into \`<target>\`.\n\nCreated by the code-radiator workflow.`

> **Important**: Do NOT unset the upstream tracking branch. After `git checkout -B "<local-branch>" "origin/<target>"`, the upstream is set to `origin/<target>`. Keep it set — the safeoutputs tool relies on this to detect the commits to push.

After creating the PR, enable automerge (merge strategy) using the GitHub MCP `enable_auto_merge` tool or `gh pr merge --auto --merge`.

#### f. Fallback: file an issue if PR creation fails

If the `create_pull_request` safeoutput tool fails (e.g., due to permission errors, branch
protection rules, or other unexpected errors), file a GitHub issue instead so the failure
is tracked and can be resolved manually.

Use the `create_issue` safeoutput tool with:
- `title`: `🤖 Code radiator: failed to create merge PR for '<target>'`
- `body`: Include:
  - The target branch name.
  - The source branch (`main`).
  - The local branch name that was prepared.
  - The error message from the failed PR creation attempt.
  - A note that this issue was automatically filed by the code-radiator workflow.
- `labels`: `["code-radiator"]`

Do not fail the entire workflow run — continue processing the remaining target branches.

### 3. Summary

After processing all branches, report:
- Which PRs were created (with links)
- Which PRs were updated
- Which PRs were closed and recreated (due to manual merges superseding them)
- Which branches had PR creation failures (with links to the filed issues)
- Which branches were skipped (closed milestone, draft PRs, no conflicts resolution possible)
- Which branches had no diff (main already merged)

## Conflict Resolution Details

### Version file merge algorithm

For `eng/Version.Details.props` and `eng/Version.Details.xml`:

1. Parse both XML files.
2. Build a map of `Dependency[@Name]` → `Version` text from each file.
3. For each dependency present in either file, select the higher version.
4. Use the target branch's file as the base structure, updating versions where main has higher values.
5. Dependencies that exist only in main should be added to the result.

### Version comparison

Split version strings on `.`, `-`, and `+`. Compare each segment numerically. Example:
- `9.0.0-preview.1.24080.9` vs `9.0.0-preview.2.24101.3` → second is higher.

## Important Notes

- Never force push.
- Do NOT run `git push` manually — the safeoutputs `create_pull_request` tool handles pushing.
- Do NOT unset the upstream tracking branch after creating the local branch. The safeoutputs tool uses `@{upstream}` to detect commits that need to be pushed. Unsetting the upstream causes the tool to report "No changes to commit - no commits found" even when commits exist.
- Always provide the `base` branch when calling `safeoutputs create_pull_request` (e.g., `base: "net11.0"`).
- The workflow operates on the current repository checkout.
- Run `git fetch origin` before starting to ensure up-to-date remote refs.
- Use the GitHub MCP tools or `gh` CLI for non-push PR operations (comment, list, merge --auto, enable automerge).
