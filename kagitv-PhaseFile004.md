Current Status
├── .gitattributes
├── .gitignore
├── Directory.Packages.props (Solution Files)
├── src
│   ├── Basket.API
│   │   ├── Properties
│   │   │   └── launchSettings.json
│   │   ├── Extensions
│   │   │   ├── Extensions.cs
│   │   │   └── ServerCallContextIdentityExtensions.cs
│   │   ├── Grpc
│   │   │   └── BasketService.cs
│   │   ├── Model
│   │   │   ├── BasketItem.cs
│   │   │   └── CustomerBasket.cs
│   │   ├── Protos
│   │   │   └── basket.proto
│   │   ├── Repositories
│   │   │   ├── IBasketRepository.cs
│   │   │   └── RedisBasketRepository.cs
│   │   ├── appsettings.Development.json
│   │   ├── appsettings.json
│   │   ├── Basket.API.csproj
│   │   ├── GlobalUsings.cs
│   │   └── Program.cs
│   ├── BlazeNexJ
│   │   ├── app
│   │   │   ├── aacomponents
│   │   │   │   ├── layouts
│   │   │   │   │   ├── SharedAppFrame.tsx
│   │   │   │   │   ├── TwDefaultLayout.tsx
│   │   │   │   │   └── TwSalesitemListLayout.tsx
│   │   │   │   ├── navigation
│   │   │   │   │   ├── AppModeSwitcher.tsx
│   │   │   │   │   ├── CartMenu.tsx
│   │   │   │   │   ├── SharedTopBar.tsx
│   │   │   │   │   └── UserMenu.tsx
│   │   │   │   ├── server
│   │   │   │   │   ├── auth.ts
│   │   │   │   │   ├── basket.ts
│   │   │   │   │   ├── catalog.ts
│   │   │   │   │   ├── ordering.ts
│   │   │   │   │   └── runtime.ts
│   │   │   │   └── SalesitemListLayout.tsx
│   │   │   ├── afluentcomponents
│   │   │   │   ├── FluentDashboardLayout.tsx
│   │   │   │   ├── FluentDashPages.tsx
│   │   │   │   ├── FluentProviderRegistry.tsx
│   │   │   │   └── FluentSampleButton.tsx
│   │   │   ├── api
│   │   │   │   ├── catalog
│   │   │   │   │   └── items
│   │   │   │   │       └── [id]
│   │   │   │   ├── salesitems
│   │   │   │   │   └── [salesitem]
│   │   │   │   │       └── route.ts
│   │   │   │   └── data.json
│   │   │   ├── auth
│   │   │   │   ├── callback
│   │   │   │   │   └── route.ts
│   │   │   │   ├── login
│   │   │   │   │   └── route.ts
│   │   │   │   ├── logout
│   │   │   │   │   └── route.ts
│   │   │   │   └── page.tsx
│   │   │   ├── cart
│   │   │   │   ├── add
│   │   │   │   │   └── route.ts
│   │   │   │   └── page.tsx
│   │   │   ├── checkout
│   │   │   │   ├── submit
│   │   │   │   │   └── route.ts
│   │   │   │   └── page.tsx
│   │   │   ├── dashboard
│   │   │   │   └── page.tsx
│   │   │   ├── fluentuisamplepages
│   │   │   │   └── page.tsx
│   │   │   ├── salesitem
│   │   │   │   ├── [salesitem]
│   │   │   │   │   └── page.tsx
│   │   │   │   └── page.tsx
│   │   │   ├── server
│   │   │   │   └── salesitems.ts
│   │   │   ├── user
│   │   │   │   └── orders
│   │   │   │       └── page.tsx
│   │   │   ├── fluentuisamplepages
│   │   │   ├── globals.css
│   │   │   ├── instrumentation.ts
│   │   │   ├── layout.tsx
│   │   │   ├── page.tsx
│   │   │   ├── server.ts
│   │   │   └── types.ts
│   │   ├── BlazeNexJ.esproj
│   │   ├── CHANGELOG.md
│   │   ├── eslint.config.js
│   │   ├── next-env.d.ts
│   │   ├── package-lock.json
│   │   ├── package.json
│   │   ├── postcss.config.mjs
│   │   ├── tsconfig.json
│   │   ├── tsconfig.server.json
│   │   └── tsconfig.tsbuildinfo
│   ├── BlazeWeb
│   │   ├── Components
│   │   │   └── Layout
│   │   │       ├── CartMenu.razor
│   │   │       ├── CartMenu.razor.css
│   │   │       ├── FooterBar.razor
│   │   │       ├── FooterBar.razor.css
│   │   │       ├── HeaderBar.razor
│   │   │       ├── HeaderBar.razor.css
│   │   │       ├── MainLayout.razor
│   │   │       ├── MainLayout.razor.css
│   │   │       ├── NavMenu.razor
│   │   │       ├── NavMenu.razor.css
│   │   │       ├── ReconnectModal.razor
│   │   │       ├── ReconnectModal.razor.css
│   │   │       ├── ReconnectModal.razor.js
│   │   │       ├── UserMenu.razor
│   │   │       └── UserMenu.razor.css
│   │   ├── Pages
│   │   │   ├── Cart
│   │   │   │   ├── CartPage.razor
│   │   │   │   └── CartPage.razor.css
│   │   │   ├── Catalog
│   │   │   │   ├── Catalog.razor
│   │   │   │   └── Catalog.razor.css
│   │   │   ├── Checkout
│   │   │   │   ├── Checkout.razor
│   │   │   │   └── Checkout.razor.css
│   │   │   ├── Item
│   │   │   │   ├── ItemPage.razor
│   │   │   │   └── ItemPage.razor.css
│   │   │   ├── User
│   │   │   │   ├── LogIn.razor
│   │   │   │   ├── LogOut.razor
│   │   │   │   ├── Orders.razor
│   │   │   │   ├── Orders.razor.css
│   │   │   │   └── OrdersRefreshOnStatusChange.razor
│   │   │   ├── Counter.razor
│   │   │   ├── Error.razor
│   │   │   ├── Home.razor
│   │   │   ├── NotFound.razor
│   │   │   └── Weather.razor
│   │   ├── _Imports.razor
│   │   ├── App.razor
│   │   ├── Routes.razor
│   │   ├── Extensions
│   │   ├── Properties
│   │   ├── Services
│   │   ├── wwwroot
│   │   ├── appsettings.Development.json
│   │   ├── appsettings.json
│   │   ├── BlazeWeb.csproj
│   │   ├── BlazeWeb.csproj.user
│   │   ├── GlobalUsings.cs
│   │   └── Program.cs
│   ├── BlazeWebComponents
│   │   ├── Catalog
│   │   │   ├── CatalogItem.cs
│   │   │   ├── CatalogListItem.razor
│   │   │   ├── CatalogListItem.razor.css
│   │   │   ├── CatalogSearch.razor
│   │   │   └── CatalogSearch.razor.css
│   │   ├── Item
│   │   │   └── ItemHelper.cs
│   │   ├── Services
│   │   │   ├── CatalogService.cs
│   │   │   ├── ICatalogService.cs
│   │   │   └── IProductImageUrlProvider.cs
│   │   ├── _Imports.razor
│   │   └── BlazeWebComponents.csproj
│   ├── Consolidation.API
│   ├── EventBus
│   ├── EventBusRabbitMQ
│   ├── ICLAco.AppHost
│   │   ├── Properties
│   │   ├── appsettings.Development.json
│   │   ├── appsettings.json
│   │   ├── Extensions.cs
│   │   ├── ICLAco.AppHost.csproj
│   │   └── Program.cs
│   ├── ICLAco.ServiceDefaults
│   │   ├── AuthenticationExtensions.cs
│   │   ├── ConfigurationExtensions.cs
│   │   ├── Extensions.cs
│   │   ├── HttpClientExtensions.cs
│   │   ├── OpenApi.Extensions.cs
│   │   ├── OpenApiOptionsExtensions.cs
│   │   └── ICLAco.ServiceDefaults.csproj
│   ├── Identity.API
│   │   ├── Configuration
│   │   │   └── Config.cs
│   │   ├── Data
│   │   │   └── ApplicationDbContext.cs
│   │   ├── Models
│   │   │   ├── AccountViewModels
│   │   │   │   ├── ForgotPasswordViewModel.cs
│   │   │   │   ├── LoggedOutViewModel.cs
│   │   │   │   ├── LoginViewModel.cs
│   │   │   │   ├── LogoutViewModel.cs
│   │   │   │   ├── RedirectViewModel.cs
│   │   │   │   ├── RegisterViewModel.cs
│   │   │   │   ├── ResetPasswordViewModel.cs
│   │   │   │   ├── SendCodeViewModel.cs
│   │   │   │   └── VerifyCodeViewModel.cs
│   │   │   ├── ConsentViewModels
│   │   │   │   ├── ConsentInputModel.cs
│   │   │   │   ├── ConsentOptions.cs
│   │   │   │   ├── ConsentViewModel.cs
│   │   │   │   ├── ProcessConsentResult.cs
│   │   │   │   └── ScopeViewModel.cs
│   │   │   ├── ManageViewModels
│   │   │   │   ├── AddPhoneNumberViewModel.cs
│   │   │   │   ├── ChangePasswordViewModel.cs
│   │   │   │   ├── ConfigureTwoFactorViewModel.cs
│   │   │   │   ├── FactorViewModel.cs
│   │   │   │   ├── IndexViewModel.cs
│   │   │   │   ├── SetPasswordViewModel.cs
│   │   │   │   └── VerifyPhoneNumberViewModel.cs
│   │   │   ├── ApplicationUser.cs
│   │   │   └── ErrorViewModel.cs
│   │   ├── Properties
│   │   │   └── launchSettings.json
│   │   ├── Quickstart
│   │   │   ├── Account
│   │   │   │   ├── AccountController.cs
│   │   │   │   ├── AccountOptions.cs
│   │   │   │   ├── ExternalController.cs
│   │   │   │   ├── ExternalProvider.cs
│   │   │   │   ├── LoggedOutViewModel.cs
│   │   │   │   ├── LoginInputModel.cs
│   │   │   │   ├── LoginViewModel.cs
│   │   │   │   ├── LogoutInputModel.cs
│   │   │   │   ├── LogoutViewModel.cs
│   │   │   │   └── RedirectViewModel.cs
│   │   │   ├── Consent
│   │   │   │   ├── ConsentController.cs
│   │   │   │   ├── ConsentInputModel.cs
│   │   │   │   ├── ConsentOptions.cs
│   │   │   │   ├── ConsentViewModel.cs
│   │   │   │   ├── ProcessConsentResult.cs
│   │   │   │   └── ScopeViewModel.cs
│   │   │   ├── Device
│   │   │   │   ├── DeviceAuthorizationInputModel.cs
│   │   │   │   ├── DeviceAuthorizationViewModel.cs
│   │   │   │   └── DeviceController.cs
│   │   │   ├── Diagnostics
│   │   │   │   ├── DiagnosticsController.cs
│   │   │   │   └── DiagnosticsViewModel.cs
│   │   │   ├── Grants
│   │   │   │   ├── GrantsController.cs
│   │   │   │   └── GrantsViewModel.cs
│   │   │   ├── Home
│   │   │   │   ├── ErrorViewModel.cs
│   │   │   │   └── HomeController.cs
│   │   │   ├── Extensions.cs
│   │   │   └── SecurityHeadersAttribute.cs
│   │   ├── Services
│   │   │   ├── EFLoginService.cs
│   │   │   ├── ILoginService.cs
│   │   │   ├── IRedirectService.cs
│   │   │   ├── ProfileService.cs
│   │   │   └── RedirectService.cs
│   │   ├── Views
│   │   │   ├── Account
│   │   │   │   ├── AccessDenied.cshtml
│   │   │   │   ├── LoggedOut.cshtml
│   │   │   │   ├── Login.cshtml
│   │   │   │   └── Logout.cshtml
│   │   │   ├── Consent
│   │   │   │   └── Index.cshtml
│   │   │   ├── Device
│   │   │   │   ├── Success.cshtml
│   │   │   │   ├── UserCodeCapture.cshtml
│   │   │   │   └── UserCodeConfirmation.cshtml
│   │   │   ├── Diagnostics
│   │   │   │   └── Index.cshtml
│   │   │   ├── Grants
│   │   │   │   └── Index.cshtml
│   │   │   ├── Home
│   │   │   │   └── Index.cshtml
│   │   │   ├── Shared
│   │   │   │   ├── Error.cshtml
│   │   │   │   ├── Redirect.cshtml
│   │   │   │   ├── _Layout.cshtml
│   │   │   │   ├── _ScopeListItem.cshtml
│   │   │   │   └── _ValidationSummary.cshtml
│   │   │   ├── _ViewImports.cshtml
│   │   │   └── _ViewStart.cshtml
│   │   ├── Migrations
│   │   ├── wwwroot
│   │   ├── appsettings.Development.json
│   │   ├── appsettings.json
│   │   ├── bundleconfig.json
│   │   ├── GlobalUsings.cs
│   │   ├── Identity.API.csproj
│   │   ├── Identity.API.csproj.user
│   │   ├── Identity.API.http
│   │   ├── libman.json
│   │   ├── Program.cs
│   │   ├── tempkey.jwk
│   │   └── UsersSeed.cs
│   ├── IntegrationEventLogEF
│   ├── Ordering.API
│   ├── Ordering.Domain
│   ├── Ordering.Infrastructure
│   ├── OrderProcessor
│   ├── PaymentProcessor
│   ├── SalesItem.API
│   │   ├── Apis
│   │   │   └── CatalogApi.cs
│   │   ├── Extensions
│   │   │   └── Extensions.cs
│   │   ├── Infrastructure
│   │   │   ├── EntityConfigurations
│   │   │   │   ├── CatalogBrandEntityTypeConfiguration.cs
│   │   │   │   ├── CatalogItemEntityTypeConfiguration.cs
│   │   │   │   └── CatalogTypeEntityTypeConfiguration.cs
│   │   │   ├── Exceptions
│   │   │   │   └── CatalogDomainException.cs
│   │   │   ├── CatalogContext.cs
│   │   │   └── CatalogContextSeed.cs
│   │   ├── Model
│   │   │   ├── CatalogBrand.cs
│   │   │   ├── CatalogItem.cs
│   │   │   ├── CatalogServices.cs
│   │   │   ├── CatalogType.cs
│   │   │   ├── PaginatedItems.cs
│   │   │   └── PaginationRequest.cs
│   │   ├── Properties
│   │   │   └── launchSettings.json
│   │   ├── Setup
│   │   │   └── catalog.json
│   │   ├── Migrations
│   │   ├── Pics
│   │   ├── appsettings.Development.json
│   │   ├── appsettings.json
│   │   ├── Catalog.API.csproj
│   │   ├── Catalog.API.csproj.user
│   │   ├── Catalog.API.http
│   │   ├── CatalogOptions.cs
│   │   ├── GlobalUsings.cs
│   │   └── Program.cs
│   ├── Shared
│   │   ├── ActivityExtensions.cs
│   │   └── MigrateDbContextExtensions.cs
│   ├── WebhookClient
│   │   ├── Components
│   │   │   ├── Layout
│   │   │   │   ├── MainLayout.razor
│   │   │   │   ├── MainLayout.razor.css
│   │   │   │   ├── UserMenu.razor
│   │   │   │   └── UserMenu.razor.css
│   │   │   ├── Pages
│   │   │   │   ├── Home
│   │   │   │   │   ├── Home.razor
│   │   │   │   │   ├── Home.razor.css
│   │   │   │   │   ├── ReceivedMessages.razor
│   │   │   │   │   └── RegisteredHooks.razor
│   │   │   │   ├── AddWebhook.razor
│   │   │   │   ├── Error.razor
│   │   │   │   └── LogIn.razor
│   │   │   ├── App.razor
│   │   │   ├── App.razor.css
│   │   │   ├── Routes.razor
│   │   │   └── _Imports.razor
│   │   ├── Endpoints
│   │   │   ├── AuthenticationEndpoints.cs
│   │   │   └── WebhookEndpoints.cs
│   │   ├── Extensions
│   │   │   └── Extensions.cs
│   │   ├── Properties
│   │   │   └── launchSettings.json
│   │   ├── Services
│   │   │   ├── HooksRepository.cs
│   │   │   ├── WebhookClientOptions.cs
│   │   │   ├── WebhookData.cs
│   │   │   ├── WebHookReceived.cs
│   │   │   ├── WebhookResponse.cs
│   │   │   ├── WebHooksClient.cs
│   │   │   ├── WebhookSubscriptionRequest.cs
│   │   │   └── WebhookType.cs
│   │   ├── wwwroot
│   │   │   └── app.css
│   │   ├── appsettings.Development.json
│   │   ├── appsettings.json
│   │   ├── GlobalUsings.cs
│   │   ├── Program.cs
│   │   ├── WebhookClient.csproj
│   │   └── WebhookClient.csproj.user
│   └── Webhooks.API
│       ├── Apis
│       │   └── WebHooksApi.cs
│       ├── Exceptions
│       │   └── WebhooksDomainException.cs
│       ├── Extensions
│       │   ├── ClaimsPrincipalExtensions.cs
│       │   ├── Extensions.cs
│       │   └── RouteHandlerBuilderExtensions.cs
│       ├── Infrastructure
│       │   └── WebhooksContext.cs
│       ├── IntegrationEvents
│       │   ├── OrderStatusChangedToPaidIntegrationEvent.cs
│       │   ├── OrderStatusChangedToPaidIntegrationEventHandler.cs
│       │   ├── OrderStatusChangedToShippedIntegrationEvent.cs
│       │   ├── OrderStatusChangedToShippedIntegrationEventHandler.cs
│       │   ├── OrderStockItem.cs
│       │   ├── ProductPriceChangedIntegrationEvent.cs
│       │   └── ProductPriceChangedIntegrationEventHandler.cs
│       ├── Migrations
│       ├── Model
│       │   ├── WebhookData.cs
│       │   ├── WebhookSubscription.cs
│       │   ├── WebhookSubscriptionRequest.cs
│       │   └── WebhookType.cs
│       ├── Properties
│       │   └── launchSettings.json
│       ├── Services
│       │   ├── GrantUrlTesterService.cs
│       │   ├── IGrantUrlTesterService.cs
│       │   ├── IWebhooksRetriever.cs
│       │   ├── IWebhooksSender.cs
│       │   ├── WebhooksRetriever.cs
│       │   └── WebhooksSender.cs
│       ├── appsettings.Development.json
│       ├── appsettings.json
│       ├── GlobalUsings.cs
│       ├── Program.cs
│       └── Webhooks.API.csproj
├── specs
│   ├── 001-aspire-bootstrap
│   │   ├── checklists
│   │   │   └── requirements.md
│   │   ├── contracts
│   │   ├── plan.md
│   │   └── spec.md
│   ├── 002-consolidated-commerce-foundation
│   │   ├── checklists
│   │   │   └── requirements.md
│   │   ├── approval-summary.md
│   │   ├── authorization-status.md
│   │   ├── dba-review.md
│   │   ├── plan.md
│   │   ├── research.md
│   │   ├── spec.md
│   │   └── tasks.md
│   ├── 003-blazenexj-helene-parity
│   │   ├── checklists
│   │   │   └── requirements.md
│   │   ├── exceptions.md
│   │   ├── plan.md
│   │   ├── research.md
│   │   ├── spec.md
│   │   └── tasks.md
│   └── 004-gilgamesh-mobile-first-parity
│       ├── checklists
│       │   └── requirements.md
│       ├── completion-summary.md
│       ├── plan.md
│       ├── research.md
│       ├── spec.md
│       └── tasks.md
└── tests
    └── ICLAco.Tests
