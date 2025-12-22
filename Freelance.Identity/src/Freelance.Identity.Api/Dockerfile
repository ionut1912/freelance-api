FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY nuget.config ./
COPY src/Freelance.Identity.Api/Freelance.Identity.Api.csproj src/Freelance.Identity.Api/
COPY src/Freelance.Identity.Application/Freelance.Identity.Application.csproj src/Freelance.Identity.Application/
COPY src/Freelance.Identity.Domain/Freelance.Identity.Domain.csproj src/Freelance.Identity.Domain/
COPY src/Freelance.Identity.Infrastructure/Freelance.Identity.Infrastructure.csproj src/Freelance.Identity.Infrastructure/
RUN dotnet restore src/Freelance.Identity.Api/Freelance.Identity.Api.csproj
COPY . .
WORKDIR /src/src/Freelance.Identity.Api
RUN dotnet build Freelance.Identity.Api.csproj -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish Freelance.Identity.Api.csproj -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Freelance.Identity.Api.dll"]
