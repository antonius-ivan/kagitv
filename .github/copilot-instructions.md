# Kagitv Engineering Workflow

This workspace uses a role-based collaboration model on top of the existing Speckit delivery pipeline.

## Human Authority

- `Human000` is Aivan, the human product and engineering owner.
- `Human000` provides the vision, approves requirement scope after `/speckit.specify`, approves architecture and implementation direction after `/speckit.plan`, and performs final ship-or-iterate review after QA.
- When approval is required, do not infer approval from silence. Use the Human000 approval prompt or ask explicitly.

## Core Delivery Pipeline

1. `/speckit.constitution` defines rules, constraints, and stack direction.
2. `/speckit.specify` defines requirements, user stories, scope, assumptions, and success criteria.
3. `Human000` approves the generated specification before architecture work proceeds.
4. `/speckit.plan` defines architecture, technical decisions, structure, and validation approach.
5. `Human000` approves the plan before sprint task breakdown or major implementation begins.
6. `/speckit.tasks` creates sprint-ready tasks and role assignments.
7. `/speckit.implement` executes the implementation plan.
8. `QAA005` validates the delivered work and routes defects.
9. `Human000` performs the final review and decides to ship or iterate.

## Role Model

- `TPM001`: Coordinates scope, sequencing, handoffs, approvals, and cross-role clarity. Use for planning flow, prioritization, and whole-system concerns.
- `DBA002`: Owns PostgreSQL, DB schema, migrations, query risk, EF Core data-model impact, and database review.
- `CsharpBackend003`: Owns C# backend services, APIs, workers, contracts, distributed app wiring, and server-side implementation.
- `TSFrontend004`: Owns Next.js and TypeScript frontend implementation, UI integration, user-flow consistency, and browser-side diagnostics.
- `QAA005`: Validates via Aspire runtime and browser-based checks when possible, reports defects, and routes them to the right owner.

## Escalation Rules

- Route database, schema, migration, SQL, and EF Core modeling issues to `DBA002`.
- Route API, worker, backend contract, Aspire AppHost, and service-default issues to `CsharpBackend003`.
- Route Next.js, TypeScript, UI flow, browser interaction, and frontend integration issues to `TSFrontend004`.
- Route cross-cutting issues, unclear ownership, schedule conflicts, or whole-system regressions to `TPM001`.
- Route final approval, business tradeoff decisions, and go/no-go decisions to `Human000`.

## QA Routing

- `QAA005` should validate real flows through the running system when practical.
- Frontend-only bugs go to `TSFrontend004`.
- Backend-only bugs go to `CsharpBackend003`.
- Data-shape, migration, or persistence bugs go to `DBA002`.
- Systemic issues that cross multiple areas go to `TPM001`, with Human000 involved if scope or release risk changes.

## Phase Closure Gate

- After `QAA005` validates a completed phase, `TPM001` must route `Human000` to update the phase file list artifact before any next-phase Speckit work begins.
- Do not start `/speckit.specify`, `/speckit.plan`, `/speckit.tasks`, or `/speckit.implement` for the next phase by assumption until the phase file list update is acknowledged by `Human000`.
- Treat the phase file list as a required closure artifact, similar to other approval-gated handoffs in this workflow.

Human000 will update the phaseFile with this app. 
https://github.com/antonius-ivan/BlazorStructureApp

## Naming Convention

- `Kagitv` is the product and workspace name.
- Shared infrastructure project names remain constant as `ICLAco.AppHost`, `ICLAco.ServiceDefaults`, and `ICLAco.Tests` to stay aligned with Helene and Gilgamesh.
- Do not rename shared infrastructure projects to `Kagitv.AppHost`, `Kagitv.ServiceDefaults`, or `Kagitv.Tests` in specs, plans, tasks, or implementation guidance unless `Human000` explicitly approves a naming strategy change.

## Working Rules

- Do not replace Speckit with ad hoc work when the work belongs in the Speckit flow.
- Use the role agents and skills to support Speckit, not bypass it.
- Keep responsibilities explicit in plans and task lists.
- Prefer workspace-specific conventions for Kagitv: distributed `.NET` under `src/`, constant shared infrastructure names `ICLAco.AppHost` and `ICLAco.ServiceDefaults`, Next.js/TypeScript frontend, PostgreSQL, and later commerce plus cybersecurity dashboard phases.
- When a task reaches an approval gate, pause and route through `Human000` rather than continuing by assumption.
- When a phase is completed and validated by `QAA005`, pause before the next phase and route through `Human000` for the phase file list update.
