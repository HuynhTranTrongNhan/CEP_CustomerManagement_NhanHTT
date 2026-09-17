# CEP Customer Management System

A basic Customer Management System developed as part of the CEP Software Developer practical assessment.

The application provides customer management features through an ASP.NET Core REST API and a Blazor WebAssembly user interface.

## Features

### Customer Management

- View customer list
- Search customers by Full Name or Phone Number
- Pagination
- Create customer
- Update customer information
- Delete customer with confirmation
- Active / Inactive customer status
- Automatic Customer Code generation

### Validation

The application validates customer information on both the frontend and backend:

- Full Name is required
- Phone Number is required
- Phone Number only accepts numeric characters
- Email must be in a valid email format
- Date of Birth cannot be in the future

### Authentication & Authorization

- JWT Bearer Authentication
- Login using administrator account
- Protected customer management APIs
- Protected Blazor routes
- JWT automatically attached to authorized API requests
- Logout and unauthorized redirect

## Technology Stack

### Backend

- ASP.NET Core Web API
- Entity Framework Core
- SQL Server
- JWT Bearer Authentication
- ASP.NET Core Identity PasswordHasher
- Data Annotations Validation

### Frontend

- Blazor WebAssembly
- MudBlazor
- Blazored.LocalStorage

### Database

- Microsoft SQL Server
- Entity Framework Core Code First
- EF Core Migrations

## Solution Architecture

The solution is separated into four projects:

```text
CustomerManagement
│
├── CustomerManagement.Domain
│   └── Domain entities
│
├── CustomerManagement.Infrastructure
│   ├── Entity Framework Core
│   ├── DbContext
│   ├── Entity configurations
│   ├── Repositories
│   └── Database initialization
│
├── CustomerManagement.Api
│   ├── Controllers
│   ├── DTOs
│   ├── Services
│   ├── Authentication
│   ├── Validation
│   └── Exception handling
│
└── CustomerManagement.Web
    ├── Blazor pages
    ├── Components
    ├── Models
    ├── Services
    ├── Authentication state
    └── HTTP authorization handler
```

The main backend request flow is:

```text
HTTP Request
     ↓
Controller
     ↓
Service
     ↓
Repository
     ↓
Entity Framework Core
     ↓
SQL Server
```

The Blazor WebAssembly application communicates with the backend through REST APIs.

## Database Design

### Customers

| Field | Description |
|---|---|
| Id | Primary key |
| CustomerCode | Unique customer code |
| FullName | Customer full name |
| Email | Customer email |
| PhoneNumber | Customer phone number |
| DateOfBirth | Date of birth |
| IsActive | Active status |
| CreatedAt | Created date |
| UpdatedAt | Last updated date |

### Users

| Field | Description |
|---|---|
| Id | Primary key |
| Username | Login username |
| PasswordHash | Hashed password |
| Role | User role |
| IsActive | Account status |
| CreatedAt | Created date |

## API Endpoints

### Authentication

| Method | Endpoint | Description |
|---|---|---|
| POST | `/api/Auth/login` | Login and receive JWT token |

### Customers

| Method | Endpoint | Description |
|---|---|---|
| GET | `/api/Customers` | Get customer list |
| GET | `/api/Customers/{id}` | Get customer details |
| POST | `/api/Customers` | Create customer |
| PUT | `/api/Customers/{id}` | Update customer |
| DELETE | `/api/Customers/{id}` | Delete customer |

Customer list supports search and pagination:

```http
GET /api/Customers?search=Nguyen&pageNumber=1&pageSize=10
```

All Customer endpoints require JWT authentication.

## Prerequisites

Before running the project, install:

- .NET SDK
- SQL Server
- SQL Server Management Studio (optional)
- Visual Studio 2022+ or another .NET-compatible IDE

## Local Configuration

Sensitive development configuration such as the database connection string and JWT signing key is stored using [.NET User Secrets](https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets) and is not committed to source control.

The repository contains the application configuration structure, while environment-specific secrets must be configured locally before running the API.

### Configure User Secrets

Initialize User Secrets for the API project:

```bash
dotnet user-secrets init --project CustomerManagement.Api
```

Configure the SQL Server connection string:

```bash
dotnet user-secrets set --project CustomerManagement.Api "ConnectionStrings:DefaultConnection" "YOUR_CONNECTION_STRING"
```

Example using SQL Server Authentication:

```bash
dotnet user-secrets set --project CustomerManagement.Api "ConnectionStrings:DefaultConnection" "Server=YOUR_SERVER;Database=CustomerManagementDb;User Id=YOUR_USER;Password=YOUR_PASSWORD;TrustServerCertificate=True"
```

Configure the JWT signing key:

```bash
dotnet user-secrets set --project CustomerManagement.Api "JwtSettings:SecretKey" "YOUR_SECRET_KEY"
```

The following JWT settings are non-sensitive and remain in `CustomerManagement.Api/appsettings.json`:

```json
{
  "JwtSettings": {
    "SecretKey": "",
    "Issuer": "CustomerManagement.Api",
    "Audience": "CustomerManagement.Web",
    "ExpirationMinutes": 60
  }
}
```

> Do not commit database passwords or JWT signing keys to source control.

## Database Configuration

Configure the SQL Server connection string in:

```text
CustomerManagement.Api/appsettings.json
```

Example:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER;Database=CustomerManagementDb;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

Update `YOUR_SERVER` according to your local SQL Server environment.

## Database Migration

Open a terminal at the solution directory.

If the EF Core CLI tool is not installed:

```bash
dotnet tool install --global dotnet-ef
```

Restore dependencies:

```bash
dotnet restore
```

Apply the existing migrations and create/update the database:

```bash
dotnet ef database update --project CustomerManagement.Infrastructure --startup-project CustomerManagement.Api
```

The application contains seed data for demonstration purposes.

## Running the Backend API

From the solution directory:

```bash
dotnet run --project CustomerManagement.Api
```

For the current development configuration, the API is available at:

```text
https://localhost:7137
```

Swagger/OpenAPI can be used to inspect and test the available API endpoints when enabled in the development environment.

## Running the Blazor Web Application

Open another terminal:

```bash
dotnet run --project CustomerManagement.Web
```

For the current development configuration, the Web application is available at:

```text
https://localhost:7046
```

The API URL used by the Blazor application is configured in:

```text
CustomerManagement.Web/wwwroot/appsettings.json
```

Example:

```json
{
  "ApiBaseUrl": "https://localhost:7137/"
}
```

If the API URL is changed, update this configuration accordingly.

## Demo Account

Use the following seeded administrator account:

```text
Username: admin
Password: Admin@123
```

The password is stored in the database as a password hash and is not stored as plain text.

## Authentication Flow

```text
Login
  ↓
POST /api/Auth/login
  ↓
Validate username/password
  ↓
Generate JWT
  ↓
Blazor stores JWT
  ↓
Authorization handler attaches Bearer token
  ↓
Protected Customer API
```

The JWT is stored in browser local storage for this demonstration project.

## Customer Code Generation

Customer codes are generated automatically by the backend after a customer is created.

Example:

```text
Id = 25
CustomerCode = KH000025
```

Customer Code cannot be modified through the Create or Update UI.

## Error Handling

The backend includes centralized exception handling and standardized API responses.

Example successful response:

```json
{
  "success": true,
  "message": "Success",
  "data": {}
}
```

Validation errors are returned using a consistent response structure.

## Project Highlights

- RESTful Customer CRUD API
- Layered project structure
- Repository and Service patterns
- Entity Framework Core Code First
- Fluent API entity configuration
- Server-side search and pagination
- Frontend and backend validation
- Global exception handling
- JWT authentication and authorization
- Password hashing
- MudBlazor UI components
- Responsive customer management interface
- Dependency Injection
- Async database and HTTP operations

## Notes

This project was developed as a practical assessment and focuses on clean structure, maintainability, validation, REST API design, authentication, and core customer management functionality.