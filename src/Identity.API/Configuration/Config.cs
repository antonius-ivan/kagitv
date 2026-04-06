namespace ICLAco.Identity.API.Configuration;

public static class Config
{
    public static readonly string[] IdentityScopes =
    [
        Scopes.OpenId,
        Scopes.Profile,
        Scopes.OfflineAccess
    ];

    public static readonly string[] ApiScopes =
    [
        "orders",
        "basket",
        "webhooks",
        "mobileshoppingagg",
        "webshoppingagg"
    ];
}
