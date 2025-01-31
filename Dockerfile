# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution and projects
COPY ["Frelance.API.sln", "./"]
COPY ["Frelance.Application/Frelance.Application.csproj", "Frelance.Application/"]
COPY ["Frelance.Contracts/Frelance.Contracts.csproj", "Frelance.Contracts/"]
COPY ["Frelance.Infrastructure/Frelance.Infrastructure.csproj", "Frelance.Infrastructure/"]
COPY ["Frelance.Web/Frelance.Web.csproj", "Frelance.Web/"]

# Copy everything and build
COPY . .
RUN dotnet publish "Frelance.Web/Frelance.Web.csproj" -c Release -o /app/publish 

# Runtime Stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

# Expose port 80 explicitly
EXPOSE 80

# Ensure ASP.NET Core listens on the correct port
ENV ASPNETCORE_URLS=http://+:80


# Start the application
ENTRYPOINT ["dotnet", "Frelance.Web.dll"]
