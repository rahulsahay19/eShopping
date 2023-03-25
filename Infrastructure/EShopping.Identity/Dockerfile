FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Infrastructure/EShopping.Identity/EShopping.Identity.csproj", "Infrastructure/EShopping.Identity/"]
RUN dotnet restore "Infrastructure/EShopping.Identity/EShopping.Identity.csproj"
COPY . .
WORKDIR "/src/Infrastructure/EShopping.Identity"
RUN dotnet build "EShopping.Identity.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "EShopping.Identity.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EShopping.Identity.dll"]
