#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

#For Ubuntu use this one
FROM mcr.microsoft.com/dotnet/runtime:3.1 AS base

#For RPI use this one
#FROM mcr.microsoft.com/dotnet/core/aspnet:3.1.2-buster-slim-arm32v7 AS base

WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["PortBridgeServiceEdge/PortBridgeServiceEdge.csproj", "PortBridgeServiceEdge/"]
COPY ["AzureRelayPortBridge/AzureRelayPortBridge.csproj", "AzureRelayPortBridge/"]
RUN dotnet restore "PortBridgeServiceEdge/PortBridgeServiceEdge.csproj"
COPY . .
WORKDIR "/src/PortBridgeServiceEdge"
RUN dotnet build "PortBridgeServiceEdge.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PortBridgeServiceEdge.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PortBridgeServiceEdge.dll"]