FROM mcr.microsoft.com/dotnet/runtime:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["RandomCoffee/RandomCoffee.csproj", "RandomCoffee/"]
RUN dotnet restore "RandomCoffee/RandomCoffee.csproj"
COPY . .
WORKDIR "/src/RandomCoffee"
RUN dotnet build "RandomCoffee.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "RandomCoffee.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RandomCoffee.dll"]
