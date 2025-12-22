FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY nuget.config ./
COPY src/Freelance.UserProfiles.Api/Freelance.UserProfiles.Api.csproj src/Freelance.UserProfiles.Api/
RUN dotnet restore src/Freelance.UserProfiles.Api/Freelance.UserProfiles.Api.csproj
COPY . .
WORKDIR /src/src/Freelance.UserProfiles.Api
RUN dotnet build Freelance.UserProfiles.Api.csproj -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish Freelance.UserProfiles.Api.csproj -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Freelance.UserProfiles.Api.dll"]
