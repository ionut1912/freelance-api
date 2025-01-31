# Use .NET 8.0 SDK for building
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution and project files (to leverage Docker caching)
COPY Frelance.API.sln ./
COPY Frelance.Application/*.csproj Frelance.Application/
COPY Frelance.Contracts/*.csproj Frelance.Contracts/
COPY Frelance.Infrastructure/*.csproj Frelance.Infrastructure/
COPY Frelance.Web/*.csproj Frelance.Web/

RUN dotnet restore "Frelance.Web/Frelance.Web.csproj"

COPY . .
RUN dotnet publish "Frelance.Web/Frelance.Web.csproj" -c Release -o /app/publish --no-restore
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

# Expose port 80 explicitly
EXPOSE 80

# Ensure ASP.NET Core listens on the correct port
ENV ASPNETCORE_URLS=http://+:80

# Pass Azure Access Token from environment variables
ENV AZURE_ACCESS_TOKEN=${AZURE_ACCESS_TOKEN}

# Use a non-root user for security
RUN adduser --disabled-password --home /app nonroot
USER nonroot

# Start the application
ENTRYPOINT ["dotnet", "Frelance.Web.dll"]