# Book Catalog

## Objective

This project builds a book catalog and allows filtering its data on a web page.

[Demonstration video](https://youtu.be/T8g4CnnqaV0)

## Running the project

**Starting SQL Server**

```shell
docker run --env=ACCEPT_EULA=Y --env=SA_PASSWORD=DB_Password --env=MSSQL_PID=developer --network=bridge -p 1433:1433 -d mcr.microsoft.com/mssql/server:2022-latest
```

**Creating Database**

```sql
CREATE DATABASE BooksCatalog;
GO

USE BooksCatalog;
GO

CREATE TABLE Author (
    Id INT NOT NULL,
    Name NVARCHAR(150) NOT NULL,
    PRIMARY KEY (Id)
);
GO

CREATE TABLE Book (
    Id INT NOT NULL,
    Title NVARCHAR(250) NOT NULL,
    CoverUri VARCHAR(70) NULL,
    Subject VARCHAR(30) NOT NULL,
    PublishYear INT NOT NULL,
    AuthorId INT NOT NULL,
    PRIMARY KEY (Id),
    FOREIGN KEY (AuthorId) REFERENCES AUTHOR(Id) ON DELETE CASCADE
);
GO
```

**Running Startup Projects**

- **WebApi**: `dotnet run --project src/backend/Presentation/BooksCatalogApi/BooksCatalogApi.csproj`
- **WorkerService**: `dotnet run --project src/backend/Infrastructure/Workers/Worker.csproj`

**Running Frontend**

- `cd src/frontend && npm run dev`