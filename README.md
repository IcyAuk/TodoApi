# Todo API
Todo REST API with state persistence using ASP.NET Core and SQL Server.
Docker-ready: Container for the database and API can be built immediately.
Primary goal:
- Explore project deployment and scale a project up all the way to Azure to automate migrations without embedding them in the code.
- Make a project docker-ready with volumes
- Make Integration Tests

## Stack
- ASP.NET Core 9
- EF Core
- SQL Server & InMemory
- Swagger
- Docker & Docker Compose
- xUnit

docker compose up -d --build
Then go to /swagger
