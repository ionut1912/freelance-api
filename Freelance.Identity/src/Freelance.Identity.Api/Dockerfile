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
COPY ["src/Freelance.Identity.Api/Freelance.Identity.Api.csproj", "src/Freelance.Identity.Api/"]
COPY ["src/Freelance.Identity.Application/Freelance.Identity.Application.csproj", "src/Freelance.Identity.Application/"]
COPY ["src/Freelance.Identity.Domain/Freelance.Identity.Domain.csproj", "src/Freelance.Identity.Domain/"]
COPY ["src/Freelance.Identity.Infrastructure/Freelance.Identity.Infrastructure.csproj", "src/Freelance.Identity.Infrastructure/"]
RUN dotnet restore "./src/Freelance.Identity.Api/Freelance.Identity.Api.csproj"
COPY . .
WORKDIR "/src/src/Freelance.Identity.Api"
RUN dotnet build "./Freelance.Identity.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build --no-restore

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Freelance.Identity.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false --no-restore

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Freelance.Identity.Api.dll"]
