<!--
Sync Impact Report
Version change: 1.0.1 -> 1.0.2
Modified principles:
- None
Added sections:
- None
Removed sections:
- None
Templates requiring updates:
- None
Follow-up TODOs:
- TODO(RATIFICATION_DATE): Original ratification date is unknown and must be confirmed.
-->

# Glaive Constitution

## Core Principles

### I. Code Quality And Change Safety
Every production change MUST leave the codebase easier to reason about or no harder to
operate than before. Changes MUST define the intended behavior, failure behavior, and
rollback or mitigation path when they affect shared services, user-critical journeys, or
cross-cutting infrastructure. Automated tests are strongly encouraged and MAY be required
by the feature spec or plan for higher-risk changes, but this constitution does not make
them universally mandatory. Complexity, duplication, and hidden coupling MUST be reduced
when discovered rather than carried forward without written justification.

### II. User Experience Consistency
Equivalent user actions MUST behave consistently across all frontend flows. Features that
affect user-facing behavior MUST define consistent terminology, validation feedback,
loading states, empty states, error states, and success confirmation before implementation
begins. Intentional deviations are allowed only when documented in the spec with the
reason and the affected journeys.

### III. Performance Budgets For Critical Journeys
Every feature touching a user-critical journey MUST define measurable performance budgets
in planning. Budgets MUST be framed in user-observable terms such as response time,
interaction completion time, throughput, or acceptable degradation under load. Work that
cannot state its performance target is not ready for implementation, and work that exceeds
its agreed budget requires explicit approval and mitigation.

### IV. Contract Consistency For Shared Interfaces
Changes to APIs, messaging contracts, shared schemas, and other integration boundaries
MUST preserve compatibility or declare the breaking change, affected consumers, and
migration plan. Specs and plans MUST identify which contracts are touched and what form of
validation is required. No feature is complete if downstream impact is left implicit.

### V. Observability And Diagnosability
Production behavior MUST be diagnosable without guesswork. Features affecting critical
flows, integrations, or background processing MUST define the logs, traces, metrics,
health signals, or audit events needed to explain failures and confirm normal operation.
If the team cannot explain how a release will be observed in production, the release is
not ready.

## Delivery Standards

Specifications MUST identify impacted user journeys, compatibility concerns, and any
non-functional requirements needed to satisfy the core principles. Plans MUST translate
those requirements into concrete quality gates, performance targets, contract validation,
and observability work. Tasks MUST include the work needed to satisfy these concerns when
the feature changes critical journeys, shared interfaces, or production diagnostics.

## Review And Release Gates

Every review MUST confirm that the change:

- states the user journeys it affects
- preserves or intentionally changes UX behavior with rationale
- defines measurable performance expectations for critical paths
- documents contract impact for APIs, events, and shared data
- leaves enough observability to diagnose production behavior

Releases that fail one or more gates require an explicit exception note describing the
reason, approver, and follow-up work.

## Governance

This constitution supersedes default workflow preferences when there is a conflict.
Amendments MUST be recorded in this file with a semantic version update and a short impact
summary. MAJOR versions redefine or remove principles, MINOR versions add principles or
materially expand requirements, and PATCH versions clarify wording without changing
meaning. Compliance MUST be checked during specification, planning, task generation, and
review. Any approved exception MUST identify the violated principle, the business reason,
the approver, and the date for re-evaluation.

**Version**: 1.0.2 | **Ratified**: TODO(RATIFICATION_DATE): Original adoption date not recorded | **Last Amended**: 2026-04-03
