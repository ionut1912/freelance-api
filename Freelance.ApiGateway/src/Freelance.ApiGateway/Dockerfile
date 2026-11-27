FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
ARG NUGET_AUTH_TOKEN
WORKDIR /src
COPY ["nuget.config", "Directory.Build.props", "Directory.Packages.props", "./"]
COPY ["src/Freelance.ApiGateway/Freelance.ApiGateway.csproj", "src/Freelance.ApiGateway/"]
RUN dotnet restore "src/Freelance.ApiGateway/Freelance.ApiGateway.csproj"
COPY . .
WORKDIR /src/src/Freelance.ApiGateway
RUN dotnet build "Freelance.ApiGateway.csproj" -c $BUILD_CONFIGURATION -o /app/build --no-restore

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Freelance.ApiGateway.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false --no-restore

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Freelance.ApiGateway.dll"]
