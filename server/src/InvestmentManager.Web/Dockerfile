FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /app

COPY *.sln. ./
COPY . .
WORKDIR /app/src/InvestmentManager.Web
RUN dotnet restore

RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
COPY --from=build /app/src/InvestmentManager.Web/out ./
EXPOSE 80

ENTRYPOINT ["dotnet", "InvestmentManager.Web.dll"]