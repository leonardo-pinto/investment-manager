# :chart_with_upwards_trend: investment-manager

:bulb: The aim of this application...

## About the project

## Technologies and Features

### Client

### Server
* .NET Core 7.0
* ASP.NET WebApi
* Clean Architecture
* Entity Framework Core - Code First
* Sql Server
* Microsoft Identity with JWT Authentication
* In-Memory Caching
* AutoMapper
* xUnit
* Moq
* AutoFixture
* FluentAssertions
* Docker
* Swagger

## Getting Started

### Server

#### Prerequisites
* .NET Core 7.0 SDK
* Docker (optional)

#### Create a Finnhub API Token
1. The server connects to Finnhub API to consume US stock price quotes.  Please register [here](https://finnhub.io/) to get an individual access token.

2. Add the token in the `appsettings.json` (investment-manager/server/src/InvestmentManager.Web/appsettings.json)
```sh
  {
	"FinnhubAccessToken": "my-created-token"
  }
  ```


#### Running the server using a local SQL Server

1. Add your connection string in appsettings.json to point to a local SQL Server instance
```sh
  {
	"ConnectionStrings": {
		"ApplicationDbContext": "sql-server-connection-string"
	},
  }
  ```

2. Ensure the tool EF is installed
```sh
  dotnet tool update --global dotnet-ef
  ```

3. Open a command prompt in the InvestmentManager.Web folder (investment-manager/server/src/InvestmentManager.Web) and execute the following commands:
```sh
  dotnet restore
  dotnet ef database update -p ../InvestmentManager.Infrastructure.csproj
  ```

These commands will create a single database, which will store user credentials, identity data, stock positions and transactions.

4. Run the application from the InvestmentManager.Web folder (investment-manager/server/src/InvestmentManager.Web) 
```sh
  dotnet run
  ```


