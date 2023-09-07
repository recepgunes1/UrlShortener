# UrlShotener
:information_source:
The URL Shortener is a web-based tool designed to take longer URLs and convert them into shorter, more manageable links. This is particularly useful for sharing links on platforms with character limits, making URLs more readable, and tracking link analytics.

[![.NET](https://github.com/recepgunes1/UrlShortener/actions/workflows/dotnet.yml/badge.svg)](https://github.com/recepgunes1/UrlShortener/actions/workflows/dotnet.yml)

:warning:
In Kibana, there isn't an automated mechanism to generate indexes. You'll need to manually create them for both `action_log` and `error_log`.

## Tech Stack
- Backend: .NET => ASP.NET, EF Core, Dapper, Ocelot, MassTransit, Quartz.NET, MediatR, xUnit, Moq
- Database: Postgres
- Message Borker: RabbitMQ
- Fronted: TypeScript and Vue.JS
- Other: Elasticsearch, Kibana, Docker and Docker-Compose, Swagger (for dev env)

## Installation Steps

1. Ensure you have the required software installed:
    * Docker
    * Git

2. Clone the repository

3. Navigate into the project directory

4. Use Docker to build and run the project:
    ```bash
    docker-compose up -d --build
    ```
5. Verify the installation:
    ```bash
    docker ps
    ```
