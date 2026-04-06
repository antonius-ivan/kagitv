# Phase 003 Parity Exceptions

## Current Intentional Deviations

1. `aacomponents`, `msft`, `api`, `extensions`, and `tailwindsamplepages` are now represented in Glaive, but they remain lighter than Helene and do not yet carry Helene's full Fluent UI or Tailwind implementation depth.
2. `dashboard`, `auth`, `cart`, `checkout`, `services`, and `user` currently provide coherent entry surfaces rather than Helene-equivalent end-to-end behavior.
3. Glaive retains the existing `catalog` route as the first live sales item slice even though Helene's surface is organized under `salesitem`.
4. `template-wiring.ts` remains in the repo only as a compatibility wrapper and now delegates its runtime service discovery to `extensions/runtime-config.ts`.
5. Helene package parity is intentionally incomplete. Glaive does not yet include Helene's broader frontend dependency stack for Fluent UI, Tailwind/PostCSS, OpenTelemetry, Redis, and Winston.

## Exit Criteria For Closing These Exceptions

1. Replace entry surfaces with richer route behavior where parity requires it, starting with `dashboard`, `auth`, and `user`.
2. Decide whether `catalog` is absorbed into `salesitem` or remains a documented long-term divergence.
3. Either remove `template-wiring.ts` entirely or keep it as a documented compatibility layer with no duplicated runtime logic.
4. Reassess package-level parity and adopt only the Helene dependencies that support approved frontend behavior.