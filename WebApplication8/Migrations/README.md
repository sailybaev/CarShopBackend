# Database Migration Instructions

## Prerequisites
1. Install PostgreSQL on your machine
2. Install EF Core CLI tools globally:
   ```bash
   dotnet tool install --global dotnet-ef
   ```

## Create and Apply Migrations

1. **Create Initial Migration:**
   ```bash
   cd WebApplication8
   dotnet ef migrations add InitialCreate
   ```

2. **Update Database:**
   ```bash
   dotnet ef database update
   ```

## Connection String
Update the connection string in `appsettings.json` with your PostgreSQL credentials:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=CarShopDb;Username=postgres;Password=YOUR_PASSWORD"
  }
}
```

## Manual Database Creation (Alternative)
If you prefer to create the database manually, use these SQL commands:

```sql
CREATE DATABASE "CarShopDb";

\c "CarShopDb"

CREATE TABLE "Users" (
    "Id" SERIAL PRIMARY KEY,
    "Username" VARCHAR(50) NOT NULL UNIQUE,
    "Password" TEXT NOT NULL,
    "Role" VARCHAR(20) NOT NULL DEFAULT 'Client'
);

CREATE TABLE "Cars" (
    "Id" SERIAL PRIMARY KEY,
    "Brand" VARCHAR(100) NOT NULL,
    "Model" VARCHAR(100) NOT NULL,
    "Year" INTEGER NOT NULL,
    "Price" DECIMAL(18,2) NOT NULL
);
```

