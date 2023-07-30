# :chart_with_upwards_trend: investment-manager

:bulb: The aim of this application...

## About the project

## Technologies and Features

### Client
* Vite
* TypeScript
* Vue 3
* VueX
* Vue Router

### Server
* .NET Core 7.0
* <span>ASP.NET WebApi</span>
* Clean Architecture
* Entity Framework Core - Code First
* Microsoft Sql Server
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
:warning: Since this is a personal project which was developed to practice software engineering skills, all server-sensitive data (e.g., db connection string, jwt token, and third-party API tokens) are stored on appsettings.json. In a real scenario, this information should be stored in a secret environment. For more information, check [here](https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-7.0)

#### Prerequisites
* Docker
* .NET Core 7.0 SDK (optional)

## :exclamation: Create a Finnhub API Token :exclamation:
1. The server connects to Finnhub API to consume US stock price quotes.  Please register [here](https://finnhub.io/) to get an individual access token.

2. Add the token in the `appsettings.json` (investment-manager/server/src/InvestmentManager.Web/appsettings.json)
```sh
  {
	...
	"FinnhubAccessToken": "my-created-token",
	...
  }
  ```

## :star: Running the server and database using Docker

Both the server application and database can run inside a Docker container following the steps below:

1. Run to following commands from the server folder where the .sln file is located (investment-manager/server)
```sh
  docker-compose build
  docker-compose up
```

This command will create and run two containers:\
a) **sql_server_db** which contains the Microsoft SQL Server database\
b) **investment_manager_web** which contains the .NET application and connects to the database container

2. The application should be running on http://localhost:5000/api

## :star: Running only the database using Docker

The application can be run without Docker, however, still using a container as an Microsoft SQL database following the steps below:

1. Ensure the tool EF is installed
```sh
  dotnet tool update --global dotnet-ef
  ```

2. Open a command prompt in the InvestmentManager.Web folder (investment-manager/server/src/InvestmentManager.Web) and execute the following command:
```sh
  dotnet restore
```

3. Run to following commands from the server folder where the .sln file is located (investment-manager/server)
```sh
  docker-compose build sql_server_db
  docker-compose up sql_server_db
```

This command will create and run a container only for the database.

4. Run the application from the InvestmentManager.Web folder (investment-manager/server/src/InvestmentManager.Web) 
```sh
  dotnet run
```

This command will start the .NET application

5. The application should be running on https://localhost:5252/api
   
## Client

#### Prerequisites
* Node Package Manager (NPM)

## Running the client

1. Install the project dependencies from the client folder (investment-manager/client/) using the following command: 
```sh
  npm install
```

2. Rename the file ***.env.example*** to ***.env*** in the investment-manager/client directory

3. Insert the correct server URL to the property ***VITE_BASE_URL*** according to the address where the server is running.

If the server and the database are running in a Docker container, the URL must be http://localhost:5000/api \
If only the database is running in a Docker container, the URL must be https://localhost:5252/api

4. To run the client, use the following command:
```sh
  npm run dev
```

:warning: Please remember to create and insert a [Finnhub token](https://finnhub.io/) before using the application.


