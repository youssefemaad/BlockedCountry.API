
# BlockedCountry.API Project Architecture

## Overview

This document outlines the architectural design of the BlockedCountry.API project, which follows Clean Architecture principles to create a maintainable, testable system for managing country-based IP blocking.

## Clean Architecture Layers

The project is organized into concentric layers, where dependencies point inward:

![Clean Architecture Diagram](https://lucid.app/publicSegments/view/108b0a6c-bedb-49d1-a592-146f0cd9d09b/image.png)

### 1. Core Layer

#### Domain Layer

- Contains enterprise business rules and entities
- Has no dependencies on other layers or external libraries

**Key Components:**

- `BlockedCountry`: Entity representing a blocked country with optional expiration
- `BlockedAttemptLog`: Entity for logging access attempts
- `TemporalBlock`: Entity for temporary blocks
- Core interfaces for repositories (`IBlockedCountryRepository`, `ILogService`)

#### Service Abstraction

- Defines application service interfaces
- Represents use cases of the system

**Key Components:**

- `IBlockedCountryService`: Interface for managing blocked countries
- `IBlockedAttemptLogService`: Interface for managing access logs
- `IGeoLocationService`: Interface for IP geolocation

#### Service Implementation

- Implements the business logic
- Depends only on domain layer and service abstractions

**Key Components:**

- `BlockedCountryService`: Manages country blocking logic
- `BlockedAttemptLogService`: Handles logging of access attempts
- `GeoLocationService`: Provides IP to country resolution

### 2. Infrastructure Layer

#### Persistence

- Implements data access logic
- Contains repository implementations
- Manages AutoMapper configuration

**Key Components:**

- Repository implementations (`InMemoryBlockedCountryRepository`)
- AutoMapper profiles for DTO mapping
- Data access services

#### Presentation

- Contains API controllers
- Handles HTTP requests/responses
- Depends on service abstractions

**Key Components:**

- `CountriesController`: Endpoints for country blocking operations
- `IpController`: Endpoints for IP checking and geolocation
- `LogsController`: Endpoints for access logs

### 3. Shared Layer

- Contains DTOs shared between layers
- Has no dependencies on other project layers

**Key Components:**

- `BlockCountryDto`: DTO for blocked country data
- `BlockedAttemptDto`: DTO for access attempt log data
- `BlockedCountryResponse`: Response DTO for blocked country data
- `PaginatedResult<T>`: Generic DTO for paginated responses

### 4. Application Layer (BlockedCountriesAPI)

- Entry point for the application
- Configures dependency injection
- Sets up middleware and services

**Key Components:**

- `Program.cs`: Application bootstrapping and configuration
- `ExpiredBlocksCleanupService`: Background service for cleaning up expired blocks

## Dependency Flow

The dependency flow follows the Dependency Inversion Principle, where:

1. Core domain doesn't depend on any other layers
2. Application services depend only on the domain layer
3. Infrastructure implementations depend on the interfaces defined in the core layer
4. The application entry point depends on everything to wire it all together

## Data Flow

1. Client request comes in through the API controllers in the Presentation layer
2. Controller calls the appropriate service interface
3. Service implementation executes business logic using domain entities
4. Service calls repository interfaces when data access is needed
5. Repository implementations in the Persistence layer handle data operations
6. Results flow back up through the layers, mapped to appropriate DTOs

## Cross-Cutting Concerns

### Mapping

- AutoMapper for mapping between domain models and DTOs
- Mapping profiles defined in the Persistence layer

### Dependency Injection

- Services registered with appropriate lifetimes in `Program.cs`
- Singleton for stateful in-memory repositories
- Scoped for request-bound services

### Background Processing

- `ExpiredBlocksCleanupService` runs as a hosted service
- Handles automatic cleanup of expired blocks

## Extension Points

The architecture is designed to be extensible:

1. **Persistent Storage**: Replace in-memory repositories with database implementations
2. **Authentication**: Add authentication/authorization services and middleware
3. **Advanced Geolocation**: Replace simple geolocation service with more advanced implementations
4. **Caching**: Add caching mechanisms for frequently accessed data

## Design Decisions

### In-Memory Storage

- Current implementation uses in-memory repositories for simplicity
- Designed to be easily replaced with persistent storage

### AutoMapper Integration

- AutoMapper used to map between domain models and DTOs
- Mapping configuration centralized in the Persistence layer
- Helps maintain clean separation between layers

### Service Lifetimes

- Singleton for stateful services (in-memory repositories)
- Scoped for services that should be created per request
- Ensuring proper resource management and state isolation
