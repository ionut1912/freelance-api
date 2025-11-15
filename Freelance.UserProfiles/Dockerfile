# Base runtime image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy NuGet config
COPY nuget.config ./

# Copy project files with correct structure
COPY Freelance.UserProfiles/src/Freelance.UserProfiles.Api/Freelance.UserProfiles.Api.csproj Freelance.UserProfiles/src/Freelance.UserProfiles.Api/
COPY Freelance.UserProfiles/src/Freelance.UserProfiles.Application/Freelance.UserProfiles.Application.csproj Freelance.UserProfiles/src/Freelance.UserProfiles.Application/
COPY Freelance.UserProfiles/src/Freelance.UserProfiles.Domain/Freelance.UserProfiles.Domain.csproj Freelance.UserProfiles/src/Freelance.UserProfiles.Domain/
COPY Freelance.UserProfiles/src/Freelance.UserProfiles.Infrastructure/Freelance.UserProfiles.Infrastructure.csproj Freelance.UserProfiles/src/Freelance.UserProfiles.Infrastructure/

# Restore dependencies
RUN dotnet restore Freelance.UserProfiles/src/Freelance.UserProfiles.Api/Freelance.UserProfiles.Api.csproj

# Copy full source code
COPY Freelance.UserProfiles/src/ Freelance.UserProfiles/src/

# Build the project from the root of the repo structure
WORKDIR /src/Freelance.UserProfiles/src
RUN dotnet build Freelance.UserProfiles.Api/Freelance.UserProfiles.Api.csproj -c $BUILD_CONFIGURATION -o /app/build

# Publish stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish Freelance.UserProfiles.Api/Freelance.UserProfiles.Api.csproj -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final runtime image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Freelance.UserProfiles.Api.dll"]
