// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace IdentityServer.Infrastructure;

public static class Config
{
    public static IEnumerable<ApiResource> ApiResources()
    {
        return new List<ApiResource>()
        {
            new ApiResource("resource_gateway")
            {
                Scopes =
                {
                    "gateway_fullpermission"
                }
            },
            new ApiResource(LocalApi.ScopeName)
        };
    }

    public static IEnumerable<IdentityResource> IdentityResources()
    {
        return new List<IdentityResource>
        {
            new IdentityResources.Email(),
            new IdentityResources.Phone(),
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
            new IdentityResource
            {
                Name = "roles",
                DisplayName = "Role(s)",
                Description = "Roles of user",
                UserClaims = new[] { JwtClaimTypes.Role },
            }
        };
    }

    public static IEnumerable<ApiScope> ApiScopes()
    {
        return new List<ApiScope>
        {
            new ApiScope("gateway_fullpermission", "Gateway Permission"),
            new ApiScope(LocalApi.ScopeName)
        };
    }

    public static IEnumerable<Client> Clients()
    {
        return new List<Client>
        {
            new Client
            {
                ClientId = "TokenExchangeClient",
                ClientName = "Token Exchange",
                AccessTokenLifetime = 30 * 24 * 60 * 60,
                AllowedGrantTypes = new[]{ OidcConstants.GrantTypes.TokenExchange },
                ClientSecrets = {new Secret("5d6803f8398f287a0b902314002f331dff9de9a395c70e71b9e7549592dc912b".Sha256())},
                AllowedScopes =
                {
                    LocalApi.ScopeName,
                    StandardScopes.OpenId
                }
            },
            new Client
            {
                ClientId = "IdentityServerClient",
                ClientName = "Identity Server Client",
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                ClientSecrets = { new Secret("ff6059a5a8b37d207e0d48b7866122e86f13cb02e0fd005e1a442044582d9616".Sha256()) },
                AccessTokenLifetime = 60 * 24 * 60 * 60,
                AllowedScopes =
                {
                    LocalApi.ScopeName
                }
            },
            new Client
            {
                ClientId = "UserClient",
                ClientName = "Client for members",
                AllowOfflineAccess = true,
                ClientSecrets = {new Secret("55729ae822f517ec65f85edae49a060d552898e0ce4f82d28820a3e58369178e".Sha256()) },
                AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                AllowedScopes =
                {
                    "roles",
                    "gateway_fullpermission",
                    StandardScopes.Email,
                    StandardScopes.OpenId,
                    StandardScopes.Profile,
                    StandardScopes.OfflineAccess,
                    LocalApi.ScopeName
                },
                AccessTokenLifetime = 1 * 60 * 60,
                RefreshTokenExpiration = TokenExpiration.Sliding,
                AbsoluteRefreshTokenLifetime = (int)(DateTime.Now.AddYears(1) - DateTime.Now).TotalSeconds,
                RefreshTokenUsage = TokenUsage.ReUse
            }
        };
    }
}