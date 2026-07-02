# Training Log: macios-binding-creator

## Session: 2026-07-01 (3) — Xcode 27 test-workflow accuracy (xtro/cecil/introspection) from 3 real binding sessions

**Trainer:** SkillTrainer | **Skill:** macios-binding-creator | **Trigger:** Three user requests to enhance the skill from three real copilot sessions, each an Xcode 27 binding task:
- `3611320c-a6c6-4982-9342-8f8cc4604a37` — AudioToolbox (`AUHeadTrackingBinauralRenderer`)
- `dcd60f0e-c28c-4b34-9623-ba3e29472001` — AuthenticationServices
- `dc7653a5-b1f0-4a7f-ad46-539a138f423e` — AVFoundation, which went all the way to **PR #25828** with maintainer review feedback **and CI failures** (8 checkpoints) — the richest evidence source.

**Method:** Every design AND implementation change was rubber-ducked READ-ONLY, in parallel, with three models (claude-sonnet-5 max, claude-opus-4.8 max, gpt-5.5 xhigh), then triple-checked against the live repo (Makefiles, test sources, `EnumCheck.cs`, `ApiCtorInitTest.cs`, `ApiSelectorTest.cs`, `Documentation.cs`). All claims cite verified file:line. **Not committed** (per standing user preference).

### Assessment

Mining the three on-disk session traces (checkpoints + `events.jsonl`) surfaced seven distinct, verifiable places where the skill's test-workflow guidance was factually wrong, silently no-op, or incomplete. Two sessions **independently** hit the same xtro `run-*` bug, confirming it was a real recurring blocker.

**Issues found (ranked):**
1. ❌ **Wrong: xtro `run-ios`/`run-tvos`/`run-macos`/`run-maccatalyst` targets do not exist** (SKILL.md Step 6a + test-workflow.md). Hit by sessions 1 **and** 3. Correct workflow = `gen-all` then `dotnet-classify` (which classifies all platforms then runs sanity). Verified: no such targets in `tests/xtro-sharpie/Makefile`.
2. ❌ **Wrong: generated-bindings grep path `api-annotations-dotnet/generated/`** (Step 4). It never existed. Correct = `tests/xtro-sharpie/api/<Platform>/ApiDefinition.cs` (gitignored, produced by `gen-all`). Verified `Makefile:167`, `.gitignore:1`.
3. ❌ **Wrong/no-op: `make -C src build`** (Step 5). No `build` target in `src/Makefile`; the word matches the `src/build/` output dir → `make: Nothing to be done for 'build'.` (exit 0, silent). Correct = `make all && make install` from the repo root (tests read installed NuGet packs). Reproduced via `make -n`.
4. ⚠️ **Incomplete: `DesignatedInitializer` re-expose failure** (Step 7). A bound subclass of a class with a designated initializer (e.g. `AUAudioUnit`) must re-expose it or `ApiCtorInitTest` fails `<Type> should re-expose <Base>::.ctor(...)`. Two patterns documented: plain public `[DesignatedInitializer] Constructor` (passes with no test edit), and the failable→factory variant (`AVSpeechSynthesisProviderAudioUnit`), which additionally needs a `Match ()` case in `ApiCtorInitTest.cs`.
5. ⚠️ **Missing: cecil `VerifyEveryVisibleMemberIsDocumented` baseline** (session 2). New public members fail unless documented; if the whole framework is already in `Documentation.KnownFailures.txt`, regenerate via `WRITE_KNOWN_FAILURES=1 make -C tests/cecil-tests run-tests` (exits non-zero by design), then re-run to confirm exit 0.
6. ⚠️ **Missing: selector-level introspection failures** (session 3, CI). `ApiSelectorTest` (`respondsToSelector:`) is a **different** test from `ApiCtorInitTest`. On a beta SDK Apple often *declares* a selector but the beta OS doesn't *implement* it (session 3: `AVAssetWriter` ProVideoStorage on macOS/MacCatalyst 27). The skill only covered ctor-init crashes.
7. ⚠️ **Missing: xtro `!extra-enum-value!`** (session 3, CI). A new enum value present in managed but `API_UNAVAILABLE` in the native header for a platform.

### Cycles (one change each; ❌ factual errors first)

- **Cycle 1 — xtro targets + generated path (issues 1, 2):** SKILL.md Step 6a rewritten to `gen-all` + `dotnet-classify` with the `?fixed-todo?` cleanup loop; Step 4 + binding-patterns.md version section repointed to `tests/xtro-sharpie/api/<Platform>/ApiDefinition.cs`; test-workflow.md Xtro Commands rewritten. **Outcome:** ✅ kept (3-model impl review found no factual error; all targets/paths verified real).
- **Cycle 2 — `make -C src build` no-op (issue 3):** Step 5 changed to `make all && make install` with an ❌ NEVER-use note. **Outcome:** ✅ kept.
- **Cycle 3 — DesignatedInitializer subclass re-expose (issue 4):** Step 7 bullet + new binding-patterns.md § "Re-exposing Designated Initializers in Subclasses" (plain + factory variants, with the `Match ()` requirement). **Outcome:** ✅ kept.
- **Cycle 4 — cecil doc baseline (issue 5):** Step 6b ⚠️ note + test-workflow.md § "Undocumented-member failures". **Outcome:** ✅ kept.
- **Cycle 5 — selector-level introspection (issue 6):** test-workflow.md new § "Selector Not Found (Declared but Not Implemented)" + Step 7 bullet. macOS → `MacApiSelectorTest.cs` `Skip (Type, string selectorName)` (precedent `accessibilityNotifiesWhenDestroyed`); iOS/tvOS/MacCatalyst → `iOSApiSelectorTest.cs` (existing selector skips live in `CheckResponse`/`CheckStaticResponse`; extend those or add a `Skip(Type,string)` override, `#if`-guarded). Skip only failing platforms — unconditional on real hardware, `TestRuntime.IsSimulator`-gated only for simulator-only gaps. **Design-review correction:** initial draft implied the cited iOS precedents lived in `Skip(Type,string)`; all three models showed they live in `CheckResponse` — corrected before applying.
- **Cycle 6 — xtro `!extra-enum-value!` (issue 7):** test-workflow.md new § + Step 6a note. **SCOPE-BASED** rule: type-level `[No<Platform>]` **only** when the whole native enum is unavailable on the platform; otherwise per-**value** `[No<Platform>]`. Precedents: `AVCaptureSessionInterruptionReason` (type-level `[NoMac]`) vs `AVAudioSessionCategoryOptions` (per-value). **Design-review correction (blocking):** my first draft said "always mark the TYPE `[NoMac]`; per-value is neither needed nor sufficient." All three models independently pulled the real merged PR and proved the correct fix here was **per-value** `[NoMac]` (the enum type stays Mac-available); type-level would strip valid members like `None` and can cause `!missing-enum!`. Reworded to the scope-based rule before applying. **Impl-review correction:** the initial type-level precedent (`AVAudioSessionRecordPermission`) was `#if XAMCORE_5_0`-gated (inactive, and contradicts the skill's own "never use XAMCORE_5_0 for new code" rule) — all three models flagged it; swapped to the always-active `AVCaptureSessionInterruptionReason`, and added the third `!extra-enum-value!` variant ("not found in native headers" → `.ignore`/remove, not `[No<Platform>]`).

**Polish from implementation review:** dropped an unverified exit-code `(2)` (kept "non-zero" for consistency with SKILL.md); corrected "appended" → "the whole sorted baseline is rewritten" for the cecil known-failures regen; fixed a misattribution (the `AVSpeechSynthesisProviderAudioUnit` "no `.ignore` needed" evidence belongs to the factory variant, not the plain-Constructor bullet); marked the illustrative `AUHeadTrackingBinauralRenderer` example as "not a real repo type."

### Patterns Learned
- **Xtro has no `run-*` targets; introspection does.** `tests/xtro-sharpie` only exposes `gen-all` / `dotnet-classify` / `unclassified2todo` / `run-tests`. The introspection suite *does* have `run-ios`/`run-tvos` via `shared-dotnet-test.mk`'s `run-%` pattern. Guidance must scope "no run-* targets" to xtro only, never conflate the two.
- **Two introspection failure modes, two test classes.** `ApiCtorInitTest` = the type crashes on `init`/dispose/ToString (fix the binding, or add a ctor-init exclusion). `ApiSelectorTest` = `respondsToSelector:` returns false (on a beta OS, usually a declared-but-unimplemented selector → add a *selector-test* skip, do **not** change availability, or xtro will report the API missing).
- **Fix `!extra-enum-value!` at the right granularity.** `EnumCheck` operates per-field, so match native availability at the same scope: whole-enum-unavailable → type-level `[No<Platform>]`; some-values-unavailable → per-value. Over-broad type-level attributes remove valid API and can flip to `!missing-enum!`.
- **Beta-runtime gaps ≠ binding bugs.** When Apple's header declares an API but the beta runtime lacks it, the binding is correct; the remedy is a narrow, platform-scoped test skip (unconditional on real hardware, `IsSimulator`-gated only where a simulator lacks something the device has).
- **Rubber-ducking caught a blocking design error.** The `!extra-enum-value!` "always type-level" framing would have taught agents an actively harmful fix. Three independent models pulling real repo state overturned my summary-derived assumption — evidence that the triple-model design pass is load-bearing, not ceremonial.

### Open Items (deferred 💡 — noted, not changed this session)
- `build-ios`/`build-tvos` casing in Step 6c works only because macOS is case-insensitive; canonical targets are `build-iOS`/`build-tvOS`. Low risk locally, could bite on case-sensitive filesystems.
- Hardcoded simulator runtime ids (`iOS-26-4`, `tvOS-26-4`) in Step 6c mlaunch examples will drift each Xcode bump; the surrounding `xcrun simctl list runtimes` note mitigates it.
- Struct marshaling nuance (session 3): a `[StructLayout(Sequential)]` struct returned **by value** must use explicit private fields + manual accessors, not get-only auto-properties (compiler-controlled backing-field layout). Reviewer-requested; not yet documented.
- Strong/weak overload idiom for NSString-typedef params + `[StrongDictionary]` (session 3 reviewer request) — partially covered by existing StrongDictionary examples; could be made explicit.
- Shipped-availability "don't narrow an already-shipped intro version" rule + how to recognize a false-positive from the automated `macios-reviewer` bot (session 3: bot wrongly asked to raise `AVAudioSession.Activate` MacCatalyst 15→27).
- Replace `"To be added."` placeholder docs with meaningful XML docs for **new** members only (session 3 reviewer feedback) — the skill already forbids the placeholder but doesn't cite this as a common review bounce.

### Reconciliation into `dev/alex/moarskillz` (post-hoc)

This session's work was later reconciled onto a rebased branch (`dev/alex/moarskillz`) that had already committed two other 2026-07-01 sessions (`(2)` xkit consolidation + runtime-only protocol conformance) via a 3-way merge against the clean common base — all three sessions preserved, no content lost either way. Rubber-ducked read-only with three models (Sonnet 5 max, Opus 4.8 max, GPT-5.5 xhigh); all confirmed the merge lossless. Sonnet's deeper fact-check caught **two now-stale claims** in this session's own content — stale because the branch's `main`-merge had meanwhile pulled in the very source PRs this session studied:
- `AUHeadTrackingBinauralRenderer` (AudioToolbox PR #25811) is now a **real** bound type using the **factory** variant, so the "simplest plain-`Constructor`" example that had borrowed its name was renamed to a genuinely hypothetical `MySpatialAudioUnit`; the real type is now cited as a factory-variant precedent (`ApiCtorInitTest.cs:564`, "exposed using a factory method").
- `iOSApiSelectorTest.cs` now **already has** a `Skip (Type, string)` override (AVFoundation PR #25828, `AVAssetWriter` Pro Video Storage), so the selector-skip guidance was corrected from "add an override" (which would be CS0111) to "extend the existing one."
- **Meta-lesson:** freshly-written file:line claims can go stale when the branch later merges the source PR they describe — re-verify "not a real type"/"no such override" negatives against the current tree at reconciliation time.


---

## Session: 2026-07-01 (2) — Shared AppKit/UIKit type consolidation into xkit.cs

**Trainer:** SkillTrainer | **Skill:** macios-binding-creator | **Trigger:** User request to enhance skill using copilot session c713d720-bbb8-4c4a-a2e1-cba321a9f92c (Xcode 27 UIKit bindings, PR #25777, branch dev/alex/xc27UIKit)

### Assessment

**Source:** Session c713d720 "Update UIKit Bindings" (Xcode 27 beta2). After binding ~130 UIKit `.todo` entries, the user flagged that `NSTextBlock`/`NSTextTable`/`NSTextTableBlock` (+5 enums) "availability seems old" and "may belong to xkit.cs instead." The agent (with heavy Opus 4.8 + GPT-5.5 rubber-ducking across many make-world/introspection/cecil iterations — see the ~40 log files in the session's `files/`) worked out a non-obvious architectural convention: these UIFoundation TextKit types, newly *exposed* to UIKit in iOS 27 but back-dated to iOS 6, must be **consolidated into the shared `src/xkit.cs`** (compiled into both AppKit and UIKit), following the `NSTextList` precedent (moved appkit.cs→xkit.cs in 2022). The skill had **no** coverage of xkit.cs or shared AppKit/UIKit binding at all (only `#if __MACOS__` for imports was mentioned).

**Issues found (ranked):**
1. ❌ **Missing: shared AppKit/UIKit type consolidation (`xkit.cs`).** No guidance on when/how to bind a type once in `xkit.cs` vs duplicating in appkit.cs/uikit.cs, the `#if MONOMAC` namespace-switch mechanics, macOS-divergence guarding, split-vs-shared enums, back-dated availability, or the byte-identical `.g.cs` ABI check.
2. ⚠️ **Missing: `initWithCoder:` / CS0108 gotcha.** Binding an explicit `[Export ("initWithCoder:")]` on an `NSCoding`/`NSSecureCoding` type collides with the auto-generated constructor → CS0108 under `-warnaserror` (cost the session a build cycle).

### Cycle 1: Add "Shared AppKit/UIKit Types (xkit.cs)" guidance

**Hypothesis:** Documenting the consolidation trigger, mechanics, `#if MONOMAC` divergences, enum share-vs-split rule, back-dated availability, and the `.g.cs` safety net will let agents make the correct architectural call (and avoid the CS0108 trap) without extended user correction.

**Edits:**
1. `references/binding-patterns.md` — New "### Shared AppKit/UIKit Types (`src/xkit.cs`)" section (compiled-twice mechanics; when-to-consolidate + `NSTextList`/`8cecb962a4` precedent; 3-step move; `#if MONOMAC` divergence list; share-vs-split enums; back-dated availability; byte-identical `.g.cs` ABI callout). Plus a CS0108 `initWithCoder:` callout after the "protocol-inherited properties" note.
2. `SKILL.md` — One-line pointer after the preprocessor-symbols list.

**Validation:** Rubber-ducked with Opus 4.8 (max) + GPT-5.5 (xhigh) in parallel, read-only against the real repo; both returned **ship-with-changes** and verified every structural claim with file:line evidence. Fixes applied from their reviews:
- **GPT-5.5:** `NSTextList` is `[MacCatalyst (13, 0)]` (xkit.cs:4709), not `(13, 1)` — corrected. "Keep enums split" was too absolute — `NSTextListOptions`/`NSTextListMarkerFormats` are shared enums *in* xkit.cs (4666/4572); reworded to "share identical, split divergent." Sharpened the MONOMAC-vs-`__MACOS__` note.
- **Opus 4.8:** (A) SKILL.md used `NSTextBlock` as an "already shared" example, but it's still in appkit.cs (only a `System.Object` dummy in xkit.cs) — swapped to `NSParagraphStyle` (genuinely in xkit.cs:1833). (B) `[NoMacCatalyst]` inside `#if MONOMAC` is dead (Catalyst compiles the `#else` branch; independently verified 110 `[NoMacCatalyst]` in xkit.cs, **0** inside `#if MONOMAC`) — removed. (C) Opus argued CS0108 should be CS0111; **rejected on empirical grounds** — the session checkpoint records "CS0108 …hides NSCoding.Constructor" verbatim, events.jsonl has 75× CS0108 / 0× CS0111, and macios builds with `-warnaserror+` (src/Makefile:240); kept CS0108, sharpened wording to "hides inherited member."

**Outcome:** ✅ Changes kept (additive-only, +33 lines across 2 files).

### Patterns Learned
- **Shared UIFoundation/TextKit types live in `xkit.cs`.** When a macOS-only `appkit.cs` type becomes exposed to UIKit (usually back-dated), consolidate into `xkit.cs` (one binding, two assemblies via the `#if MONOMAC` namespace switch), don't duplicate into `uikit.cs`. Precedent: `NSTextList` (`8cecb962a4`).
- **Enums: share identical, split divergent.** Identical enums live in `xkit.cs`; split into AppKit/UIKit enum files only when they diverge (ABI-frozen `ulong` vs new `long`). xtro flags `!wrong-enum-size!` on size only, never signedness.
- **`xkit.cs`'s namespace switch uses legacy `MONOMAC`**, but `__MACOS__` and `MONOMAC` are both defined for the macOS build — **prefer `#if __MACOS__` for new members** (per rolfbjarne's PR #25133 preference to retire `MONOMAC`).
- **`#if __MACOS__` (like `#if MONOMAC`) already excludes Mac Catalyst** — no `[NoMacCatalyst]` needed on members inside it.
- **Byte-identical macOS `.g.cs`** is the ABI safety net when moving a type (no apidiff target for generated sources).
- **Don't bind `initWithCoder:` on NSCoding/NSSecureCoding types** — the conformance auto-generates it; an explicit one is CS0108 under `-warnaserror`.

### Open Items
- Availability *style* for back-dated shared types (explicit `[iOS]`/`[TV]`/`[Mac]` vs implicit + `[MacCatalyst]`) — the two reviewers split in the source session; documented the `NSTextList` implicit shape as the safe default, but this remains a judgment call.

---

## Session: 2026-07-01 — Runtime-only protocol conformance (introspection Skip vs binding)

**Trainer:** SkillTrainer | **Skill:** macios-binding-creator | **Trigger:** User request to enhance skill using copilot session 5db26e99-1d61-4efd-9462-d7a93401ade7 (Xcode 27 AppKit bindings, PR #25777)

### Assessment

**Source:** Session 5db26e99 "Update AppKit Bindings" (Xcode 27 beta, branch `dev/alex/xc27-appkit`, PR #25777). Two checkpoints — "Fixing iOS CarPlay NSCopying introspection" and "Fixing NSViewCornerRadii macOS 27 introspection" — hit the SAME class of problem, and a placement sub-bug broke CI (build 14508058):
- **CarPlay:** the Xcode 27 runtime conforms 5 CarPlay types (`CPButton` + 3 subclasses + `CPTravelEstimates`) to `NSCopying`, but the headers declare only `: NSObject` — introspection failed; the agent had to work out that the fix is a test-only introspection Skip, not a binding change.
- **NSViewCornerRadii:** the macOS 27 runtime adds `NSCoding`+`NSSecureCoding` (header declares only `<NSCopying>`); the agent's first Skip landed in the adjacent `NSCopying` block (compiled, passed locally, failed in CI on the SecureCoding test).

The agent reached the correct answers only via extensive rubber-ducking with two models, and still shipped the placement bug once. The skill had **zero** coverage of the protocol-conformance introspection test (`ApiProtocolTest.cs`) — its "Handling Introspection Failures" section only covered ctor-init crash exclusions.

**Issues found (ranked):**
1. ❌ **Missing: runtime-only protocol conformance → introspection Skip.** No guidance that a beta runtime can conform a type to a protocol the header doesn't declare, that the fix is a Skip in `ApiProtocolTest.cs` (not a binding change), or how to tell this apart from a real "binding forgot the interface" bug.
2. ❌ **Missing: the placement trap.** The adjacent `NSCopying/NSMutableCopying/NSCoding/NSSecureCoding` case blocks let a misplaced `case` compile and fail only in CI — this literally broke CI in the session.
3. ⚠️ **Missing: NSSecureCoding ⊃ NSCoding coupling.** A runtime NSSecureCoding conformance fails both the Coding and SecureCoding tests, so both blocks need an entry (a one-block fix regresses in CI).

### Cycle 1: Add "Runtime-Only Protocol Conformance" guidance

**Hypothesis:** Documenting the symptom → root cause (runtime vs header) → decision rule (Skip, not bind, not defer), plus the placement trap, the both-blocks coupling, and a hard xtro-based discriminator, will let agents fix these failures correctly on the first try without inventing a binding change.

**Edits:**
1. `references/test-workflow.md` — New "## Runtime-Only Protocol Conformance" section: symptom/root-cause/fix; scope→file table (base `ApiProtocolTest.cs` vs `Mac`/`iOS`ApiProtocolTest.cs, both chaining to base); a "first rule out a real binding bug" xtro discriminator; the NSSecureCoding⊃NSCoding both-blocks rule; the CI-breaking placement trap; exact-`type.Name`/subclass rule; and a defer-vs-Skip disambiguation.
2. `SKILL.md` Step 6c — One-line callout pointing to the new section, with the discriminator baked in.
3. `references/binding-patterns.md` — Contrast note after the `!missing-protocol-conformance!` guidance: only add conformance the header/category declares; runtime-only → Skip.

**Validation:** Rubber-ducked design + implementation with Opus 4.8 (max) + GPT-5.5 (xhigh) in parallel, both read-only against the real repo. Both returned **ship-with-changes** and verified every structural claim with file:line evidence (`ApiProtocolTest.cs:82/856/879/897`, adjacent case blocks, `Mac`/`iOS` overrides ending in `base.Skip` at `:421`/`:1052`, the `shared.csproj:28/45` Catalyst→iOS-file routing, the `// Conformance not in headers` convention). Fixes applied from their review:
- Removed an **invented** `!extra-protocol-conformance!` xtro token (xtro only emits `!missing-protocol-conformance!`; `ObjCInterfaceCheck.cs:127` = `// TODO : check for extraneous protocols`).
- Added the "first rule out a real binding bug" xtro discriminator (the failure message is identical for a genuine missing-interface bug) — message text verified verbatim at `ObjCInterfaceCheck.cs:125`, including its `(defined in '…' category)` variant.
- Added the NSSecureCoding⊃NSCoding both-blocks rule (empirically 104 types live in both blocks; VSSubscription/PKSuicaPassProperties in both).
- Softened the placement-trap wording, added `CPTravelEstimates`, fixed a `!missing-…!` shorthand, narrowed the defer aside, and recommended the base file as the safe default.

**Outcome:** ✅ Changes kept (additive-only, +51 lines across 3 files).

### Patterns Learned
- **Runtime conformance ≠ header conformance.** New Xcode betas often add a protocol conformance to the RUNTIME before the header declares it. Bindings/xtro are header-driven, so the fix is a test-only introspection Skip in `ApiProtocolTest.cs`, NOT adding the interface (a public-API commitment that breaks if Apple drops it).
- **The failure message is ambiguous — use xtro as the discriminator.** `X conforms to <P> but does not implement I<P>` is identical whether the runtime added an undeclared conformance (→ Skip) or the binding forgot a header-declared one (→ fix binding). `!missing-protocol-conformance!` from xtro ⇒ fix the binding; xtro silence ⇒ Skip.
- **`NSSecureCoding : NSCoding` → skip in both blocks.** A runtime NSSecureCoding conformance fails both tests.
- **Adjacent switch blocks are a placement trap.** A misplaced `case` compiles and fails only in CI — always verify the enclosing `case "<Protocol>":`.

### Open Items
- Secondary session learnings not yet incorporated (future-cycle candidates): xtro `dotnet-classify` (sanity) doesn't catch unclassified `!extra-!`/`!unknown-!` — only `make -C tests/xtro-sharpie all` (report) does, and `git clean -xfd tests/xtro-sharpie` is needed first; the cecil `WRITE_KNOWN_FAILURES=1` regeneration workflow; making a protocol member `[Abstract]` removes its generated `_Extensions` methods (stale doc known-failures).

---

## Session: 2026-04-10 (1) — Mixed API surface frameworks, .todo cleanup, monotouch-test commands

**Trainer:** SkillTrainer | **Skill:** macios-binding-creator | **Trigger:** Deeper analysis of copilot session 62c564f6-99e3-47f5-b523-d206c665b71d (ARKit bindings, turns 12-14 + test workflow)

### Assessment

**Source:** Session 62c564f6 turns 12-14 — User guided agent through architectural cleanup of ARKit's `frameworks.sources` organization. Agent created `ARKIT_C_API_SOURCES` + `MACOS_DOTNET_SOURCES +=` hack; user showed cleaner approach of guarding entire bgen file with `#if !__MACOS__` and adding framework to `MACOS_FRAMEWORKS`. Also turn 11: user had to remind agent to delete empty `.todo` file.

**Issues found (ranked):**
1. ❌ **Missing: Mixed API surface framework pattern** — When a framework has ObjC APIs on mobile and C APIs on macOS, the agent created split source lists instead of using `#if` guards on the bgen file. This is a key architectural pattern not documented.
2. ❌ **Wrong: Monotouch-test commands** — Skill said `make -C tests/monotouch-test run` which doesn't work. Correct commands are per-platform from `tests/monotouch-test/dotnet/{Platform}/`. User had to ask about macOS target (turn 3-4) and explicitly request per-platform runs (turns 6, 9).
3. ⚠️ **Weak: Empty .todo file cleanup** — Agent forgot despite existing ⚠️ guidance. Needs upgrade to ❌ level.

### Cycle 1: Add mixed framework pattern + strengthen .todo cleanup

**Hypothesis:** Documenting the "guard entire bgen file with `#if !__MACOS__`" pattern will prevent agents from creating convoluted split source lists. Upgrading .todo cleanup to ❌ level will make it harder to miss.

**Edits:**
1. `references/binding-patterns.md` — New "Frameworks with Mixed API Surfaces (ObjC + C)" subsection with 4-step pattern, code examples (frameworks.sources, bgen file guard, manual C API guard), and anti-pattern against split source lists.
2. `SKILL.md` — Upgraded empty `.todo` file deletion from ⚠️ to ❌ with stronger language.
3. `SKILL.md` Step 6d — Replaced wrong `make -C tests/monotouch-test run` with per-platform commands (iOS, tvOS, macOS, MacCatalyst) including `run-bare` for desktop platforms and casing warning.
4. `references/test-workflow.md` — Rewrote Monotouch Tests section with per-platform command table, `run-bare` guidance for desktop, note that mlaunch is NOT needed (unlike introspection), and casing requirements.

**Outcome:** ✅ Changes kept.

### Patterns Learned
- **Guard the bgen file, not the source list** — When ObjC API definitions won't compile on a platform (UIKit dependencies on macOS), wrap the entire bgen file in `#if !__MACOS__` rather than splitting source lists. This is simpler and more maintainable.
- **Severity matters for agent compliance** — The ⚠️ level for .todo cleanup wasn't enough; the agent skipped it. ❌ level rules get followed more consistently.
- **Per-platform monotouch-test commands** — `make -C tests/monotouch-test run` doesn't exist. Must use `tests/monotouch-test/dotnet/{Platform}/` with exact casing. Desktop uses `run-bare`, mobile uses `run` (no mlaunch needed unlike introspection).

### Open Items
- None.

---

## Session: 2026-04-09 (3) — Version determination and enum member availability

**Trainer:** SkillTrainer | **Skill:** macios-binding-creator | **Trigger:** User request to enhance skill using copilot session d8792953-287f-485e-aed6-d4a6d46043c8 (SystemConfiguration bindings)

### Assessment

**Source:** Session d8792953 — "Update SystemConfiguration Bindings" (1 turn, 2026-04-09). Cross-referenced with 16 other binding-creator sessions, particularly:
- Session 91f91750 (MediaSetup, 3 turns) — User corrected: agent used `[MacCatalyst (26, 5)]` when framework was actually introduced at `(16, 0)`
- Session 28911011 (Photos, 2 turns) — User corrected: agent forgot `[iOS (26, 5)]` on new enum member `Process = 3`

Multi-model pre-assessment (Sonnet 4, GPT-5.1, Haiku 4.5) scored skill at **Accuracy 2/5, Completeness 2/5, Clarity 2/5** on two behavioral scenarios. All 3 models reproduced both failure modes when following the skill literally.

**Issues found (ranked):**
1. ❌ **Wrong: SDK version conflated with introduction version** — Skill said "use SdkVersions.cs version for all availability attributes" which is only correct for brand-new APIs. When introducing a framework on a new platform, the actual historical introduction version is needed. All 3 models used `[MacCatalyst (26, 5)]` instead of `[MacCatalyst (16, 0)]`.
2. ❌ **Missing: Per-member enum availability** — Skill mentioned "every new API" needs attributes but didn't specifically call out individual enum members added to existing enums. 1 of 3 models omitted the per-member attribute entirely; the other 2 added it but noted the skill was ambiguous.
3. ⚠️ **Missing: Version research methodology** — No guidance on how to determine when an API was actually introduced. Generated reference bindings (from `make -C tests/xtro-sharpie gen-all`) contain `[Introduced]` attributes from Apple headers — this was not mentioned as a source.

### Cycle 1: Fix version determination + enum member availability

**Hypothesis:** Rewriting the version determination section to use generated reference bindings as primary source of truth (instead of SdkVersions.cs) will prevent wrong availability versions. Adding explicit per-member enum guidance will prevent omitted attributes.

**Edits:**
1. `SKILL.md` — Rewrote "Determine the Correct Availability Version" subsection in Step 4. Changed primary source from SdkVersions.cs to generated reference bindings. Added fallback hierarchy (Apple headers → SdkVersions.cs for brand-new APIs only). Added ❌ NEVER rule against assuming SDK version = introduction version, with MediaSetup as example.
2. `SKILL.md` — Enhanced the availability bullet list to explicitly include "Individual enum members added to an existing enum" with guidance to check generated reference bindings for per-member versions.
3. `references/binding-patterns.md` — New "Adding New Members to Existing Enums" subsection under Enum Bindings with code example showing `[iOS (26, 5)]` on a new member within an `[iOS (18, 0)]` enum.
4. `references/binding-patterns.md` — Rewrote "Determining the Correct Version" subsection with prioritized source list (generated reference bindings → Apple headers → SdkVersions.cs) and new "Common Version Mistakes" table covering framework-on-new-platform, new-enum-member, and brand-new-API scenarios.

**Validation (same 3 models, same 2 scenarios):**

| Metric | Before | After |
|--------|--------|-------|
| Accuracy (avg) | 2/5 | **5/5** |
| Completeness (avg) | 2/5 | **5/5** |
| Clarity (avg) | 2/5 | **5/5** |

All 3 models now correctly: use `[MacCatalyst (16, 0)]` from generated reference bindings (not `26, 5`), add per-member `[iOS (26, 5)]` on new enum values, and cite the generated reference bindings as the source of truth.

**Outcome:** ✅ Committed as `e96a8558b8`. No regressions detected.

### Patterns Learned
- **Generated reference bindings are the best version source** — `make -C tests/xtro-sharpie gen-all` produces `.cs` files with `[Introduced]` attributes extracted from Apple SDK headers. These contain the correct per-platform, per-member introduction versions. The skill previously didn't mention this as a version source at all.
- **SDK version ≠ introduction version** — SdkVersions.cs gives the current Xcode SDK version (e.g., 26.5), which is correct for brand-new APIs but wrong for APIs that were introduced in earlier releases or on other platforms. The distinction must be explicit in the skill.
- **Enum members need individual availability** — When a new value is added to an existing enum, it needs its own `[iOS (X, Y)]` attribute with the member's introduction version, separate from the enum-level attribute. This was a common oversight because the skill only said "every new API" without calling out this specific case.

### Open Items
- None — both ranked issues addressed. Stop signal met (≥4/5 across 3 models, 2 families).

---

## Session: 2026-04-09 (2) — Naming prefixes, platform directives, callback patterns

**Trainer:** SkillTrainer | **Skill:** macios-binding-creator | **Trigger:** User request to enhance skill using copilot session 62c564f6-99e3-47f5-b523-d206c665b71d (ARKit bindings)

### Assessment

**Source:** Session 62c564f6 — "Update ARKit C# Bindings" (9 turns, 2026-04-09)

The session revealed three gaps: the agent named new ARKit types `Ar*` instead of `AR*` (turn 8 correction), used `MACOS_DOTNET_SOURCES` instead of `#if MONOMAC` (turn 7 correction), and implemented a callback handler with null safety issues on nullable `DispatchQueue?` parameters (turn 1 code review).

**Issues found (ranked):**
1. ❌ **Wrong: ObjC type prefix casing** — Agent applied .NET acronym rules to type name prefixes, producing `ArSession` instead of `ARSession`. Current guidance ("Acronyms shouldn't be all uppercase") was ambiguous — it applies to property/method names, not type prefixes.
2. ⚠️ **Incomplete: Platform-specific code pattern** — Agent used `MACOS_DOTNET_SOURCES` in frameworks.sources for macOS-only code. User corrected to `#if __MACOS__`. Skill only documented `#if !TVOS`, not the full set of platform directives.
3. ⚠️ **Missing: C callback handler binding pattern** — Agent implemented GCHandle-based callback handler with null safety issues on nullable parameters and incorrect memory management ordering. No guidance existed for this pattern.

### Cycle 1: Fix naming + platform directives + callback pattern

**Hypothesis:** Explicitly distinguishing type name prefixes from .NET acronym rules will prevent the `Ar*` vs `AR*` mistake. Documenting preprocessor symbols and anti-pattern for source file lists will prevent `MACOS_DOTNET_SOURCES` misuse. Adding callback handler patterns will prevent null safety and memory management bugs.

**Edits:**
1. `references/binding-patterns.md` — Rewrote naming bullet in Common Pitfalls to explicitly state type names preserve ObjC prefix exactly, with examples (ARSession, AVPlayer, CGColor).
2. `references/binding-patterns.md` — New "Platform-Specific Code Within Shared Files" subsection with preprocessor symbol table and anti-pattern.
3. `references/binding-patterns.md` — New "C Callback Handler Binding" subsection with BlockLiteral trampoline and GCHandle patterns, null safety rules, memory management ordering.
4. `SKILL.md` — Added ❌ NEVER rule about ObjC class prefix casing in Step 4.
5. `SKILL.md` — Enhanced Step 4b with platform directive anti-pattern and preprocessor symbol list.

**Outcome:** ✅ Changes kept. All five edits address direct user corrections or code review findings from the session.

### Patterns Learned
- **Type name prefix vs. inner acronym** — The ObjC class *prefix* (AR*, AV*, CG*, CK*) is preserved exactly, but an acronym *inside* a type name follows .NET rules (per rolfbjarne's PR #25133 review): `NSURLSession` → `NSUrlSession`, `NSURLSessionHandler` → `NSUrlSessionHandler` — the `NS` prefix stays, `URL` becomes `Url`. The .NET acronym rule applies to property/method names and to inner acronyms, not to the leading prefix.
- **Preprocessor directives over source file lists** — The codebase convention is `#if __MACOS__` in shared files, not per-platform source file entries in frameworks.sources.
- **Callback handler null safety** — When a setter method has both a nullable handler and a nullable queue, ALL code paths must null-check both parameters independently. The "handler is null → pass null to native" shortcut still needs to handle the queue parameter safely.

### Open Items
- None — all three ranked issues addressed.

---

## Session: 2026-04-09 (1) — NativeObject marshal type guidance

**Trainer:** SkillTrainer | **Skill:** macios-binding-creator | **Trigger:** User request to enhance skill using copilot session 1a7e421e-e4ca-44bc-a5d1-4d745db2ed06 (PrintCore bindings)

### Assessment

**Source:** Session 1a7e421e — "Update PrintCore Bindings" (12 turns, 2026-04-08 to 2026-04-09)

The session revealed a critical knowledge gap: when binding protocol methods that return opaque C types with managed NativeObject wrappers (PMPrintSession, PMPrintSettings, PMPageFormat, PMPrinter), the agent initially used IntPtr returns and required 8 turns of user guidance to discover the bgen marshal type registration pattern. The user had to:
1. Ask the agent to research concrete types (turn 1)
2. Request Runtime.GetINativeObject docs (turns 2-3)
3. Guide through CORE_SOURCES + IntPtr → concrete type (turn 6)
4. Correct the file-splitting approach to #if !COREBUILD (turn 7)
5. Point to CGColor as a precedent for marshal types (turn 8)
6. Approve the TypeCache + MarshalTypeList fix (turn 9)

**Issues found (ranked):**
1. ❌ **Missing: NativeObject marshal type registration** — No guidance on the multi-step process for making NativeObject types work as bgen return types (TypeCache.cs + MarshalTypeList.cs + CORE_SOURCES). Agent defaulted to IntPtr and couldn't self-correct.
2. ⚠️ **Incomplete: CORE_SOURCES in frameworks.sources** — Skill only mentioned `*_SOURCES` and `*_API_SOURCES`, not `*_CORE_SOURCES` for making types visible to bgen's core assembly.
3. ⚠️ **Incomplete: #if !COREBUILD for NativeObject shells** — Guidance only covered struct properties. Agent tried splitting into separate files instead of using the established #if !COREBUILD pattern for class shells.

### Cycle 1: Add NativeObject marshal type registration section

**Hypothesis:** Adding a comprehensive "NativeObject Return Types in Protocol Methods" section to binding-patterns.md will prevent agents from using IntPtr when concrete types exist, and will guide them through the TypeCache + MarshalTypeList registration process.

**Evidence:** Session 1a7e421e showed the agent needed 8 turns of guidance. The pattern is well-established (CGColor, CGImage, CMSampleBuffer are all registered this way) but undocumented in the skill.

**Edits:**
1. `references/binding-patterns.md` — New section "NativeObject Return Types in Protocol Methods" with recognition criteria, step-by-step process (CORE_SOURCES → #if !COREBUILD → TypeCache → MarshalTypeList → use concrete type), code examples, real example (PrintCore), and anti-pattern.
2. `references/binding-patterns.md` — Enhanced `frameworks.sources` subsection to explain `*_CORE_SOURCES` with code example and when to use it (BI1078 error).
3. `references/binding-patterns.md` — Added NativeObject class shell guidance to Common Pitfalls `#if !COREBUILD` bullet.
4. `SKILL.md` — Added cross-reference warning in Step 4 about protocol methods returning opaque types, pointing to binding-patterns.md.

**Outcome:** ✅ Changes kept. All four edits are minimal and targeted — no bloat, each adds information the agent couldn't discover on its own.

### Patterns Learned
- **bgen marshal type registration is a 4-file change** — TypeCache.cs (property + lookup), MarshalTypeList.cs (Add call), frameworks.sources (CORE_SOURCES), and the manual code file (#if !COREBUILD guards). All four steps must be documented together since missing any one causes different errors.
- **#if !COREBUILD is the universal pattern** — Not just for structs. NativeObject class shells use it identically. The skill should present it as a general pattern, not struct-specific.
- **Precedent-based learning works** — The user pointing to CGColor as a precedent was the key breakthrough. The skill should name precedent types to help agents self-discover.

### Open Items
- None — all three ranked issues addressed in this cycle.
