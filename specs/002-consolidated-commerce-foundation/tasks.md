# Tasks: Phase 002 Consolidation.API Creation Approval Reconciliation

**Input**: Design documents from `/specs/002-consolidated-commerce-foundation/`
**Prerequisites**: plan.md, spec.md, checklists/requirements.md

**Tests**: No new automated tests are planned for this phase. Validation is document and artifact review against the current repository state.

**Organization**: Tasks are grouped by user story so each approval and governance outcome can be completed and validated independently.

## Phase 1: Setup (Shared Infrastructure)

**Purpose**: Create the Phase 002 artifact set needed to reconcile approval state with existing implementation

- [X] T001 Create schema inventory notes in specs/002-consolidated-commerce-foundation/research.md
- [X] T002 [P] Create Human000 approval summary in specs/002-consolidated-commerce-foundation/approval-summary.md
- [X] T003 [P] Create DBA002 review worksheet in specs/002-consolidated-commerce-foundation/dba-review.md
- [X] T004 [P] Create authorization handoff note in specs/002-consolidated-commerce-foundation/authorization-status.md

---

## Phase 2: Foundational (Blocking Prerequisites)

**Purpose**: Establish the baseline facts and governance rules that all user stories depend on

**⚠️ CRITICAL**: No user story work should be considered complete until this phase is finished

- [X] T005 Consolidate current schema scope and implementation evidence in specs/002-consolidated-commerce-foundation/research.md
- [X] T006 [P] Define the approval-state vocabulary and handoff rules in specs/002-consolidated-commerce-foundation/authorization-status.md
- [X] T007 [P] Define the DBA002 review sections for scope, schema impact, EF Core impact, migration risk, performance notes, and compatibility notes in specs/002-consolidated-commerce-foundation/dba-review.md
- [X] T008 Align the phase narrative with the new artifact set in specs/002-consolidated-commerce-foundation/plan.md

**Checkpoint**: Foundation ready - the approval reconciliation stories can now proceed independently

---

## Phase 3: User Story 1 - Human000 Reviews Consolidation.API Creation Scope (Priority: P1) 🎯 MVP

**Goal**: Publish a Human000-readable summary of the already-created `Consolidation.API`, the current schema areas in scope, and the recorded approval decision

**Independent Test**: Human000 can read the Phase 002 artifacts and understand the approval decision, current schema scope, and remaining gate without inspecting source files

### Implementation for User Story 1

- [X] T009 [US1] Draft the Human000-readable Phase 002 scope summary in specs/002-consolidated-commerce-foundation/approval-summary.md
- [X] T010 [P] [US1] Update the approval reconciliation narrative in specs/002-consolidated-commerce-foundation/spec.md
- [X] T011 [US1] Update the validation and implementation approach for the recorded approval state in specs/002-consolidated-commerce-foundation/plan.md
- [X] T012 [US1] Reconfirm specification readiness notes against the approval summary in specs/002-consolidated-commerce-foundation/checklists/requirements.md

**Checkpoint**: User Story 1 should be independently reviewable by Human000

---

## Phase 4: User Story 2 - Human000 Gets DBA002 Overview (Priority: P2)

**Goal**: Record the DBA002 review path and capture the eventual review outcome or explicit waiver in a durable artifact

**Independent Test**: A reviewer can determine from the artifact set whether DBA002 review is pending, completed, or waived, and what risks or boundaries were identified

### Implementation for User Story 2

- [X] T013 [US2] Record the TPM001 routing prompt and DBA002 review request in specs/002-consolidated-commerce-foundation/dba-review.md
- [X] T014 [P] [US2] Update the review-gate language in specs/002-consolidated-commerce-foundation/spec.md
- [X] T015 [US2] Capture the DBA002 overview findings or explicit waiver decision in specs/002-consolidated-commerce-foundation/dba-review.md
- [X] T016 [US2] Reflect the DBA002 status and resulting authorization boundary in specs/002-consolidated-commerce-foundation/authorization-status.md

**Checkpoint**: User Story 2 should make the DBA002 gate explicit and auditable on its own

---

## Phase 5: User Story 3 - Approve Implementation Of The Latest Schema Cycle (Priority: P3)

**Goal**: Close the Phase 002 approval loop so later backend work has an explicit recorded authorization state

**Independent Test**: A delivery role can read the artifact set and determine whether Phase 002 is closed, what was authorized, and what next phase may proceed

### Implementation for User Story 3

- [X] T017 [US3] Record the final authorization state for Phase 002 in specs/002-consolidated-commerce-foundation/authorization-status.md
- [X] T018 [US3] Update the approval record and feature status in specs/002-consolidated-commerce-foundation/spec.md
- [X] T019 [US3] Update the handoff and closure language in specs/002-consolidated-commerce-foundation/plan.md
- [X] T020 [US3] Mark the final checklist state for the closed approval loop in specs/002-consolidated-commerce-foundation/checklists/requirements.md

**Checkpoint**: Phase 002 should now have an explicit closed or intentionally open authorization outcome

---

## Phase 6: Polish & Cross-Cutting Concerns

**Purpose**: Make the artifact set internally consistent and ready for later phase handoff

- [X] T021 [P] Reconcile terminology and cross-references in specs/002-consolidated-commerce-foundation/spec.md
- [X] T022 [P] Reconcile terminology and cross-references in specs/002-consolidated-commerce-foundation/plan.md
- [X] T023 [P] Reconcile terminology and cross-references in specs/002-consolidated-commerce-foundation/approval-summary.md
- [X] T024 [P] Reconcile terminology and cross-references in specs/002-consolidated-commerce-foundation/dba-review.md
- [X] T025 [P] Reconcile terminology and cross-references in specs/002-consolidated-commerce-foundation/authorization-status.md
- [X] T026 Validate next-phase handoff language in specs/002-consolidated-commerce-foundation/spec.md

---

## Dependencies & Execution Order

### Phase Dependencies

- **Setup (Phase 1)**: No dependencies - can start immediately
- **Foundational (Phase 2)**: Depends on Setup completion - blocks all user stories
- **User Story 1 (Phase 3)**: Depends on Foundational completion
- **User Story 2 (Phase 4)**: Depends on Foundational completion and benefits from User Story 1 context
- **User Story 3 (Phase 5)**: Depends on User Story 2 because the final authorization state depends on the DBA002 review outcome or waiver
- **Polish (Phase 6)**: Depends on all intended user stories being complete

### User Story Dependencies

- **User Story 1 (P1)**: Can start after Foundational - no dependency on other stories
- **User Story 2 (P2)**: Can start after Foundational - depends on the artifact baseline from Setup and Foundational
- **User Story 3 (P3)**: Depends on User Story 2 because authorization closure requires the DBA002 gate outcome

### Parallel Opportunities

- Setup tasks `T002`, `T003`, and `T004` can run in parallel
- Foundational tasks `T006` and `T007` can run in parallel
- User Story 1 task `T010` can run in parallel with `T009` once the artifact baseline exists
- User Story 2 task `T014` can run in parallel with `T013`
- Polish tasks `T021` through `T025` can run in parallel

---

## Parallel Example: User Story 1

```text
Task: "Draft the Human000-readable Phase 002 scope summary in specs/002-consolidated-commerce-foundation/approval-summary.md"
Task: "Update the approval reconciliation narrative in specs/002-consolidated-commerce-foundation/spec.md"
```

---

## Parallel Example: User Story 2

```text
Task: "Record the TPM001 routing prompt and DBA002 review request in specs/002-consolidated-commerce-foundation/dba-review.md"
Task: "Update the review-gate language in specs/002-consolidated-commerce-foundation/spec.md"
```

---

## Implementation Strategy

### MVP First (User Story 1 Only)

1. Complete Phase 1: Setup
2. Complete Phase 2: Foundational
3. Complete Phase 3: User Story 1
4. Stop and validate that Human000 can review the approval and scope summary without code inspection

### Incremental Delivery

1. Complete Setup + Foundational to establish the artifact baseline
2. Add User Story 1 to make the Human000 approval state explicit
3. Add User Story 2 to record the DBA002 review path and outcome
4. Add User Story 3 to close the authorization loop and hand off to the next phase
5. Finish with cross-cutting reconciliation so all Phase 002 artifacts say the same thing

### Parallel Team Strategy

1. One owner prepares the baseline artifacts in Setup + Foundational
2. A documentation owner handles approval summary and spec alignment for User Story 1
3. A database owner or coordinator handles DBA002 review capture for User Story 2
4. A coordinating owner closes authorization state and handoff for User Story 3

---

## Notes

- [P] tasks touch different files and can be completed independently
- [US1], [US2], and [US3] map directly to the user stories in spec.md
- This phase is intentionally artifact-driven; it does not introduce new backend feature work
- User Story 3 should not be treated as complete until the DBA002 gate is either completed or explicitly waived