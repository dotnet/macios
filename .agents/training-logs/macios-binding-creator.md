# macios-binding-creator Training Log

Training logs are operational artifacts (sibling to `skills/`, per the SkillTrainer
convention). They record *why* skill edits were made so future trainers can follow the
reasoning. The skill itself ships to users; this log does not.

## Session: 2026-07-09 — Enum-availability, named-delegate, and host-gating gaps

**Trainer:** SkillTrainer | **Skill:** macios-binding-creator | **Trigger:** Enhance the skill using four real Copilot sessions that ran it (Xcode 27 binding tasks: WebKit, VideoToolbox, Vision, UserNotifications).

### Assessment (eval = 4 real session traces)

Instead of synthetic evals, this session mined four real runs of the skill (session IDs
`d7492b51…` WebKit, `00439604…` VideoToolbox, `1c893136…` Vision, `2b2f6fe9…`
UserNotifications). Real behavioral data is stronger than a quiz: it shows exactly where the
skill's guidance led the agent astray or was missed. Each run had rubber-duck review by three
models (Opus 4.8, Sonnet 5, GPT-5.5) whose findings were also mined.

**Issues found (ranked):**
1. ❌ **Error-enum members must NOT carry availability attributes** — the skill's rule (SKILL.md "each new member needs its own `[iOS (X,Y)]`") is actively wrong for error enums and build-breaking. Hit in **2 of 4** sessions.
   - Vision: the agent added `[iOS/TV/Mac/MacCatalyst (27,0)]` to new `VNErrorCode` members → would fail cecil `EnumTest.NoAvailabilityOnError`. Only Sonnet-5 (1 of 3 reviewers) caught it; the agent removed them.
   - UserNotifications: new `UNErrorCode.AttachmentUnsupportedType` correctly got no attribute; a reviewer noted "your instinct beats the generic skill rule here" — i.e. the skill rule was contradicting correct behavior.
2. ⚠️ **Enum-member availability nuance** — add a per-member attribute only when the header annotates that member (its own `API_AVAILABLE`, or `API_UNAVAILABLE`/absence); a member with no per-member annotation inherits the enum's availability and gets nothing. The old rule over-generalized ("each new member needs its own `[iOS]`").
3. ⚠️ **Named-delegate rule was buried** — `binding-patterns.md:321` says never use `Action<T>` for callbacks, but WebKit's agent used `Action<T>` anyway and the **user had to correct it**. Reference-only critical rules get missed (skill-builder-knowledge "burying critical rules" anti-pattern).
4. 💡 **Introspection host-OS version gating** — VideoToolbox: host macOS 26.5.2 < 27.0, so macOS/MacCatalyst introspection gated the new 27.0 symbols away (skipped via `SkipDueToAttribute`/`IsAvailableOnHostPlatform`, not crashed); the agent validated the new `[Field]` symbols on the iOS 27 simulator instead. Not documented in Step 6c.
5. 💡 **Multi-platform enum-member version inheritance** — VideoToolbox smart-enum member `VTProjectionKind.AppleImmersiveVideo`: bgen inherits the parent enum's *highest* introduced version per platform, so specifying only `[iOS (27,0)]` would leave Mac/MacCatalyst reporting the parent's older 26.0.

### Cycle 1: Enum-member availability (issues #1, #2, #5)

**Hypothesis:** Replacing the over-broad "each new member needs its own `[iOS]`" rule with a
per-header rule, adding an explicit error-enum anti-pattern, and adding a multi-platform
inheritance note will stop the build-breaking error-enum mistake (seen twice) and the
partial-version mistake — because the skill was actively wrong for error enums and silent on
inheritance.

**Verified against repo before editing:**
- `tests/cecil-tests/EnumTest.cs:28-90` — `IsErrorEnum` = name `EndsWith("Error")` OR `EndsWith("ErrorCode")` OR has `[ErrorDomain]`; flags Introduced/Unavailable/iOS/Mac/TV/NoiOS/NoMac/NoTV/Supported/UnsupportedOSPlatform on fields; `ObsoletedOSPlatform` and plain `[Obsolete]` exempt; `Assert.That(found, Is.Empty)` with no known-failures allowlist.
- `src/bgen/AttributeFactory.cs:179-204` (`FindHighestIntroducedAttributes`) + `Generator.PrintAttributes.cs:32-67` — bgen back-fills each platform where a member lacks an introduced attribute with the parent's version; runs for **every** enum field (`src/bgen/Enums.cs`), not only `[Field]`/smart ones.
- Real bare examples: `src/vision.cs` (`VNErrorCode`), `src/usernotifications.cs` (`UNErrorCode`); real multi-platform example `src/videotoolbox.cs` (`VTProjectionKind.AppleImmersiveVideo`).

**Edits:** `SKILL.md` — rewrote the enum-member bullet + added an error-enum ❌ anti-pattern.
`references/binding-patterns.md` § "Adding New Members to Existing Enums" — refined the intro,
clarified the single-platform example, added the error-enum exception + a "Multi-platform enums
(numeric *or* smart)" inheritance note.

### Cycle 2: Elevate the named-delegate rule (issue #3)

**Hypothesis:** Promoting the `Action<T>`→named-delegate rule from a reference into a SKILL.md
Step-4 anti-pattern will stop the miss that required a user correction in WebKit.

**Edit:** `SKILL.md` Step 4 — added the named-delegate ❌ anti-pattern (links to the reference).

### Cycle 3: Introspection host-OS version gating (issue #4)

**Hypothesis:** A Step-6c note explaining host-OS gating + telling the agent to bump the
simulator `runtime=` to the new SDK will prevent false "validated" conclusions from
macOS/Catalyst runs that silently gate the new API away.

**Verified:** `tests/common/PlatformInfo.cs` (`Host.Version` = running OS) + `ApiBaseTest.cs`
(`IsAvailableOnHostPlatform`/`SkipDueToAttribute`).

**Edit:** `SKILL.md` Step 6c — added the host-OS-gating ⚠️ note (bump `--device runtime=` to
the new-SDK runtime; noted macOS-only APIs have no simulator fallback).

### Validation (multi-model, per user's "rubber-duck EVERYTHING" directive)

Draft edits were reviewed read-only by three models (Opus 4.8 max, Sonnet 5, GPT-5.5 xhigh)
**before** applying. Consensus corrections were all incorporated:
- Add `[Unavailable]` to the forbidden-attribute lists (GPT + Sonnet + Opus).
- Generalize "smart enum" → **multi-platform enum** (numeric *or* smart) for the inheritance
  note (Sonnet blocking, Opus) — the bug isn't `[Field]`-specific.
- Broaden "only `API_AVAILABLE`" → include unavailability annotations (`API_UNAVAILABLE`/absence
  → `[No*]`) (GPT blocking).
- Step 6c note must tell the agent to bump the hardcoded `iOS-26-4`/`tvOS-26-4` runtime, and
  scope out macOS-only APIs (Opus).
- Simplify the `[Obsolete]`/`[Obsoleted…]` exemption; trim SKILL.md↔reference redundancy.

Confirmed core claims all correct across reviewers: error-enum scope, no known-failures
allowlist, bgen version-inheritance mechanism, named-delegate rule absent from SKILL.md,
host-OS gating.

**Outcome:** ✅ kept — three cycles applied, all edits backed by real session evidence and
triple-model review; each closes a false-negative in the exact rules the sessions exposed.

### Patterns learned
- **Skill guidance can be *actively wrong* for a subclass of cases** (generic enum rule vs
  error enums). Highest-priority fix class: wrong > incomplete. Confirmed by 2/4 sessions.
- **Reference-only critical rules get missed** — the `Action<T>` rule lived in a reference and
  was still violated (needed a user correction). Compliance ❌ rules belong inline in SKILL.md.
- **Real session traces are a superior eval substrate** to synthetic quizzes: they show the
  exact failure, the reviewer that caught it, and the fix — no guessing at the gap.

### Open items
- None filed to Arena. The three edits are documentation-only, low-risk, and evidence-backed;
  no controlled A/B needed. Re-mine future binding sessions to confirm the error-enum mistake
  no longer recurs.

### Post-review revision (PR #26028, maintainer @rolfbjarne)

The PR was approved but the maintainer flagged several statements that needed correcting;
all were verified against the repo and fixed (each re-reviewed read-only by GPT-5.6 Sol,
Opus 4.8, Sonnet 5, and GPT-5.6 Terra):

- **Named-delegate rule was too absolute.** `Action<T>`/`Func<T>` now support nullable type
  arguments, so `[NullAllowed]`/nullability is no longer a reason to avoid them. Downgraded
  the SKILL.md + reference rule from "❌ NEVER use `Action<T>`" to "⚠️ PREFER a named delegate
  when a parameter's meaning isn't obvious from its type" (named delegates still uniquely carry
  parameter names + XML docs).
- **Host-OS gating ≠ TCC crash.** A member gated away because it's unavailable on the host OS
  is *skipped* by `SkipDueToAttribute` (`ApiBaseTest.cs:160`), not crashed. Removed the
  "(and may TCC-crash)" conflation; TCC crashes are a separate phenomenon (instantiating a
  privacy-gated type in `ApiCtorInitTest`).
- **Introspection can't catch privatized selectors.** `ApiSelectorTest` only fails if a
  selector is *truly gone* from the runtime; a header-removed-but-still-implemented ("private")
  selector still responds, so introspection stays green while the binding rots. Clarified: don't
  rely on introspection to decide which selectors to remove.
- **Frameworks.cs sort convention.** Corrected the vague "keep it sorted" to the real rule:
  grouped by the OS version recorded in the entry (newest last), then **case-insensitive**
  alphabetical by managed name within each version (Opus caught that it's not ordinal —
  `MetalPerformanceShadersGraph` precedes `MLCompute`).
- **Cecil doc baseline.** Led the taxonomy with the easier `WRITE_KNOWN_FAILURES=1` self-update
  (per the maintainer), keeping the taxonomy for the hand-written-docs path.
- **mlaunch `--json`.** Framed as a likely mlaunch bug rather than an environment quirk.
- **Two follow-up issues filed** at the maintainer's request: #26052 (generate-frameworks-constants
  not rebuilt when `tools/common/Frameworks.cs` changes — root cause: `scripts/template.mk`'s
  `TemplateScript` only tracks the tool's own `*.cs`/`*.csproj`, not its linked common sources)
  and #26053 ([xtro] report extra/unknown selectors).
