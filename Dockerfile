FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["iOSClub.WebSite/iOSClub.WebSite.csproj", "iOSClub.WebSite/"]
COPY ["iOSClub.WebSite.Client/iOSClub.WebSite.Client.csproj", "iOSClub.WebSite.Client/"]
COPY ["iOSClub.Data/iOSClub.Data.csproj", "iOSClub.Data/"]
RUN dotnet restore "iOSClub.WebSite/iOSClub.WebSite.csproj"
COPY . .
WORKDIR "/src/iOSClub.WebSite"
RUN dotnet build "iOSClub.WebSite.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "iOSClub.WebSite.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
ENV SQL ""
ENV USER ""
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "iOSClub.WebSite.dll"]
