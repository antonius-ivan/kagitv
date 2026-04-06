Singapore@2025
pg7adiconsulting.co

dotnet ef migrations add InitialCatalog \
  --project src/Catalog.API \
  --startup-project src/Catalog.API \
  --output-dir Migrations


dotnet ef migrations add InitialCatalog `
  --project src/Catalog.API `
  --startup-project src/Catalog.API `
  --output-dir Migrations

dotnet tool install --global dotnet-ef

dotnet ef --version

