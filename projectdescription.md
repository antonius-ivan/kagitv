# Project Inventory Summary

Generated from:
- `src/**/*.csproj`
- `dbdiagram.dbml`

## Services and Projects

- Active downspec baseline centers on the reduced catalog-and-login surface described in `surviving-feature.md`.
- Supported runtime projects in the active baseline: **5 .NET projects + 1 Node frontend**

Active baseline projects:
1. `src/BlazeNexJ/BlazeNexJ.esproj`
2. `src/ICLAco.AppHost/ICLAco.AppHost.csproj`
3. `src/ICLAco.ServiceDefaults/ICLAco.ServiceDefaults.csproj`
4. `src/Identity.API/Identity.API.csproj`
5. `src/SalesItem.API/SalesItem.API.csproj`
6. `src/Consolidation.API/Consolidation.API.csproj`

Removed from the active runtime baseline:
1. `Basket.API`
2. `BlazeWeb`
3. `BlazeWebComponents`
4. `Ordering.API`
5. `OrderProcessor`
6. `PaymentProcessor`
7. `WebhookClient`
8. `Webhooks.API`
9. `EventBus`
10. `EventBusRabbitMQ`
11. `IntegrationEventLogEF`

## Database Schemas and Tables

From `dbdiagram.dbml`:
- Total schemas: **5**
- Total tables: **37**

Tables per schema:
1. `identity`: **12**
2. `ordering`: **13**
3. `sales`: **10**
4. `public`: **1**
5. `webhooksdb`: **1**

## Notes

- `sales` remains the key schema area for the surviving catalog experience.
- `identity` remains relevant for login and session-backed frontend access.
- `ordering` and `webhooksdb` still exist in the shared database, but their runtime features are outside the current downspecced storefront baseline.
- `Consolidation.API` remains in place and is intentionally left untouched.
