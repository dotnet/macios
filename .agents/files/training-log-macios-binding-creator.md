# Training Log: macios-binding-creator

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
2. ⚠️ **Incomplete: Platform-specific code pattern** — Agent used `MACOS_DOTNET_SOURCES` in frameworks.sources for macOS-only code. User corrected to `#if MONOMAC`. Skill only documented `#if !TVOS`, not the full set of platform directives.
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
- **Type name prefix ≠ acronym** — The .NET "don't use all-caps acronyms" rule only applies within property/method names. ObjC type prefixes (AR*, AV*, CG*, CK*) are preserved exactly. The existing skill guidance was ambiguous enough for the agent to misapply it.
- **Preprocessor directives over source file lists** — The codebase convention is `#if MONOMAC` in shared files, not per-platform source file entries in frameworks.sources.
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
