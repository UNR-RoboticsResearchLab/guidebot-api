FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY guidebot-api.csproj .
RUN dotnet restore guidebot-api.csproj

COPY . .
RUN dotnet publish guidebot-api.csproj -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_HTTP_PORTS=8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "guidebot-api.dll"]
