# LoanApplications

### Prerequisites

- .NET 10.0 SDK
- Visual Studio 2022 or JetBrains Rider with .NET Aspire plugin

## Setup

- Edit `src/LoanApplications.API/appsettings.json` and `src/LoanApplications.BackgroundProcessor/appsettings.json` to set an appropriate local path for Sqlite DB (must be the same for both)

## Running

Recommended to run via Aspire (from IDE or CLI)

Aspire will start the API immediately (which will scaffold the database) and the background processor can be ran from the aspire dashboard

You can also run the two projects seperately (via IDE or `dotnet run`) - But note the API must be ran first to scaffold the database

## Architecture Notes

### What would you change if this system had to handle **5,000,000 applications per day?**

1) More appropriate database (MSSQL, Postgres etc)

2) Seperate read and writes. Use a Readonly replica DB for reads etc

3) Replace the background polling mechanism with a message queue - API adds the queue. Service either reads from queue or a lambda or similar - triggered on message recieved. Allows for better scaling and resilience. Can scale on queue size.

4) Cache (Redis etc) for GET endpoint

5) Resiliency improvements - Rate limiting, connection resiliency for database etc

### What shortcuts or trade-offs did you make, and what would you do differently with more time?

1) Full E2E test of the flow (add application, get application, process in background)

2) More unit tests for mapping, validators etc

3) Addition of OTEL for tracing, metrics etc

4) Add a seperate db migrator rather than run in place in the API

5) Worker currently relies on a single instance (degree of idempotent checks but not foolproof) - Multi instances running simulatenously could cause problems.
