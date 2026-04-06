# Research: Phase 003 BlazeNexJ Helene Parity

## Decision Summary

- Phase 003 goal is parity with Helene BlazeNexJ.
- Helene route and capability structure is the approved frontend baseline.
- The previous “template wiring” interpretation is not the primary success criterion for this phase.

## Helene Baseline Inventory

### Route Areas Observed Under `E:\Netaspcore10-1\Helene\src\BlazeNexJ\app`

- `about/`
- `auth/`
- `cart/`
- `checkout/`
- `config/`
- `contacts/`
- `dashboard/`
- `health/`
- `home/`
- `salesitem/`
- `services/`
- `user/`

### Additional Frontend Support Areas

- `aacomponents/`
- `api/`
- `extensions/`
- `msft/`
- `tailwindsamplepages/`
- `types.ts`
- `globals.css`
- `instrumentation.ts`
- `layout.tsx`
- `page.tsx`
- `server.ts`

### Helene Package Baseline Signals

Observed in `E:\Netaspcore10-1\Helene\src\BlazeNexJ\package.json`:

- Fluent UI packages
- Tailwind/PostCSS
- OpenTelemetry packages
- Redis client
- Winston logging
- Helper utilities such as `match-sorter`, `sort-by`, and `tiny-invariant`

Interpretation: Helene parity is broader than page count. It includes a richer frontend support structure and dependency baseline.

## Current Glaive State

### Current Glaive Route/Support Inventory Under `src/BlazeNexJ/app`

- `about/`
- `aacomponents/`
- `api/`
- `auth/`
- `cart/`
- `catalog/`
- `checkout/`
- `config/`
- `contacts/`
- `dashboard/`
- `extensions/`
- `health/`
- `home/`
- `globals.css`
- `instrumentation.ts`
- `layout.tsx`
- `msft/`
- `page.tsx`
- `parity-data.ts`
- `parity-entry-page.tsx`
- `parity-subroute-page.tsx`
- `salesitem/`
- `server.ts`
- `services/`
- `tailwindsamplepages/`
- `template-wiring.ts`
- `types.ts`
- `user/`

### Current Gap

Glaive now carries the approved top-level parity route areas and the expected supporting route-level structure. The remaining gap is intentional depth, not missing topology: several areas remain lighter than Helene in behavior, package stack, and UI richness.

## Planning Implications

- Phase 003 can now be reviewed as a completed parity-baseline phase with documented exceptions.
- Existing Glaive work such as the health route and initial catalog slice has been preserved inside the parity baseline.
- Root experience parity has been achieved by replacing the Phase 001 placeholder with a route-oriented parity hub.
- Remaining differences from Helene should be tracked as post-Phase-003 follow-on work rather than left ambiguous.

## Recommended Implementation Order

1. Treat the current route and support-tree parity as the approved baseline.
2. Use the exceptions list to decide which lighter areas should deepen in a later phase.
3. Preserve the catalog slice and runtime wiring as the first live commerce-facing frontend capability.
4. Avoid reopening Phase 003 for unrelated backend or dependency expansion that is not required by the parity baseline.
5. Start any richer dashboard, auth, cart, checkout, or user behavior as a follow-on phase with explicit scope.