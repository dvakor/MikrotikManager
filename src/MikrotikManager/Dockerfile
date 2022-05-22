FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["MikrotikManager/MikrotikManager.csproj", "MikrotikManager/"]
RUN dotnet restore "MikrotikManager/MikrotikManager.csproj"
COPY . .
WORKDIR "/src/MikrotikManager"
RUN dotnet build "MikrotikManager.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MikrotikManager.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MikrotikManager.dll"]
