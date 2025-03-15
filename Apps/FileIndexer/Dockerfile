FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Apps/FileIndexer/ConsoleApplication/ConsoleApplication.csproj", "Apps/FileIndexer/ConsoleApplication/"]
COPY ["Apps/FileIndexer/Service/Service.csproj", "Apps/FileIndexer/Service/"]
COPY ["Apps/FileIndexer/Infrastructure/Infrastructure.csproj", "Apps/FileIndexer/Infrastructure/"]
COPY ["Packages/SharedModels/SharedModels.csproj", "Packages/SharedModels/"]
COPY ["Packages/Events/Events.csproj", "Packages/Events/"]
COPY ["Packages/Logger/Logger.csproj", "Packages/Logger/"]
RUN dotnet restore "Apps/FileIndexer/ConsoleApplication/ConsoleApplication.csproj"

COPY Apps/FileIndexer/ConsoleApplication/ Apps/FileIndexer/ConsoleApplication/
COPY Apps/FileIndexer/Service/ Apps/FileIndexer/Service/
COPY Apps/FileIndexer/Infrastructure/ Apps/FileIndexer/Infrastructure/
COPY Packages/SharedModels/ Packages/SharedModels/
COPY Packages/Events/ Packages/Events/
COPY Packages/Logger/ Packages/Logger/

WORKDIR "Apps/FileIndexer/ConsoleApplication"
RUN dotnet build "ConsoleApplication.csproj" -c $BUILD_CONFIGURATION -o /app/build


FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "ConsoleApplication.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ConsoleApplication.dll"]
