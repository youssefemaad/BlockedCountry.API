# Blocked Countries API

A comprehensive system for managing country-based IP blocking with Clean Architecture design principles. This application allows for blocking access from specific countries, keeping logs of blocked attempts, and providing geolocation services.

## Project Overview

The Blocked Countries API is designed to help manage access to web services based on country location. It provides endpoints to:

- Block/unblock specific countries
- Check if an IP address is from a blocked country
- View logs of blocked access attempts
- Automatically clean up expired blocks

## Architecture

This project follows Clean Architecture principles, separating concerns into distinct layers:

### Core Layer

- **DomainLayer**: Contains entity models and core interfaces
- **ServiceAbstraction**: Defines service contracts
- **Service**: Implements business logic and service contracts

### Infrastructure Layer

- **Persistence**: Handles data storage and retrieval
- **Presentation**: Contains API controllers and presentation logic

### Shared Layer

- Contains DTOs used across the application

## Key Components

### Services

#### BlockedCountryService

Manages the blocked countries list with the following features:

- Add/remove countries to/from the blocked list
- Check if a country is blocked
- Support for temporary blocks with automatic expiration
- Pagination and search for retrieving blocked countries

#### BlockedAttemptLogService

Manages logs of access attempts from blocked countries:

- Log attempts with IP address, timestamp, and blocked status
- Retrieve logs for analysis

#### GeoLocationService

Determines the country of origin for IP addresses:

- Convert IP addresses to country codes

#### ExpiredBlocksCleanupService

Background service that automatically removes expired blocks:

- Runs as a hosted service
- Configurable cleanup interval

### APIs

#### Countries Controller

```
POST /api/countries/block            - Block a country
DELETE /api/countries/unblock/{code} - Unblock a country
GET /api/countries/blocked           - Get all blocked countries (with pagination)
POST /api/countries/block/temporal   - Add a temporary block
```

#### IP Controller

```
GET /api/ip/ip-lookup                - Get country for an IP address
POST /api/ip/check                   - Check if an IP is from a blocked country
```

#### Logs Controller

```
GET /api/logs                        - Retrieve access attempt logs
```

## Configuration

### Dependency Injection

The application uses the following service lifetimes:

- **Singleton Services**:

  - `IBlockedCountryRepository`
  - `ILogService`
  - `IBlockedCountryService`
  - `IBlockedAttemptLogService`

- **Scoped Services**:

  - `IGeoLocationService`

- **Hosted Services**:
  - `ExpiredBlocksCleanupService`

### AutoMapper Integration

The project uses AutoMapper to map between domain models and DTOs. The mapping profiles are defined in the `Persistence` layer.

## Data Flow

1. Client sends a request
2. API controller receives the request
3. Service layer processes the business logic
4. Repository layer handles data operations
5. Results are mapped back using AutoMapper
6. Response is returned to the client

## Getting Started

### Prerequisites

- .NET 8.0 SDK or later

### Running the Application

1. Clone the repository
2. Navigate to the project directory
3. Run `dotnet build` to build the project
4. Run `dotnet run --project BlockedCountriesAPI/BlockedCountriesAPI.csproj` to start the API

### API Documentation

Once running, Swagger documentation is available at:

```
https://localhost:5001/swagger
```

## Implementation Details

### In-Memory Storage

The current implementation uses in-memory repositories for storing blocked countries and logs. This can be extended to use persistent storage by implementing the respective repository interfaces.

### Country Identification

The system identifies countries using standard two-letter country codes (ISO 3166-1 alpha-2).

### Temporal Blocks

Countries can be blocked temporarily by specifying a duration. After the duration expires, the block is automatically removed by the cleanup service.
