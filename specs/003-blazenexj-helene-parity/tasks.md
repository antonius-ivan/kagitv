# Tasks: Phase 003 BlazeNexJ Helene Parity

**Input**: Design documents from `/specs/003-blazenexj-helene-parity/`
**Prerequisites**: plan.md, spec.md, research.md, exceptions.md, checklists/requirements.md

**Tests**: No new automated tests are planned for this phase. Validation uses the existing BlazeNexJ typecheck/build workflow, AppHost build validation, and browser smoke checks against the implemented parity routes.

**Organization**: Tasks are grouped by user story so the remaining Phase 003 work closes route parity, shared frontend capability parity, and parity-baseline documentation independently.

## Phase 1: Setup (Shared Infrastructure)

**Purpose**: Refresh the feature artifacts so they describe the implemented BlazeNexJ parity baseline rather than the earlier bootstrap-era gap analysis

- [X] T001 Refresh the implemented route and support-area inventory in specs/003-blazenexj-helene-parity/research.md
- [X] T002 [P] Refresh the current intentional divergence summary in specs/003-blazenexj-helene-parity/exceptions.md
- [X] T003 [P] Align the phase summary and validation framing to the implemented repo state in specs/003-blazenexj-helene-parity/plan.md

---

## Phase 2: Foundational (Blocking Prerequisites)

**Purpose**: Establish the shared parity-status, runtime wiring, and closure criteria that all remaining user-story work depends on

**⚠️ CRITICAL**: Do not treat any user story as complete until the parity inventory, route status model, and validation rules all reflect the current codebase

- [X] T004 Reconcile route readiness states, notes, and remaining-gap labels in src/BlazeNexJ/app/parity-data.ts
- [X] T005 [P] Reconcile duplicated runtime service discovery logic in src/BlazeNexJ/app/template-wiring.ts and src/BlazeNexJ/app/extensions/runtime-config.ts
- [X] T006 [P] Reconfirm the AppHost-to-BlazeNexJ endpoint handoff assumptions in src/ICLAco.AppHost/Program.cs
- [X] T007 Update closure-oriented validation and acceptance wording in specs/003-blazenexj-helene-parity/checklists/requirements.md

**Checkpoint**: Foundation ready - remaining parity closure work can now proceed by story

---

## Phase 3: User Story 1 - Equivalent Frontend Surface (Priority: P1) 🎯 MVP

**Goal**: Finish and validate the remaining route-surface and landing-experience gaps so Kagitv BlazeNexJ presents a coherent Helene-style frontend surface rather than a partly completed parity inventory

**Independent Test**: Start BlazeNexJ and verify `/`, `/home`, `/dashboard`, `/auth`, `/salesitem`, `/health`, `/cart`, `/checkout`, `/services`, and `/user` render as intentional entry surfaces with parity-oriented navigation and no regression to the old bootstrap-only experience

### Implementation for User Story 1

- [X] T008 [P] [US1] Tighten the parity landing hub, route summary, and navigation cues in src/BlazeNexJ/app/page.tsx
- [X] T009 [P] [US1] Align the home-route entry surface with the approved parity hub language in src/BlazeNexJ/app/home/page.tsx
- [X] T010 [P] [US1] Replace placeholder-only cart and checkout copy with approved commerce-entry content in src/BlazeNexJ/app/cart/page.tsx and src/BlazeNexJ/app/checkout/page.tsx
- [X] T011 [US1] Normalize the remaining top-level parity entry surfaces in src/BlazeNexJ/app/about/page.tsx, src/BlazeNexJ/app/config/page.tsx, src/BlazeNexJ/app/contacts/page.tsx, src/BlazeNexJ/app/services/page.tsx, and src/BlazeNexJ/app/user/page.tsx
- [X] T012 [US1] Run and record route-surface smoke validation through src/BlazeNexJ/package.json

**Checkpoint**: User Story 1 should leave the parity route surface coherent and independently reviewable in the running frontend

---

## Phase 4: User Story 2 - Equivalent Shared Frontend Capability (Priority: P2)

**Goal**: Close the remaining shared-capability gaps so BlazeNexJ's route support structure, dashboard composition, auth surfaces, and runtime wiring match the architectural role expected by the Helene baseline

**Independent Test**: Inspect the BlazeNexJ app tree and runtime behavior to confirm shared support areas, dashboard composition, auth subroutes, and runtime service configuration are all present, internally consistent, and passing typecheck/build validation

### Implementation for User Story 2

- [X] T013 [P] [US2] Consolidate runtime-config and template-wiring responsibilities in src/BlazeNexJ/app/template-wiring.ts and src/BlazeNexJ/app/extensions/runtime-config.ts
- [X] T014 [P] [US2] Align shared route-group and parity metadata usage in src/BlazeNexJ/app/extensions/route-groups.ts and src/BlazeNexJ/app/types.ts
- [X] T015 [P] [US2] Finish the dashboard composition handoff between src/BlazeNexJ/app/dashboard/page.tsx and src/BlazeNexJ/app/msft/dashboard/
- [X] T016 [P] [US2] Finish the auth entry-surface and subroute handoff in src/BlazeNexJ/app/auth/page.tsx, src/BlazeNexJ/app/auth/login/page.tsx, src/BlazeNexJ/app/auth/logout/page.tsx, and src/BlazeNexJ/app/auth/callback/page.tsx
- [X] T017 [US2] Validate shared frontend capability integrity through src/BlazeNexJ/package.json and src/ICLAco.AppHost/ICLAco.AppHost.csproj

**Checkpoint**: User Story 2 should leave the shared frontend structure and runtime wiring materially aligned to the Helene baseline

---

## Phase 5: User Story 3 - Controlled Parity Baseline (Priority: P3)

**Goal**: Make the remaining drift explicit so later frontend work can distinguish completed parity from approved exceptions instead of inheriting stale or ambiguous status notes

**Independent Test**: A reviewer can compare the Phase 003 artifacts to the current repo and determine what is complete, what remains intentionally lighter than Helene, and what validation was performed without inspecting unrelated history

### Implementation for User Story 3

- [X] T018 [P] [US3] Refresh the implemented-versus-deferred parity narrative in specs/003-blazenexj-helene-parity/research.md
- [X] T019 [P] [US3] Rewrite the active parity exceptions and exit criteria from the current codebase in specs/003-blazenexj-helene-parity/exceptions.md
- [X] T020 [US3] Update the feature requirements and success-criteria interpretation for closure-oriented parity review in specs/003-blazenexj-helene-parity/spec.md
- [X] T021 [US3] Record the final validation path, remaining divergence boundaries, and post-Phase-003 handoff in specs/003-blazenexj-helene-parity/plan.md

**Checkpoint**: User Story 3 should leave Phase 003 with an explicit, reviewable parity baseline and exception list

---

## Phase 6: Polish & Cross-Cutting Concerns

**Purpose**: Reconcile terminology, rerun final validation, and leave the feature artifacts ready for QA and Human000 review

- [X] T022 [P] Reconcile parity status terminology across specs/003-blazenexj-helene-parity/spec.md, specs/003-blazenexj-helene-parity/plan.md, specs/003-blazenexj-helene-parity/research.md, and specs/003-blazenexj-helene-parity/exceptions.md
- [X] T023 [P] Re-run the requirements quality pass in specs/003-blazenexj-helene-parity/checklists/requirements.md
- [X] T024 Validate final BlazeNexJ and AppHost closure checks through src/BlazeNexJ/package.json and src/ICLAco.AppHost/ICLAco.AppHost.csproj

---

## Dependencies & Execution Order

### Phase Dependencies

- **Setup (Phase 1)**: No dependencies - start immediately to refresh stale feature artifacts
- **Foundational (Phase 2)**: Depends on Setup completion - blocks all user stories because route-status and runtime-wiring facts must be stable first
- **User Story 1 (Phase 3)**: Depends on Foundational completion
- **User Story 2 (Phase 4)**: Depends on Foundational completion and benefits from the route-status alignment completed for User Story 1
- **User Story 3 (Phase 5)**: Depends on User Story 1 and User Story 2 because the artifact closure work must reflect validated frontend reality
- **Polish (Phase 6)**: Depends on all intended user stories being complete

### User Story Dependencies

- **User Story 1 (P1)**: Can start after Foundational - establishes the user-visible parity surface and MVP validation path
- **User Story 2 (P2)**: Can start after Foundational - should follow the shared status and runtime assumptions settled in Phase 2
- **User Story 3 (P3)**: Depends on the evidence from User Story 1 and User Story 2 to document completed parity versus approved exceptions accurately

### Parallel Opportunities

- Setup tasks `T002` and `T003` can run in parallel after `T001`
- Foundational tasks `T005` and `T006` can run in parallel after `T004`
- User Story 1 tasks `T008`, `T009`, and `T010` can run in parallel
- User Story 2 tasks `T013`, `T014`, `T015`, and `T016` can run in parallel once the foundational status model is stable
- User Story 3 tasks `T018` and `T019` can run in parallel
- Polish tasks `T022` and `T023` can run in parallel

---

## Parallel Example: User Story 1

```text
Task: "Tighten the parity landing hub, route summary, and navigation cues in src/BlazeNexJ/app/page.tsx"
Task: "Align the home-route entry surface with the approved parity hub language in src/BlazeNexJ/app/home/page.tsx"
Task: "Replace placeholder-only cart and checkout copy with approved commerce-entry content in src/BlazeNexJ/app/cart/page.tsx and src/BlazeNexJ/app/checkout/page.tsx"
```

---

## Parallel Example: User Story 2

```text
Task: "Consolidate runtime-config and template-wiring responsibilities in src/BlazeNexJ/app/template-wiring.ts and src/BlazeNexJ/app/extensions/runtime-config.ts"
Task: "Align shared route-group and parity metadata usage in src/BlazeNexJ/app/extensions/route-groups.ts and src/BlazeNexJ/app/types.ts"
Task: "Finish the dashboard composition handoff between src/BlazeNexJ/app/dashboard/page.tsx and src/BlazeNexJ/app/msft/dashboard/"
Task: "Finish the auth entry-surface and subroute handoff in src/BlazeNexJ/app/auth/page.tsx, src/BlazeNexJ/app/auth/login/page.tsx, src/BlazeNexJ/app/auth/logout/page.tsx, and src/BlazeNexJ/app/auth/callback/page.tsx"
```

---

## Parallel Example: User Story 3

```text
Task: "Refresh the implemented-versus-deferred parity narrative in specs/003-blazenexj-helene-parity/research.md"
Task: "Rewrite the active parity exceptions and exit criteria from the current codebase in specs/003-blazenexj-helene-parity/exceptions.md"
```

---

## Implementation Strategy

### MVP First (User Story 1 Only)

1. Complete Phase 1: Setup
2. Complete Phase 2: Foundational
3. Complete Phase 3: User Story 1
4. Stop and validate the route surface in the running BlazeNexJ app

### Incremental Delivery

1. Refresh the stale artifact baseline in Setup + Foundational
2. Close and validate the user-visible route surface in User Story 1
3. Close shared-capability and runtime-structure gaps in User Story 2
4. Lock the parity baseline and approved exceptions in User Story 3
5. Finish with terminology reconciliation and final validation in Polish

### Parallel Team Strategy

1. One owner refreshes feature artifacts in Setup + Foundational
2. One frontend owner closes route-surface tasks in User Story 1
3. One frontend/platform owner closes shared-capability tasks in User Story 2
4. A coordinating owner captures final parity-baseline and exception status in User Story 3

---

## Notes

- [P] tasks touch separate files or separable file groups and can be completed independently
- [US1], [US2], and [US3] map directly to the user stories in specs/003-blazenexj-helene-parity/spec.md
- This task list assumes most Phase 003 route scaffolding and support folders already exist in src/BlazeNexJ and focuses the remaining work on validation, targeted completion, and artifact closure
- `catalog` remains treated as the first live sales-item slice unless User Story 3 intentionally changes that exception stance