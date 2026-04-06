using ICLAco.SalesItem.API.Infrastructure.Wisata;
using ICLAco.SalesItem.API.Model.Wisata;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ICLAco.SalesItem.API;

public static class WisataApi
{
    public static IEndpointRouteBuilder MapWisataApi(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/Auth/login", LoginAsync)
            .WithName("WisataLogin")
            .WithSummary("Login wisata")
            .WithTags("WisataAuth");

        app.MapPost("/api/Auth/refresh", RefreshAsync)
            .WithName("WisataRefresh")
            .WithSummary("Refresh token wisata")
            .WithTags("WisataAuth");

        app.MapPost("/api/Auth/logout", LogoutAsync)
            .WithName("WisataLogout")
            .WithSummary("Logout wisata")
            .WithTags("WisataAuth");

        var wisataApi = app.MapGroup("/api/wisata")
            .RequireAuthorization("WisataJwtPolicy")
            .WithTags("Wisata");

        wisataApi.MapGet("/", ListAsync)
            .WithName("ListWisata")
            .WithSummary("List wisata dengan search dan pagination");

        wisataApi.MapGet("/report", GetReportAsync)
            .WithName("WisataReport")
            .WithSummary("Report data wisata");

        wisataApi.MapGet("/{id:int}", GetByIdAsync)
            .WithName("GetWisata")
            .WithSummary("Detail wisata");

        wisataApi.MapPost("/", CreateAsync)
            .WithName("CreateWisata")
            .WithSummary("Tambah wisata");

        wisataApi.MapPut("/{id:int}", UpdateAsync)
            .WithName("UpdateWisata")
            .WithSummary("Ubah wisata");

        wisataApi.MapDelete("/{id:int}", DeleteAsync)
            .WithName("DeleteWisata")
            .WithSummary("Hapus wisata");

        return app;
    }

    private static async Task<Results<Ok<WisataAuthResponse>, UnauthorizedHttpResult, ValidationProblem>> LoginAsync(
        WisataLoginRequest request,
        WisataAuthService authService,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
        {
            return TypedResults.ValidationProblem(new Dictionary<string, string[]>
            {
                ["username"] = ["Username wajib diisi."],
                ["password"] = ["Password wajib diisi."]
            });
        }

        var result = await authService.LoginAsync(request.Username, request.Password, cancellationToken);
        return result is null
            ? TypedResults.Unauthorized()
            : TypedResults.Ok(ToAuthResponse(result));
    }

    private static async Task<Results<Ok<WisataAuthResponse>, UnauthorizedHttpResult>> RefreshAsync(
        WisataRefreshRequest request,
        WisataAuthService authService,
        CancellationToken cancellationToken)
    {
        var result = await authService.RefreshAsync(request.RefreshToken, cancellationToken);
        return result is null
            ? TypedResults.Unauthorized()
            : TypedResults.Ok(ToAuthResponse(result));
    }

    private static async Task<NoContent> LogoutAsync(
        WisataLogoutRequest request,
        WisataAuthService authService,
        CancellationToken cancellationToken)
    {
        await authService.LogoutAsync(request.RefreshToken, cancellationToken);
        return TypedResults.NoContent();
    }

    private static async Task<Ok<WisataListResponse>> ListAsync(
        WisataDbContext dbContext,
        [FromQuery] string? search,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 5,
        CancellationToken cancellationToken = default)
    {
        var safePage = page <= 0 ? 1 : page;
        var safePageSize = pageSize <= 0 ? 5 : Math.Min(pageSize, 50);

        var query = dbContext.Wisata.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var trimmedSearch = search.Trim();
            query = query.Where(item => item.Nama.Contains(trimmedSearch));
        }

        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .OrderBy(item => item.WisataId)
            .Skip((safePage - 1) * safePageSize)
            .Take(safePageSize)
            .Select(item => new WisataDto(item.WisataId, item.Nama, item.Kota, item.Harga, item.CreatedDate))
            .ToListAsync(cancellationToken);

        return TypedResults.Ok(new WisataListResponse(safePage, safePageSize, totalCount, items));
    }

    private static async Task<Results<Ok<WisataDto>, NotFound>> GetByIdAsync(
        int id,
        WisataDbContext dbContext,
        CancellationToken cancellationToken)
    {
        var item = await dbContext.Wisata.AsNoTracking()
            .Where(entry => entry.WisataId == id)
            .Select(entry => new WisataDto(entry.WisataId, entry.Nama, entry.Kota, entry.Harga, entry.CreatedDate))
            .SingleOrDefaultAsync(cancellationToken);

        return item is null ? TypedResults.NotFound() : TypedResults.Ok(item);
    }

    private static async Task<Results<Created<WisataDto>, ValidationProblem>> CreateAsync(
        WisataUpsertRequest request,
        WisataDbContext dbContext,
        CancellationToken cancellationToken)
    {
        var validation = ValidateRequest(request);
        if (validation is not null)
        {
            return TypedResults.ValidationProblem(validation);
        }

        var item = new WisataItem
        {
            Nama = request.Nama.Trim(),
            Kota = request.Kota.Trim(),
            Harga = request.Harga,
            CreatedDate = DateTime.UtcNow
        };

        await dbContext.Wisata.AddAsync(item, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return TypedResults.Created($"/api/wisata/{item.WisataId}", new WisataDto(item.WisataId, item.Nama, item.Kota, item.Harga, item.CreatedDate));
    }

    private static async Task<Results<Ok<WisataDto>, NotFound, ValidationProblem>> UpdateAsync(
        int id,
        WisataUpsertRequest request,
        WisataDbContext dbContext,
        CancellationToken cancellationToken)
    {
        var validation = ValidateRequest(request);
        if (validation is not null)
        {
            return TypedResults.ValidationProblem(validation);
        }

        var item = await dbContext.Wisata.SingleOrDefaultAsync(entry => entry.WisataId == id, cancellationToken);
        if (item is null)
        {
            return TypedResults.NotFound();
        }

        item.Nama = request.Nama.Trim();
        item.Kota = request.Kota.Trim();
        item.Harga = request.Harga;
        await dbContext.SaveChangesAsync(cancellationToken);

        return TypedResults.Ok(new WisataDto(item.WisataId, item.Nama, item.Kota, item.Harga, item.CreatedDate));
    }

    private static async Task<Results<NoContent, NotFound>> DeleteAsync(
        int id,
        WisataDbContext dbContext,
        CancellationToken cancellationToken)
    {
        var item = await dbContext.Wisata.SingleOrDefaultAsync(entry => entry.WisataId == id, cancellationToken);
        if (item is null)
        {
            return TypedResults.NotFound();
        }

        dbContext.Wisata.Remove(item);
        await dbContext.SaveChangesAsync(cancellationToken);
        return TypedResults.NoContent();
    }

    private static async Task<Ok<WisataReportResponse>> GetReportAsync(
        WisataDbContext dbContext,
        CancellationToken cancellationToken)
    {
        var items = await dbContext.Wisata.AsNoTracking()
            .OrderBy(item => item.WisataId)
            .Select(item => new WisataDto(item.WisataId, item.Nama, item.Kota, item.Harga, item.CreatedDate))
            .ToListAsync(cancellationToken);

        return TypedResults.Ok(new WisataReportResponse(
            Items: items,
            TotalHarga: items.Sum(item => item.Harga),
            PrintedAt: DateTime.UtcNow));
    }

    private static Dictionary<string, string[]>? ValidateRequest(WisataUpsertRequest request)
    {
        var errors = new Dictionary<string, string[]>();

        if (string.IsNullOrWhiteSpace(request.Nama))
        {
            errors["nama"] = ["Nama wajib diisi."];
        }

        if (string.IsNullOrWhiteSpace(request.Kota))
        {
            errors["kota"] = ["Kota wajib diisi."];
        }

        if (request.Harga <= 0)
        {
            errors["harga"] = ["Harga harus lebih besar dari 0."];
        }

        return errors.Count == 0 ? null : errors;
    }

    private static WisataAuthResponse ToAuthResponse(WisataTokenResponse response)
    {
        return new WisataAuthResponse(
            AccessToken: response.AccessToken,
            AccessTokenExpiresAt: response.AccessTokenExpiresAt,
            RefreshToken: response.RefreshToken,
            RefreshTokenExpiresAt: response.RefreshTokenExpiresAt,
            Username: response.Username);
    }
}

public sealed record WisataLoginRequest(string Username, string Password);

public sealed record WisataRefreshRequest(string RefreshToken);

public sealed record WisataLogoutRequest(string RefreshToken);

public sealed record WisataAuthResponse(
    string AccessToken,
    DateTime AccessTokenExpiresAt,
    string RefreshToken,
    DateTime RefreshTokenExpiresAt,
    string Username);

public sealed record WisataUpsertRequest(string Nama, string Kota, decimal Harga);

public sealed record WisataDto(int WisataId, string Nama, string Kota, decimal Harga, DateTime CreatedDate);

public sealed record WisataListResponse(int Page, int PageSize, int TotalCount, IReadOnlyList<WisataDto> Items);

public sealed record WisataReportResponse(IReadOnlyList<WisataDto> Items, decimal TotalHarga, DateTime PrintedAt);