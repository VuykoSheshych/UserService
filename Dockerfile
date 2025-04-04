FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ["UsersAndAuth.csproj", "./"]
RUN dotnet restore "UsersAndAuth.csproj"

COPY . .
RUN dotnet publish "UsersAndAuth.csproj" -c Release -o /app/publish --no-restore

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "UsersAndAuth.dll"]