#1 - build
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY *.sln* .

COPY PrepPal+.Core/*.csproj PrepPal+.Core/
COPY PrepPal+.Infrastructure/*.csproj PrepPal+.Infrastructure/
COPY PrepPal+.UI/*.csproj PrepPal+.UI/

RUN dotnet restore

COPY . .

#2 - publish
WORKDIR /src/PrepPal+.UI
RUN dotnet publish -c Release -o /app/publish


#3 - runtime
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app

COPY --from=build /app/publish .
EXPOSE 8080

ENTRYPOINT ["dotnet", "PrepPal+.UI.dll"]