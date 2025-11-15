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
COPY Freelance.Identity/src/Freelance.Identity.Api/Freelance.Identity.Api.csproj Freelance.Identity/src/Freelance.Identity.Api/
COPY Freelance.Identity/src/Freelance.Identity.Infrastructure/Freelance.Identity.Infrastructure.csproj Freelance.Identity/src/Freelance.Identity.Infrastructure/
COPY Freelance.Identity/src/Freelance.Identity.Domain/Freelance.Identity.Domain.csproj Freelance.Identity/src/Freelance.Identity.Domain/
COPY Freelance.Identity/src/Freelance.Identity.Application/Freelance.Identity.Application.csproj Freelance.Identity/src/Freelance.Identity.Application/

# Restore dependencies
RUN dotnet restore Freelance.Identity/src/Freelance.Identity.Api/Freelance.Identity.Api.csproj

# Copy full source code
COPY Freelance.Identity/src/ Freelance.Identity/src/ 

# Build the project
WORKDIR /src/Freelance.Identity/src
RUN dotnet build Freelance.Identity.Api/Freelance.Identity.Api.csproj -c $BUILD_CONFIGURATION -o /app/build

# Publish stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish Freelance.Identity.Api/Freelance.Identity.Api.csproj -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final runtime image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Freelance.Identity.Api.dll"]
