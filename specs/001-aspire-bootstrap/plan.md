# Implementation Plan: [FEATURE]

**Branch**: `[###-feature-name]` | **Date**: [DATE] | **Spec**: [link]
**Input**: Feature specification from `/specs/[###-feature-name]/spec.md`

**Note**: This template is filled in by the `/speckit.plan` command. See `.specify/templates/plan-template.md` for the execution workflow.

## Summary

[Extract from feature spec: primary requirement + technical approach from research]

## Technical Context

<!--
  ACTION REQUIRED: Replace the content in this section with the technical details
  for the project. The structure here is presented in advisory capacity to guide
  the iteration process.
-->

**Language/Version**: [e.g., Python 3.11, Swift 5.9, Rust 1.75 or NEEDS CLARIFICATION]  
**Primary Dependencies**: [e.g., FastAPI, UIKit, LLVM or NEEDS CLARIFICATION]  
**Storage**: [if applicable, e.g., PostgreSQL, CoreData, files or N/A]  
**Testing**: [e.g., pytest, XCTest, cargo test or NEEDS CLARIFICATION]  
**Target Platform**: [e.g., Linux server, iOS 15+, WASM or NEEDS CLARIFICATION]
**Project Type**: [e.g., library/cli/web-service/mobile-app/compiler/desktop-app or NEEDS CLARIFICATION]  
**Critical User Journeys**: [list the user-visible flows affected by this feature or NEEDS CLARIFICATION]
**Performance Goals**: [measurable budgets for affected critical journeys or NEEDS CLARIFICATION]  
**Constraints**: [domain-specific limits, compatibility requirements, or observability requirements]  
**Scale/Scope**: [domain-specific, e.g., 10k users, 1M LOC, 50 screens or NEEDS CLARIFICATION]

## Constitution Check

*GATE: Must pass before Phase 0 research. Re-check after Phase 1 design.*

- **Code Quality And Change Safety**: Describe the validation approach, failure modes, and
  rollback or mitigation path for risky changes.
- **User Experience Consistency**: Identify affected frontend journeys and the shared
  behavior rules for loading, empty, error, validation, and success states.
- **Performance Budgets**: Record measurable budgets for each critical journey affected by
  this feature.
- **Contract Consistency**: List APIs, messages, schemas, or shared data contracts changed
  by this feature and the compatibility plan.
- **Observability And Diagnosability**: List the logs, traces, metrics, health checks, or
  audit events required to operate this feature in production.

## Project Structure

### Documentation (this feature)

```text
specs/[###-feature]/
├── plan.md              # This file (/speckit.plan command output)
├── research.md          # Phase 0 output (/speckit.plan command)
├── data-model.md        # Phase 1 output (/speckit.plan command)
├── quickstart.md        # Phase 1 output (/speckit.plan command)
├── contracts/           # Phase 1 output (/speckit.plan command)
└── tasks.md             # Phase 2 output (/speckit.tasks command - NOT created by /speckit.plan)
```

### Source Code (repository root)
<!--
  ACTION REQUIRED: Replace the placeholder tree below with the concrete layout
  for this feature. Delete unused options and expand the chosen structure with
  real paths (e.g., apps/admin, packages/something). The delivered plan must
  not include Option labels.
-->

```text
src/
├── ICLAco.AppHost/             # .NET Aspire orchestration host
├── ICLAco.ServiceDefaults/     # Shared service defaults, telemetry, resilience
├── Shared/                     # Shared contracts, primitives, and utilities
├── [Feature].API/              # .NET Web API project(s)
├── [Feature].Domain/           # Domain model and business rules (when needed)
├── [Feature].Infrastructure/   # Persistence and external integrations (when needed)
├── [Feature].Worker/           # Background worker(s) and processors (optional)
├── BlazeWeb/                   # Blazor frontend app (optional)
├── BlazeWebComponents/         # Shared Blazor UI components (optional)
├── BlazeNexJ/                  # Next.js frontend app (optional)
└── EventBus/                   # Messaging and integration infrastructure (optional)

tests/
└── ICLAco.Tests/               # Test projects: unit, integration, contract, UI/e2e as needed

docs/                           # Project documentation, ADRs, architecture
specs/                          # Feature specs, plans, contracts, tasks
tools/                          # Developer tools, codegen, analyzers
```

**Structure Decision**: [Document the selected structure and reference the real
directories captured above]

## Complexity Tracking

> **Fill ONLY if Constitution Check has violations that must be justified**

| Violation | Why Needed | Simpler Alternative Rejected Because |
|-----------|------------|-------------------------------------|
| [e.g., 4th project] | [current need] | [why 3 projects insufficient] |
| [e.g., Repository pattern] | [specific problem] | [why direct DB access insufficient] |
