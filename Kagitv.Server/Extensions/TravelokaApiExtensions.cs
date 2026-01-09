//using AiclaRM.Server.Services.Tourney.Employee;
//using AiclaRM.Server.Services.Tourney.Person;
//using AiclaRM.Server.Services.Tourney.Wisata;
//using AIRMDataManager.Library.Modules.Tourney.Employee.Models;
//using AIRMDataManager.Library.Modules.Tourney.Person.Models;
//using AIRMDataManager.Library.Modules.Tourney.Wisata.Models;
using AIRMDataManager.Library.Modules.Traveloka.Wisata.Models;
using Kagitv.Server.Services.Traveloka.Wisata;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AiclaRM.Server.Extensions
{
    public static class TravelokaApiExtensions
    {
        public static void MapTravelokaAPI(this WebApplication app)
        {
            var tourneyApi = app.MapGroup("/api/v1/traveloka");

            //
            // WISATA ENDPOINTS
            //
            tourneyApi.MapGet("/wisatas", async ([FromServices] IWisataService wisataService) =>
                Results.Ok(await wisataService.GetAllWisatasAsync()))
            .WithName("GetAllWisatas")
            .WithTags("TNM :  Wisata");

            tourneyApi.MapGet("/wisatas/{wisataid:int}", async (
                    [FromServices] IWisataService wisataService,
                    [FromRoute] int wisataid) =>
            {
                var wisata = await wisataService.GetWisataByIdAsync(wisataid);
                return wisata is not null
                    ? Results.Ok(wisata)
                    : Results.NotFound();
            })
            .WithName("GetWisataById")
            .WithTags("TNM :  Wisata");

            tourneyApi.MapPost("/wisatas", async (
                    [FromServices] IWisataService wisataService,
                    [FromBody] Wisata wisata) =>
                Results.Ok(await wisataService.InsertWisataAsync(wisata)))
            .WithName("CreateWisata")
            .WithTags("TNM :  Wisata");

            tourneyApi.MapPut("/wisatas/{wisataid:int}", async (
                    [FromServices] IWisataService wisataService,
                    [FromBody] Wisata wisata,
                    [FromRoute] int wisataid) =>
            {
                if (wisata.WisataID != wisataid)
                    return Results.BadRequest("Wisata ID mismatch.");
                return Results.Ok(await wisataService.UpdateWisataAsync(wisata));
            })
            .WithName("UpdateWisata")
            .WithTags("TNM :  Wisata");

            tourneyApi.MapDelete("/wisatas/{id:int}", async (
                    [FromServices] IWisataService wisataService,
                    [FromRoute] int id) =>
            {
                var deleted = await wisataService.DeleteWisataAsync(id);
                return deleted
                    ? Results.NoContent()
                    : Results.NotFound();
            })
            .WithName("DeleteWisata")
            .WithTags("TNM :  Wisata");


        }
    }
}
