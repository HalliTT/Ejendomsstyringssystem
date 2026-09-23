FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ESS.Api/ESS.Api.csproj ESS.Api/
COPY ESS.Application/ESS.Application.csproj ESS.Application/
COPY ESS.Domain/ESS.Domain.csproj ESS.Domain/
COPY ESS.Infrastructure/ESS.Infrastructure.csproj ESS.Infrastructure/
RUN dotnet restore ESS.Api/ESS.Api.csproj

COPY . .
RUN dotnet publish ESS.Api/ESS.Api.csproj -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build /app/publish ./

EXPOSE 8080
ENTRYPOINT ["dotnet", "ESS.Api.dll"]
