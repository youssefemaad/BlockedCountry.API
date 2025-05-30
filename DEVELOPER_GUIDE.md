# Developer Guide

This guide provides detailed information for developers working on the Blocked Countries API project.

## Project Structure

The Blocked Countries API follows Clean Architecture principles, with clear separation of concerns:

```
BlockedCountriesAPI/              # Web API project
Core/
  DomainLayer/                    # Entities, interfaces, business rules
  ServiceAbstraction/             # Service interfaces
  Service/                        # Service implementations
Infrastructure/
  Persistence/                    # Data access, repositories
  Presentation/                   # Controllers, API endpoints
Shared/                           # DTOs, shared utilities
```

## Development Environment Setup

### Requirements

- .NET 8.0 SDK or later
- Visual Studio 2022 or VS Code with C# extensions
- Git

### Getting Started

1. Clone the repository
2. Open the solution file `BlockedCountriesAPI.sln` in Visual Studio or open the folder in VS Code
3. Restore NuGet packages: `dotnet restore`
4. Build the solution: `dotnet build`
5. Run the application: `dotnet run --project BlockedCountriesAPI/BlockedCountriesAPI.csproj`

## Key Design Patterns

### Repository Pattern

Used for data access abstraction in the `IBlockedCountryRepository` and `ILogService` interfaces.

### Dependency Injection

All services are registered in `Program.cs` with appropriate lifetimes:

- Singleton for stateful services maintaining in-memory data
- Scoped for request-bound services

### AutoMapper

Used for mapping between domain models and DTOs. Configured in the `Persistence` layer.

## Adding New Features

### Adding a New Entity

1. Create the entity class in `Core/DomainLayer/Models`
2. Create a corresponding DTO in `Shared/DataTransferObject`
3. Update the AutoMapper profile in `Infrastructure/Persistence/Mapping/MappingProfile.cs`
4. Create or update repository interfaces in `Core/DomainLayer/Interfaces`
5. Implement repository interfaces in `Infrastructure/Persistence`

### Adding a New API Endpoint

1. Define service methods in the appropriate service interface
2. Implement the service methods in the service implementation
3. Add controller methods in the appropriate controller
4. Register any new services in `Program.cs`

## Testing

### Unit Tests

Create unit tests for service implementations focusing on the business logic.

### Integration Tests

Create integration tests for the API endpoints to verify the entire request/response flow.

### Testing Tools

- xUnit for test framework
- Moq for mocking dependencies
- FluentAssertions for readable assertions

## Best Practices

### Data Transfer

- Use DTOs for all data moving across layer boundaries
- Avoid exposing domain models directly through the API
- Use AutoMapper to handle the mapping between domain models and DTOs

### Exception Handling

- Use custom domain exceptions for specific business rule violations
- Handle exceptions at the controller level using filters
- Return appropriate HTTP status codes for different types of errors

### Dependency Injection

- Always program to interfaces, not implementations
- Register services with appropriate lifetimes
- Consider the lifecycle implications when injecting dependencies

### Performance Considerations

- Use pagination for endpoints returning large collections
- Consider caching for frequently accessed data
- Use asynchronous methods for I/O-bound operations

## AutoMapper Configuration

### Adding New Maps

To add new mappings between domain models and DTOs:

1. Update the `MappingProfile` class in `Infrastructure/Persistence/Mapping/MappingProfile.cs`
2. For complex mappings, use projection or custom value resolvers
3. Ensure that all properties are mapped correctly

Example:

```csharp
// Simple mapping
CreateMap<SourceClass, DestinationClass>();

// Mapping with custom member configuration
CreateMap<SourceClass, DestinationClass>()
    .ForMember(dest => dest.DestProperty, opt => opt.MapFrom(src => src.SourceProperty))
    .ForMember(dest => dest.ComputedValue, opt => opt.MapFrom(src => CalculateValue(src)));
```

## Extending the Project

### Persistence Layer

The current implementation uses in-memory repositories. To add database persistence:

1. Add Entity Framework Core packages to the Persistence project
2. Create a DbContext class with entity configurations
3. Implement repository interfaces using the DbContext
4. Update the dependency injection in `Program.cs`

### Authentication and Authorization

To add authentication:

1. Configure authentication in `Program.cs`
2. Add authorization attributes to controllers or specific endpoints
3. Implement user service interfaces and implementations

### Logging

The project uses the built-in logging in ASP.NET Core. To enhance logging:

1. Configure specific logging providers in `Program.cs`
2. Use the `ILogger<T>` interface in services and controllers
3. Consider adding structured logging for better analysis
