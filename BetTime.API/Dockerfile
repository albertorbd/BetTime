FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /app

COPY *.sln .
COPY BetTime.Models/*.csproj BetTime.Models/
COPY BetTime.Business/*.csproj BetTime.Business/
COPY BetTime.Data/*.csproj BetTime.Data/
COPY BetTime.API/*.csproj BetTime.API/
RUN dotnet restore BetTime.sln

COPY . .
RUN dotnet publish BetTime.sln -c Release -o API/out

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime
WORKDIR /app
COPY --from=build /app/API/out .

EXPOSE 80
ENTRYPOINT ["dotnet", "BetTime.API.dll"]