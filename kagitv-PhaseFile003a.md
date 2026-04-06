Current Status
├── .gitattributes
├── .gitignore
├── Directory.Packages.props (Solution Files)
└── src
    ├── Basket.API
    │   ├── Extensions
    │   │   ├── Extensions.cs
    │   │   └── ServerCallContextIdentityExtensions.cs
    │   ├── Grpc
    │   │   └── BasketService.cs
    │   ├── Model
    │   │   ├── BasketItem.cs
    │   │   └── CustomerBasket.cs
    │   ├── Properties
    │   │   └── launchSettings.json
    │   ├── Protos
    │   │   └── basket.proto
    │   ├── Repositories
    │   │   ├── IBasketRepository.cs
    │   │   └── RedisBasketRepository.cs
    │   ├── appsettings.Development.json
    │   ├── appsettings.json
    │   ├── Basket.API.csproj
    │   ├── Basket.API.csproj.user
    │   ├── GlobalUsings.cs
    │   └── Program.cs
    ├── BlazeNexJ
    ├── BlazeWeb
    │   ├── Components
    │   │   └── Layout
    │   │       ├── CartMenu.razor
    │   │       ├── CartMenu.razor.css
    │   │       ├── FooterBar.razor
    │   │       ├── FooterBar.razor.css
    │   │       ├── HeaderBar.razor
    │   │       ├── HeaderBar.razor.css
    │   │       ├── MainLayout.razor
    │   │       ├── MainLayout.razor.css
    │   │       ├── NavMenu.razor
    │   │       ├── NavMenu.razor.css
    │   │       ├── ReconnectModal.razor
    │   │       ├── ReconnectModal.razor.css
    │   │       ├── ReconnectModal.razor.js
    │   │       ├── UserMenu.razor
    │   │       └── UserMenu.razor.css
    │   ├── Pages
    │   │   ├── Cart
    │   │   │   ├── CartPage.razor
    │   │   │   └── CartPage.razor.css
    │   │   ├── Catalog
    │   │   │   ├── Catalog.razor
    │   │   │   └── Catalog.razor.css
    │   │   ├── Checkout
    │   │   │   ├── Checkout.razor
    │   │   │   └── Checkout.razor.css
    │   │   ├── Item
    │   │   │   ├── ItemPage.razor
    │   │   │   └── ItemPage.razor.css
    │   │   ├── User
    │   │   │   ├── LogIn.razor
    │   │   │   ├── LogOut.razor
    │   │   │   ├── Orders.razor
    │   │   │   ├── Orders.razor.css
    │   │   │   └── OrdersRefreshOnStatusChange.razor
    │   │   ├── Counter.razor
    │   │   ├── Error.razor
    │   │   ├── Home.razor
    │   │   ├── NotFound.razor
    │   │   └── Weather.razor
    │   ├── _Imports.razor
    │   ├── App.razor
    │   ├── Routes.razor
    │   ├── Extensions
    │   ├── Properties
    │   ├── Services
    │   ├── wwwroot
    │   ├── appsettings.Development.json
    │   ├── appsettings.json
    │   ├── BlazeWeb.csproj
    │   ├── BlazeWeb.csproj.user
    │   ├── GlobalUsings.cs
    │   └── Program.cs
    ├── BlazeWebComponents
    │   ├── Catalog
    │   │   ├── CatalogItem.cs
    │   │   ├── CatalogListItem.razor
    │   │   ├── CatalogListItem.razor.css
    │   │   ├── CatalogSearch.razor
    │   │   └── CatalogSearch.razor.css
    │   ├── Item
    │   │   └── ItemHelper.cs
    │   ├── Services
    │   │   ├── CatalogService.cs
    │   │   ├── ICatalogService.cs
    │   │   └── IProductImageUrlProvider.cs
    │   ├── _Imports.razor
    │   └── BlazeWebComponents.csproj
    ├── Consolidation.API
    ├── EventBus
    ├── EventBusRabbitMQ
    ├── ICLAco.AppHost
    │   ├── Properties
    │   ├── appsettings.Development.json
    │   ├── appsettings.json
    │   ├── Extensions.cs
    │   ├── ICLAco.AppHost.csproj
    │   └── Program.cs
    ├── ICLAco.ServiceDefaults
    │   ├── AuthenticationExtensions.cs
    │   ├── ConfigurationExtensions.cs
    │   ├── Extensions.cs
    │   ├── HttpClientExtensions.cs
    │   ├── OpenApi.Extensions.cs
    │   ├── OpenApiOptionsExtensions.cs
    │   └── ICLAco.ServiceDefaults.csproj
    ├── Identity.API
    │   ├── Configuration
    │   │   └── Config.cs
    │   ├── Data
    │   │   └── ApplicationDbContext.cs
    │   ├── Models
    │   │   ├── AccountViewModels
    │   │   │   ├── ForgotPasswordViewModel.cs
    │   │   │   ├── LoggedOutViewModel.cs
    │   │   │   ├── LoginViewModel.cs
    │   │   │   ├── LogoutViewModel.cs
    │   │   │   ├── RedirectViewModel.cs
    │   │   │   ├── RegisterViewModel.cs
    │   │   │   ├── ResetPasswordViewModel.cs
    │   │   │   ├── SendCodeViewModel.cs
    │   │   │   └── VerifyCodeViewModel.cs
    │   │   ├── ConsentViewModels
    │   │   │   ├── ConsentInputModel.cs
    │   │   │   ├── ConsentOptions.cs
    │   │   │   ├── ConsentViewModel.cs
    │   │   │   ├── ProcessConsentResult.cs
    │   │   │   └── ScopeViewModel.cs
    │   │   ├── ManageViewModels
    │   │   │   ├── AddPhoneNumberViewModel.cs
    │   │   │   ├── ChangePasswordViewModel.cs
    │   │   │   ├── ConfigureTwoFactorViewModel.cs
    │   │   │   ├── FactorViewModel.cs
    │   │   │   ├── IndexViewModel.cs
    │   │   │   ├── SetPasswordViewModel.cs
    │   │   │   └── VerifyPhoneNumberViewModel.cs
    │   │   ├── ApplicationUser.cs
    │   │   └── ErrorViewModel.cs
    │   ├── Properties
    │   │   └── launchSettings.json
    │   ├── Quickstart
    │   │   ├── Account
    │   │   │   ├── AccountController.cs
    │   │   │   ├── AccountOptions.cs
    │   │   │   ├── ExternalController.cs
    │   │   │   ├── ExternalProvider.cs
    │   │   │   ├── LoggedOutViewModel.cs
    │   │   │   ├── LoginInputModel.cs
    │   │   │   ├── LoginViewModel.cs
    │   │   │   ├── LogoutInputModel.cs
    │   │   │   ├── LogoutViewModel.cs
    │   │   │   └── RedirectViewModel.cs
    │   │   ├── Consent
    │   │   │   ├── ConsentController.cs
    │   │   │   ├── ConsentInputModel.cs
    │   │   │   ├── ConsentOptions.cs
    │   │   │   ├── ConsentViewModel.cs
    │   │   │   ├── ProcessConsentResult.cs
    │   │   │   └── ScopeViewModel.cs
    │   │   ├── Device
    │   │   │   ├── DeviceAuthorizationInputModel.cs
    │   │   │   ├── DeviceAuthorizationViewModel.cs
    │   │   │   └── DeviceController.cs
    │   │   ├── Diagnostics
    │   │   │   ├── DiagnosticsController.cs
    │   │   │   └── DiagnosticsViewModel.cs
    │   │   ├── Grants
    │   │   │   ├── GrantsController.cs
    │   │   │   └── GrantsViewModel.cs
    │   │   ├── Home
    │   │   │   ├── ErrorViewModel.cs
    │   │   │   └── HomeController.cs
    │   │   ├── Extensions.cs
    │   │   └── SecurityHeadersAttribute.cs
    │   ├── Services
    │   │   ├── EFLoginService.cs
    │   │   ├── ILoginService.cs
    │   │   ├── IRedirectService.cs
    │   │   ├── ProfileService.cs
    │   │   └── RedirectService.cs
    │   ├── Views
    │   │   ├── Account
    │   │   │   ├── AccessDenied.cshtml
    │   │   │   ├── LoggedOut.cshtml
    │   │   │   ├── Login.cshtml
    │   │   │   └── Logout.cshtml
    │   │   ├── Consent
    │   │   │   └── Index.cshtml
    │   │   ├── Device
    │   │   │   ├── Success.cshtml
    │   │   │   ├── UserCodeCapture.cshtml
    │   │   │   └── UserCodeConfirmation.cshtml
    │   │   ├── Diagnostics
    │   │   │   └── Index.cshtml
    │   │   ├── Grants
    │   │   │   └── Index.cshtml
    │   │   ├── Home
    │   │   │   └── Index.cshtml
    │   │   ├── Shared
    │   │   │   ├── Error.cshtml
    │   │   │   ├── Redirect.cshtml
    │   │   │   ├── _Layout.cshtml
    │   │   │   ├── _ScopeListItem.cshtml
    │   │   │   └── _ValidationSummary.cshtml
    │   │   ├── _ViewImports.cshtml
    │   │   └── _ViewStart.cshtml
    │   ├── Migrations
    │   ├── wwwroot
    │   ├── appsettings.Development.json
    │   ├── appsettings.json
    │   ├── bundleconfig.json
    │   ├── GlobalUsings.cs
    │   ├── Identity.API.csproj
    │   ├── Identity.API.csproj.user
    │   ├── Identity.API.http
    │   ├── libman.json
    │   ├── Program.cs
    │   ├── tempkey.jwk
    │   └── UsersSeed.cs
    ├── IntegrationEventLogEF
    ├── Ordering.API
    ├── Ordering.Domain
    ├── Ordering.Infrastructure
    ├── OrderProcessor
    ├── PaymentProcessor
    ├── SalesItem.API
    │   ├── Apis
    │   │   └── CatalogApi.cs
    │   ├── Extensions
    │   │   └── Extensions.cs
    │   ├── Infrastructure
    │   │   ├── EntityConfigurations
    │   │   │   ├── CatalogBrandEntityTypeConfiguration.cs
    │   │   │   ├── CatalogItemEntityTypeConfiguration.cs
    │   │   │   └── CatalogTypeEntityTypeConfiguration.cs
    │   │   ├── Exceptions
    │   │   │   └── CatalogDomainException.cs
    │   │   ├── CatalogContext.cs
    │   │   └── CatalogContextSeed.cs
    │   ├── Model
    │   │   ├── CatalogBrand.cs
    │   │   ├── CatalogItem.cs
    │   │   ├── CatalogServices.cs
    │   │   ├── CatalogType.cs
    │   │   ├── PaginatedItems.cs
    │   │   └── PaginationRequest.cs
    │   ├── Properties
    │   │   └── launchSettings.json
    │   ├── Setup
    │   │   └── catalog.json
    │   ├── Migrations
    │   ├── Pics
    │   ├── appsettings.Development.json
    │   ├── appsettings.json
    │   ├── Catalog.API.csproj
    │   ├── Catalog.API.csproj.user
    │   ├── Catalog.API.http
    │   ├── CatalogOptions.cs
    │   ├── GlobalUsings.cs
    │   └── Program.cs
    ├── WebhookClient
    ├── Webhooks.API
