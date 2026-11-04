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
        git checkout --quiet -b queued-pr
        git commit --quiet --allow-empty -m 'Queued PR head'
        $queuedHead = git rev-parse HEAD
        git checkout --quiet --detach $target
        git merge --quiet --no-ff queued-pr -m 'Queued GitHub merge'
        $queuedMerge = git rev-parse HEAD
        git checkout --quiet queued-pr
        git commit --quiet --allow-empty -m 'New live PR head'
        $liveHead = git rev-parse HEAD
        git update-ref refs/remotes/origin/pull/42/head $liveHead
        git checkout --quiet --detach $queuedMerge
    }

    AfterEach {
        Pop-Location
    }

    It 'checks out the queued PR parent instead of the target or live PR head' {
        $messages = @(& $scriptPath -SourceVersion $queuedMerge -IsPr true 6>&1 |
            ForEach-Object { $_.ToString() })

        git rev-parse HEAD | Should -Be $queuedHead
        git rev-parse refs/remotes/origin/pull/42/head | Should -Be $liveHead
        $queuedHead | Should -Not -Be $target
        $queuedHead | Should -Not -Be $liveHead
        $messages | Should -Contain "##vso[task.setvariable variable=GIT_HASH;isOutput=true]$queuedHead"
    }

    It 'resolves the queued merge even if the current checkout differs' {
        git checkout --quiet --detach $liveHead

        & $scriptPath -SourceVersion $queuedMerge -IsPr true

        git rev-parse HEAD | Should -Be $queuedHead
    }

    It 'keeps the existing checkout for non-PR builds' {
        & $scriptPath -SourceVersion $queuedMerge -IsPr false

        git rev-parse HEAD | Should -Be $queuedMerge
    }

    It 'fails without changing the checkout when the source has no PR parent' {
        { & $scriptPath -SourceVersion $target -IsPr true } | Should -Throw

        git rev-parse HEAD | Should -Be $queuedMerge
    }

    It 'fails without changing the checkout when the source commit is missing' {
        { & $scriptPath -SourceVersion ('0' * 40) -IsPr true } | Should -Throw

        git rev-parse HEAD | Should -Be $queuedMerge
    }

    It 'passes the immutable build source version from the checkout template' {
        $yaml = Get-Content -Raw (Join-Path $PSScriptRoot '../templates/common/checkout.yml')

        $yaml | Should -Match 'undo_github_merge\.ps1 -SourceVersion "\$\(Build\.SourceVersion\)"'
        $yaml | Should -Not -Match 'undo_github_merge\.ps1 -SourceBranch'
    }
}
