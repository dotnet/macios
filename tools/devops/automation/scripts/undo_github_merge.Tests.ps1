# Copyright (c) Microsoft Corporation.
# Licensed under the MIT License.

Describe 'Queued PR checkout' {
    BeforeAll {
        $scriptPath = Join-Path $PSScriptRoot 'undo_github_merge.ps1'
    }

    BeforeEach {
        $repository = Join-Path $TestDrive ([guid]::NewGuid().ToString())
        New-Item -ItemType Directory -Path $repository | Out-Null
        Push-Location $repository
        git init --quiet
        git config user.name 'Checkout test'
        git config user.email 'checkout-test@example.invalid'
        git config commit.gpgsign false
        git config core.hooksPath (Join-Path $repository 'no-hooks')
        git commit --quiet --allow-empty -m 'Target branch'
        $target = git rev-parse HEAD
        $sourceBranch = 'dev/rolf/queued-pr'
        git checkout --quiet -b $sourceBranch
        git commit --quiet --allow-empty -m 'Queued PR head'
        $queuedHead = git rev-parse HEAD
        git checkout --quiet --detach $target
        git merge --quiet --no-ff $sourceBranch -m 'Queued GitHub merge'
        $queuedMerge = git rev-parse HEAD
        git checkout --quiet $sourceBranch
        git commit --quiet --allow-empty -m 'New live PR head'
        $liveHead = git rev-parse HEAD
        git update-ref refs/remotes/origin/pull/42/head $liveHead
        git checkout --quiet --detach $queuedMerge
    }

    AfterEach {
        Pop-Location
    }

    It 'checks out the queued PR parent instead of the target or live PR head' {
        $messages = @(& $scriptPath -SourceVersion $queuedMerge -SourceBranch "refs/heads/$sourceBranch" -IsPr true 6>&1 |
            ForEach-Object { $_.ToString() })

        git rev-parse HEAD | Should -Be $queuedHead
        git symbolic-ref --short HEAD | Should -Be $sourceBranch
        git rev-parse refs/remotes/origin/pull/42/head | Should -Be $liveHead
        $queuedHead | Should -Not -Be $target
        $queuedHead | Should -Not -Be $liveHead
        $messages | Should -Contain "##vso[task.setvariable variable=GIT_HASH;isOutput=true]$queuedHead"
    }

    It 'resolves the queued merge even if the current checkout differs' {
        git checkout --quiet --detach $liveHead

        & $scriptPath -SourceVersion $queuedMerge -SourceBranch $sourceBranch -IsPr true

        git rev-parse HEAD | Should -Be $queuedHead
        git symbolic-ref --short HEAD | Should -Be $sourceBranch
    }

    It 'keeps the queued commit and branch name on repeated execution' {
        & $scriptPath -SourceVersion $queuedMerge -SourceBranch $sourceBranch -IsPr true
        & $scriptPath -SourceVersion $queuedMerge -SourceBranch $sourceBranch -IsPr true

        git rev-parse HEAD | Should -Be $queuedHead
        git symbolic-ref --short HEAD | Should -Be $sourceBranch
    }

    It 'keeps the existing checkout for non-PR builds' {
        git checkout --quiet $sourceBranch

        & $scriptPath -SourceVersion $queuedMerge -IsPr false

        git rev-parse HEAD | Should -Be $liveHead
        git symbolic-ref --short HEAD | Should -Be $sourceBranch
    }

    It 'fails without changing the checkout when the source has no PR parent' {
        { & $scriptPath -SourceVersion $target -SourceBranch $sourceBranch -IsPr true } | Should -Throw

        git rev-parse HEAD | Should -Be $queuedMerge
    }

    It 'fails without changing the checkout when the source commit is missing' {
        { & $scriptPath -SourceVersion ('0' * 40) -SourceBranch $sourceBranch -IsPr true } | Should -Throw

        git rev-parse HEAD | Should -Be $queuedMerge
    }

    It 'rejects a missing PR source branch without changing the checkout' {
        { & $scriptPath -SourceVersion $queuedMerge -IsPr true } |
            Should -Throw '*PR source branch is required*'

        git rev-parse HEAD | Should -Be $queuedMerge
    }

    It 'rejects an invalid PR source branch without changing the checkout' {
        { & $scriptPath -SourceVersion $queuedMerge -SourceBranch 'invalid..branch' -IsPr true } |
            Should -Throw

        git rev-parse HEAD | Should -Be $queuedMerge
    }

    It 'passes the immutable build source version from the checkout template' {
        $yaml = Get-Content -Raw (Join-Path $PSScriptRoot '../templates/common/checkout.yml')

        $yaml | Should -Match 'undo_github_merge\.ps1 -SourceVersion "\$\(Build\.SourceVersion\)"'
        $yaml | Should -Match '-SourceBranch "\$Env:SYSTEM_PULLREQUEST_SOURCEBRANCH"'
    }
}
