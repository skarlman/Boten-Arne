FROM mcr.microsoft.com/dotnet/core/runtime:3.0-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
WORKDIR /src
COPY ["TwitchHandler/TwitchHandler.csproj", "TwitchHandler/"]
RUN dotnet restore "TwitchHandler/TwitchHandler.csproj"
COPY . .
WORKDIR "/src/TwitchHandler"
RUN dotnet build "TwitchHandler.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TwitchHandler.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TwitchHandler.dll"]