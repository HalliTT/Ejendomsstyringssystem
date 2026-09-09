

dotnet ef migrations add CreateOwner --project .\ESS.Infrastructure\ --startup-project .\ESS.API\

dotnet ef database update --project .\ESS.Infrastructure\ --startup-project .\ESS.API\