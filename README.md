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

## Server

#### Disclaimer
:warning: Since this is a personal project which was developed to practice software engineering skills, all server sensitive data (e.g., db connection string, jwt token and thirdt-party api tokens) are stored on appsettings.json. In a real scenario, these information should be stored in a secret environment. For more information, check [here](https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-7.0)

#### Prerequisites
* .NET Core 7.0 SDK (to run local)
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

The application can be run locally using a local SQL Server following the steps:

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

5. The application should be running on localhost:5252

#### Running the server using Docker

Both the application and database can run inside a Docker container following the steps:

1. Run to following commands from the server folder where the .sln file is located (investment-manager/server)
```sh
  docker-compose build
  docker-compose up
  ```

2. The application should be running on localhost:5000

