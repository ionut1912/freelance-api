FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["Frelance.API.sln", "./"]
COPY ["Frelance.Application/Frelance.Application.csproj", "Frelance.Application/"]
COPY ["Frelance.Contracts/Frelance.Contracts.csproj", "Frelance.Contracts/"]
COPY ["Frelance.Infrastructure/Frelance.Infrastructure.csproj", "Frelance.Infrastructure/"]
COPY ["Frelance.Web/Frelance.Web.csproj", "Frelance.Web/"]

RUN dotnet restore "Frelance.Web/Frelance.Web.csproj"

COPY . .
RUN dotnet publish "Frelance.Web/Frelance.Web.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENV AZURE_AUTHORITY_HOST="https://login.microsoftonline.com/"
ENV AZURE_IDENTITY_DISABLE_IMDS=0

EXPOSE 80
ENTRYPOINT ["dotnet", "Frelance.Web.dll"]
