# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
ARG NUGET_AUTH_TOKEN
WORKDIR /src
COPY ["nuget.config", "Directory.Build.props", "Directory.Packages.props", "./"]
COPY ["src/Freelance.UserProfiles.Api/Freelance.UserProfiles.Api.csproj", "src/Freelance.UserProfiles.Api/"]
COPY ["src/Freelance.UserProfiles.Application/Freelance.UserProfiles.Application.csproj", "src/Freelance.UserProfiles.Application/"]
COPY ["src/Freelance.UserProfiles.Domain/Freelance.UserProfiles.Domain.csproj", "src/Freelance.UserProfiles.Domain/"]
COPY ["src/Freelance.UserProfiles.Infrastructure/Freelance.UserProfiles.Infrastructure.csproj", "src/Freelance.UserProfiles.Infrastructure/"]
RUN dotnet restore "./src/Freelance.UserProfiles.Api/Freelance.UserProfiles.Api.csproj"
COPY . .
WORKDIR "/src/src/Freelance.UserProfiles.Api"
RUN dotnet build "./Freelance.UserProfiles.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build --no-restore

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Freelance.UserProfiles.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false --no-restore

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Freelance.UserProfiles.Api.dll"]
