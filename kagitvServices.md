Current Status
├── .gitattributes
├── .gitignore
├── Directory.Packages.props (Solution Files)
└── src
    ├── BlazeNexJ (catalog browsing, login, dashboard menu)
    ├── Consolidation.API (kept in place, no downspec edits)
    ├── ICLAco.AppHost (reduced runtime orchestration)
    ├── ICLAco.ServiceDefaults
    ├── Identity.API (kept in place, no downspec edits)
    └── SalesItem.API (catalog backend)

Active downspec baseline

- Surviving user-facing scope is BlazeNexJ catalog browsing, login, and dashboard menu access.
- AppHost now runs only the reduced baseline services needed for that flow.
- RabbitMQ, Redis, basket, checkout, ordering workers, payment flow, webhooks, and legacy BlazeWeb surfaces are outside the active runtime baseline.

---

## Clarification: Why `Catalog` currently contains `Price`

The current design follows the eShop sample approach (learning-oriented):

- `Catalog.API` keeps a simple `Price` field directly in product data.
- This is enough for demo scenarios and simple checkout flow.
- It is **not** ideal for large-scale enterprise pricing (tax, promotions, multi-currency, auditing).

## What this means for Makkari today

With current services, price handling is simplified and mostly behaves like a single-price catalog model:

- Product details + base amount are driven from `Catalog.API`.
- Basket/ordering use those amounts for checkout flow.
- Advanced enterprise pricing rules are not yet modeled as a separate bounded context.

## Recommended target model for large Indonesia auto dealer

For your use case (sales item, tax, service tax, simple discount, flexible layered promotions), move toward this separation:

- `Catalog.API`: product/specification only (no final sale price logic).
- `Pricing Service` (new): price list, per-currency amounts, effective dates.
- `Tax Service` (new): PPN/service tax rules by region/type/date.
- `Promotion Service` (new): stackable and prioritized discount rules.
- `Ordering.API`: store immutable sales snapshot at checkout.

## Currency strategy

Yes, currency should be normalized into separate tables rather than embedded in one catalog price column.

Minimum structure:

- `Currency` (`Code`, `Name`, `MinorUnit`, `Symbol`).
- `PriceList` (`Id`, `Name`, `EffectiveFrom`, `EffectiveTo`).
- `PriceListItem` (`ProductId`, `CurrencyCode`, `Amount`, validity window).

For orders, persist snapshot fields per line item:

- `BaseAmount`
- `TaxAmount`
- `ServiceTaxAmount`
- `DiscountAmount` (can be multiple detail rows)
- `FinalAmount`
- `CurrencyCode`
- `ExchangeRateUsed` (if conversion is applied)

## Practical next step

Keep the reduced catalog-and-login baseline stable first. Any future pricing, payment, or ordering work should be treated as a new scope decision instead of part of the current active runtime.


Todo List 15 Februari 2026
Create New Sprite Repo
Currency Must Indonesian Rupiah
Try Integrate the Payment.
Copilot said: Yes—this kind of architecture can integrate with
Yes—this kind of architecture can integrate with a payment gateway like Midtrans. In practice, the Payment Processor service would be the integration point. It would:

Create payment transactions with Midtrans (charge/authorize).
Handle payment status updates (success/failed/expired).
Receive Midtrans webhook notifications (callbacks) to confirm payment outcomes.
In that case:

Webhook API would expose an endpoint that Midtrans calls with payment updates.
Webhook Client might be used internally to fan out events (e.g., “PaymentSucceeded”) to other services after Midtrans confirms the payment.
If you want, tell me which eShop service you plan to modify and I can outline the exact flow with Midtrans’ API calls/webhooks.
then can you explain to me if it will integrate to midtrans payment gateway?