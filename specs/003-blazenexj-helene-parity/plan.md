# Implementation Plan: Phase 003 BlazeNexJ Helene Parity

**Branch**: `003-blazenexj-helene-parity` | **Date**: 2026-04-03 | **Spec**: [spec.md](./spec.md)
**Input**: Feature specification from [spec.md](./spec.md)

## Summary

Bring `src/BlazeNexJ` in Kagitv to parity with the approved frontend baseline in `E:\Netaspcore10-1\Helene\src\BlazeNexJ`. Phase 003 is not a generic template-wiring effort. The primary goal is to close the route, page-surface, and shared frontend capability gap so Kagitv BlazeNexJ resembles Helene BlazeNexJ as the standard frontend baseline for later feature work.

## Technical Context

**Language/Version**: TypeScript with Next.js App Router on Node.js; supporting .NET Aspire wiring in `ICLAco.AppHost`
**Primary Dependencies**: Next.js, React, TypeScript, Node runtime server, existing AppHost endpoint wiring; Helene baseline also includes Fluent UI, Tailwind/PostCSS, OpenTelemetry, Redis, and Winston packages
**Storage**: No new frontend-owned storage planned in this phase
**Testing**: BlazeNexJ typecheck/build, AppHost build validation, browser smoke tests on key parity routes
**Target Platform**: Windows local development under Aspire orchestration with a Node-hosted Next.js frontend
**Project Type**: Frontend parity expansion of an existing Next.js app
**Critical User Journeys**:
- A developer can open Kagitv BlazeNexJ and see a route structure materially aligned with Helene rather than a one-page bootstrap app.
- A user can reach the approved major frontend areas from Kagitv BlazeNexJ, including parity entry points for informational, auth, cart, checkout, dashboard, sales item, service, and user areas.
- Delivery roles can compare Kagitv and Helene frontend trees and identify any remaining difference as an intentional exception rather than accidental drift.
**Performance Goals**:
- Kagitv BlazeNexJ should boot without regressing the current working homepage, health route, and first catalog slice.
- Key parity routes should render without introducing obvious build or runtime errors.
**Constraints**:
- Preserve `ICLAco.AppHost`, `ICLAco.ServiceDefaults`, and `ICLAco.Tests` names.
- Preserve the existing `src/BlazeNexJ` App Router structure.
- Treat Helene as the baseline for route and capability parity, not as a source for arbitrary backend expansion.
- Do not rewrite Phase 003 as a backend-heavy integration phase.
**Scale/Scope**: Lock Phase 003 as the implemented parity baseline that expands Kagitv BlazeNexJ from the original minimal surface into the broader Helene route inventory and shared frontend capability set.

**Naming Convention**: Product/workspace name is `Kagitv`; shared infrastructure project names remain `ICLAco.AppHost`, `ICLAco.ServiceDefaults`, and `ICLAco.Tests`.

## Constitution Check

*GATE: Must pass before Phase 0 research. Re-check after Phase 1 design.*

- **Code Quality And Change Safety**: This phase expands frontend routes and shared frontend structure inside `src/BlazeNexJ`. Validation centers on type safety, build integrity, and browser smoke tests on parity routes. The current catalog slice and AppHost endpoint wiring must remain functional while parity expands.
- **User Experience Consistency**: The phase explicitly replaces bootstrap-only UX with a broader parity-oriented frontend surface. New routes must feel like one coherent app rather than disconnected placeholders.
- **Performance Budgets**: The frontend must continue to build cleanly and render the primary routes without obvious runtime failure. No heavy client-state or telemetry additions should be introduced without need.
- **Contract Consistency**: Frontend parity may consume existing backend services, but the phase goal is route and capability parity with Helene, not a new backend contract design effort.
- **Observability And Diagnosability**: Existing health and runtime visibility must remain intact. Where parity routes are not yet fully wired to live services, failure states must be explicit and diagnosable.

## Project Structure

### Documentation (this feature)

```text
specs/003-blazenexj-helene-parity/
├── spec.md                # Approved parity-focused feature specification
├── plan.md                # This file
├── exceptions.md          # Current intentional parity deviations from Helene
├── research.md            # Helene-to-Kagitv parity inventory and gap summary
└── checklists/
    └── requirements.md    # Specification quality checklist
```

### Source Code (repository root)

```text
src/
├── BlazeNexJ/                    # Kagitv Next.js frontend under expansion
├── ICLAco.AppHost/               # Aspire host and frontend/backend endpoint wiring
├── ICLAco.ServiceDefaults/       # Shared diagnostics and service defaults
└── ... backend services already present in the repo

Helene reference:
E:\Netaspcore10-1\Helene\src\BlazeNexJ/
├── app/
│   ├── aacomponents/
│   ├── about/
│   ├── api/
│   ├── auth/
│   ├── cart/
│   ├── checkout/
│   ├── config/
│   ├── contacts/
│   ├── dashboard/
│   ├── extensions/
│   ├── health/
│   ├── home/
│   ├── msft/
│   ├── salesitem/
│   ├── services/
│   ├── tailwindsamplepages/
│   ├── user/
│   ├── globals.css
│   ├── instrumentation.ts
│   ├── layout.tsx
│   ├── page.tsx
│   ├── server.ts
│   └── types.ts
└── package.json
```

**Structure Decision**: Reuse the existing `src/BlazeNexJ` app and treat the current route surface, dashboard composition, runtime metadata, and preserved catalog/health slices as the approved Phase 003 baseline.

## Parity Inventory

### Helene Baseline Areas

- Route areas: `about`, `auth`, `cart`, `checkout`, `config`, `contacts`, `dashboard`, `health`, `home`, `salesitem`, `services`, `user`
- Supporting areas: `aacomponents`, `api`, `extensions`, `msft`, `tailwindsamplepages`, `types.ts`
- Shared frontend capabilities: richer package baseline, route-level composition, diagnostics/instrumentation presence, multiple UI surface patterns

### Current Kagitv State

- Present route areas: root parity hub, `about`, `auth`, `cart`, `catalog`, `checkout`, `config`, `contacts`, `dashboard`, `health`, `home`, `salesitem`, `services`, `tailwindsamplepages`, and `user`
- Present supporting files and areas: `aacomponents`, `api`, `extensions`, `msft`, `types.ts`, parity metadata helpers, `globals.css`, `instrumentation.ts`, `layout.tsx`, `server.ts`, and `template-wiring.ts`
- Remaining difference: several areas are intentionally lighter than Helene and are tracked in `exceptions.md`

### Planned Phase 003 Outcome

- Kagitv exposes approved top-level parity route entry areas matching Helene at a structural level
- Kagitv root experience no longer reads as a temporary integration dashboard only
- Shared frontend support files and route organization now materially align to the Helene baseline for this phase
- Intentionally lighter areas are captured as explicit parity exceptions instead of implicit gaps

## Implementation Approach

### Phase 0 Research

- Record the Helene route inventory and shared frontend capability baseline in `research.md`.
- Confirm the current Kagitv BlazeNexJ route and support-file inventory.
- Identify where Kagitv already has partial progress that can be preserved during parity work, such as `health` and the first `catalog` slice.

### Phase 1 Design

- Define the parity target as a concrete route-and-capability inventory rather than a vague “match Helene” statement.
- Organize the implementation into parity slices, starting with route scaffolding and shared frontend structure, then progressively filling those areas with representative entry surfaces.
- Preserve the current working app while reshaping the root surface toward Helene parity.

### Phase 2 Implementation Slices

**Result A: Route Surface Parity**
The approved top-level route areas and nested entry paths are present in Kagitv BlazeNexJ.
**Result B: Shared Frontend Structure Parity**
The supporting frontend structure expected by the Helene baseline is represented through `aacomponents`, `api`, `extensions`, `msft`, `tailwindsamplepages`, `types.ts`, and parity metadata helpers.
**Result C: Root Experience Parity**
The Kagitv root surface is a parity-oriented route hub rather than a template-wiring or bootstrap-only page.
**Result D: Capability Fill-In**
Representative route pages exist for auth, dashboard, salesitem, cart, checkout, services, and user areas, with remaining lighter behavior documented as exceptions.

### Phase 3 Exceptions And Validation

- Record any Helene area intentionally deferred from Kagitv as a parity exception.
- Validate route existence, build integrity, and browser reachability for the primary parity areas.

## Completion State

Phase 003 is implemented as a closure-oriented parity baseline. Remaining differences are explicit, bounded, and intended for later phases rather than unresolved within this one.

## Validation Approach

- Verify Kagitv and Helene route trees against the parity inventory in `research.md`.
- Verify `npm run typecheck` and `npm run build` continue to pass for `src/BlazeNexJ`.
- Verify `dotnet build` continues to pass for `src/ICLAco.AppHost/ICLAco.AppHost.csproj`.
- Verify browser smoke tests for `/`, `/health`, `/catalog`, and newly added parity routes.
- Verify the root user experience is no longer only a temporary integration status page.

## Risks And Mitigations

- **Risk**: The phase drifts back into generic service-integration work instead of frontend parity.
  **Mitigation**: Keep the parity inventory explicit and evaluate progress against Helene route and capability areas first.
- **Risk**: Kagitv accumulates placeholder routes that satisfy structure but not useful parity.
  **Mitigation**: Require each major parity area to present a coherent entry surface rather than an empty shell.
- **Risk**: Current working slices such as `catalog` regress during route restructuring.
  **Mitigation**: Re-run typecheck/build/browser smoke tests after each parity slice.
- **Risk**: Helene contains routes or packages that are not appropriate to copy wholesale.
  **Mitigation**: Treat Helene as the parity baseline but document any intentional exceptions explicitly.

## Complexity Tracking

No constitution violations identified. The main challenge is scope control: this phase should broaden the frontend toward Helene parity without turning into unrestricted backend or infrastructure expansion.