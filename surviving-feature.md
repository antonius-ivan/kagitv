# Downspec Plan

## Feature That Survive
- BlazeNexJ only for catalog browsing and login.
- Don't Touch Consolidation API
- Dashboard Menu
- don't touch identity.api

## Feature Removed
- RabbitMQ
- Redis
- Basket flow
- Checkout flow
- Order processing workers
- Message-driven integration flow
- Legacy Blazor frontend surfaces
- PaymentFlow

## Feature Adjusted
- AppHost

## Target Baseline
- Keep BlazeNexJ as the only frontend surface.
- Keep only the backend services required to support catalog and login.
- Treat SalesItem.API as the likely catalog backend.
- Treat Identity.API as the likely login backend.
- Keep AppHost and ServiceDefaults only if they are still needed to run the reduced baseline.
- Keep Consolidation.API only if it is still required for the reduced data/runtime shape.

## Execution Plan
1. Remove solution and AppHost references to deleted services and deleted infrastructure.
2. Remove RabbitMQ and Redis wiring from runtime configuration, service discovery, and local orchestration.
3. Remove basket, checkout, worker, and event-bus code paths that no longer support the surviving scope.
4. Verify BlazeNexJ only exposes catalog and login-oriented routes as the supported product surface.
5. Verify Identity and catalog backend paths still run end-to-end under the reduced baseline.
6. Update project description, phase files, and other root documents so they describe the smaller catalog-and-login product.

## Verification
- If any removed feature (e.g., basket, order-processing), links must be hidden or disabled
- BlazeNexJ can open catalog pages.
- BlazeNexJ can reach login flow.
- No build/runtime dependency remains on RabbitMQ or Redis.
- No active repo narrative still describes the removed commerce stack as current.



Plan: Downspec To Catalog Login
The surviving product surface is BlazeNexJ for catalog browsing and login only. RabbitMQ and Redis are explicitly removed. That means basket, checkout, order-processing, and message-driven flows should be treated as removed unless a direct catalog or auth dependency proves otherwise.

Steps

Lock the surviving scope to catalog and login in BlazeNexJ.
Remove infrastructure assumptions that only exist for removed features, especially RabbitMQ and Redis.
Reconcile runtime wiring in AppHost and solution membership so deleted services are no longer required.
Keep only the backend/platform pieces needed to support catalog and login: most likely SalesItem.API, Identity.API, AppHost, and ServiceDefaults, with Consolidation.API kept only if it is still needed for the reduced runtime.
Update stale docs and phase artifacts that still describe the larger commerce stack.