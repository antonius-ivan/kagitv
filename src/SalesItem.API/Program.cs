using SalesItem.API.Extensions;
using ICLAco.ServiceDefaults;
using ICLACo.SalesItem.API;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.AddServiceDefaults();
builder.AddApplicationServices();
builder.Services.AddProblemDetails();
builder.Services.AddApiVersioning();

var app = builder.Build();

//app.UseHttpsRedirection();
app.MapDefaultEndpoints();
app.UseAuthentication();
app.UseAuthorization();

app.UseStatusCodePages();

app.MapCatalogApi();
app.MapWisataApi();
app.Run();
