FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
ARG APP_UID=0
USER root
WORKDIR /app

RUN apt-get update && apt-get install -y \
    libglib2.0-0 \
    libgl1 \
    libsm6 \
    libxrender1 \
    libxext6 \
    libgomp1 \
    libopenblas0 \
    && rm -rf /var/lib/apt/lists/*

USER ${APP_UID}
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["models","./models"]
COPY ["nuget.config", "."]

COPY ["src/Freelance.Face.Api/Freelance.Face.Api.csproj", "src/Freelance.Face.Api/"]
COPY ["src/Freelance.Face.Application/Freelance.Face.Application.csproj", "src/Freelance.Face.Application/"]
COPY ["src/Freelance.Face.Domain/Freelance.Face.Domain.csproj", "src/Freelance.Face.Domain/"]
COPY ["src/Freelance.Face.Infrastructure/Freelance.Face.Infrastructure.csproj", "src/Freelance.Face.Infrastructure/"]

RUN dotnet restore "./src/Freelance.Face.Api/Freelance.Face.Api.csproj"

COPY . .
WORKDIR "/src/src/Freelance.Face.Api"
RUN dotnet build "./Freelance.Face.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Freelance.Face.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
ARG APP_UID=0
WORKDIR /app

COPY --from=publish /app/publish .

USER root
COPY ["models","./models"]
RUN chmod -R a+rX /app/models
USER ${APP_UID}

ENV ASPNETCORE_URLS=http://+:8080
ENTRYPOINT ["dotnet", "Freelance.Face.Api.dll"]
