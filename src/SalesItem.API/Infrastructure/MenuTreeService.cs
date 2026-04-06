using System.Security.Claims;

namespace ICLAco.SalesItem.API.Infrastructure;

public interface IMenuTreeService
{
    Task<IReadOnlyList<MenuModuleDto>> GetMenuAsync(ClaimsPrincipal user, CancellationToken cancellationToken);
}

public sealed class MenuTreeService(SalesItemContext context) : IMenuTreeService
{
    public async Task<IReadOnlyList<MenuModuleDto>> GetMenuAsync(ClaimsPrincipal user, CancellationToken cancellationToken)
    {
        var roleNames = user.Claims
            .Where(claim => claim.Type is ClaimTypes.Role or "role")
            .Select(claim => claim.Value)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        if (roleNames.Count == 0)
        {
            var userId = user.Claims
                .FirstOrDefault(claim => claim.Type is ClaimTypes.NameIdentifier or "sub")
                ?.Value;

            if (string.IsNullOrWhiteSpace(userId) is false)
            {
                var resolvedRoleNames = await context.Database.SqlQuery<string>($"""
                    select r.name as value
                    from identity.asp_net_user_role ur
                    inner join identity.asp_net_role r on r.id = ur.role_id
                    where ur.user_id = {userId}
                    """)
                    .ToListAsync(cancellationToken);

                foreach (var roleName in resolvedRoleNames.Where(roleName => string.IsNullOrWhiteSpace(roleName) is false))
                {
                    roleNames.Add(roleName);
                }
            }
        }

        var modules = await context.Modules
            .AsNoTracking()
            .Where(module => module.IsEnabled)
            .OrderBy(module => module.SortOrder)
            .ThenBy(module => module.Name)
            .ToListAsync(cancellationToken);

        if (modules.Count == 0)
        {
            return [];
        }

        var menus = await context.Menus
            .AsNoTracking()
            .Include(menu => menu.MenuRoles)
            .Where(menu => menu.IsEnabled)
            .OrderBy(menu => menu.SortOrder)
            .ThenBy(menu => menu.Name)
            .ToListAsync(cancellationToken);

        if (menus.Count == 0)
        {
            return [];
        }

        var accessibleMenuIds = menus
            .Where(menu => menu.MenuRoles.Count == 0 || menu.MenuRoles.Any(menuRole => roleNames.Contains(menuRole.RoleName)))
            .Select(menu => menu.Id)
            .ToHashSet();

        if (accessibleMenuIds.Count == 0)
        {
            return [];
        }

        var menuById = menus.ToDictionary(menu => menu.Id);
        var expandedMenuIds = new HashSet<int>(accessibleMenuIds);

        foreach (var menu in menus.Where(menu => accessibleMenuIds.Contains(menu.Id)))
        {
            var currentMenu = menu;
            while (currentMenu.ParentMenuId is int parentMenuId && menuById.TryGetValue(parentMenuId, out var parentMenu))
            {
                expandedMenuIds.Add(parentMenu.Id);
                currentMenu = parentMenu;
            }
        }

        var visibleMenus = menus
            .Where(menu => expandedMenuIds.Contains(menu.Id))
            .ToList();

        var childrenByParentId = visibleMenus
            .ToLookup(menu => menu.ParentMenuId);

        MenuNodeDto BuildNode(Menu menu)
        {
            var children = childrenByParentId[menu.Id]
                .OrderBy(childMenu => childMenu.SortOrder)
                .ThenBy(childMenu => childMenu.Name)
                .Select(BuildNode)
                .ToArray();

            return new MenuNodeDto(menu.Id, menu.Name, menu.Path, menu.Icon, children);
        }

        var menuModules = new List<MenuModuleDto>();
        foreach (var module in modules)
        {
            var rootMenus = visibleMenus
                .Where(menu => menu.ModuleId == module.Id && menu.ParentMenuId is null)
                .OrderBy(menu => menu.SortOrder)
                .ThenBy(menu => menu.Name)
                .Select(BuildNode)
                .ToArray();

            if (rootMenus.Length == 0)
            {
                continue;
            }

            menuModules.Add(new MenuModuleDto(module.Code, module.Name, rootMenus));
        }

        return AppendWisataModule(menuModules);
    }

    private static IReadOnlyList<MenuModuleDto> AppendWisataModule(List<MenuModuleDto> menuModules)
    {
        if (menuModules.Any(module =>
                string.Equals(module.Code, "WISATA", StringComparison.OrdinalIgnoreCase) ||
                module.Children.Any(NodeContainsWisataRoute)))
        {
            return menuModules;
        }

        return
        [
            .. menuModules,
            new MenuModuleDto(
                "WISATA",
                "Wisata",
                [
                    new MenuNodeDto(-201, "List Wisata", "/wisata", "map", []),
                    new MenuNodeDto(-202, "Tambah Wisata", "/wisata/tambah", "add", []),
                    new MenuNodeDto(-203, "Laporan", "/wisata/laporan", "chart", [])
                ])
        ];
    }

    private static bool NodeContainsWisataRoute(MenuNodeDto node)
    {
        if (node.Path?.StartsWith("/wisata", StringComparison.OrdinalIgnoreCase) is true)
        {
            return true;
        }

        return node.Children.Any(NodeContainsWisataRoute);
    }
}

public sealed record MenuModuleDto(string Code, string Name, IReadOnlyList<MenuNodeDto> Children);

public sealed record MenuNodeDto(int Id, string Name, string? Path, string? Icon, IReadOnlyList<MenuNodeDto> Children);