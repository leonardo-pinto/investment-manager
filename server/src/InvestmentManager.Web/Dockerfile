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
#EXPOSE 5252

#FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
#WORKDIR /src
#COPY  *.sln ./
#COPY ["src/InvestmentManager.Web/InvestmentManager.Web.csproj", "src/InvestmentManager.Web/"]
#COPY ["src/InvestmentManager.ApplicationCore/InvestmentManager.ApplicationCore.csproj", "src/InvestmentManager.ApplicationCore/"]
#COPY ["src/InvestmentManager.Infrastructure/InvestmentManager.Infrastructure.csproj", "src/InvestmentManager.Infrastructure/"]
#RUN dotnet restore "src/InvestmentManager.Web/InvestmentManager.Web.csproj"
#COPY . .
#WORKDIR "/src/src/InvestmentManager.Web"
#RUN dotnet build "InvestmentManager.Web.csproj" -c Release -o /app/build

#FROM build AS publish
#RUN dotnet publish "InvestmentManager.Web.csproj" -c Release -o /app/publish /p:UseAppHost=false
#
#FROM base AS final
#WORKDIR /app
#COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "InvestmentManager.Web.dll"]