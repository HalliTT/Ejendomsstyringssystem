FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY AuthService.Api/AuthService.Api.csproj AuthService.Api/
COPY AuthService.Application/AuthService.Application.csproj AuthService.Application/
COPY AuthService.Contracts/AuthService.Contracts.csproj AuthService.Contracts/
COPY AuthService.Domain/AuthService.Domain.csproj AuthService.Domain/
COPY AuthService.Infrastructure/AuthService.Infrastructure.csproj AuthService.Infrastructure/
RUN dotnet restore AuthService.Api/AuthService.Api.csproj

COPY . .
RUN dotnet publish AuthService.Api/AuthService.Api.csproj -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build /app/publish ./

EXPOSE 8080
ENTRYPOINT ["dotnet", "AuthService.Api.dll"]
