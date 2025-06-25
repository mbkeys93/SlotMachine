# Slot Machine API Unit Tests

This project contains comprehensive unit tests for the Slot Machine API using xUnit and dependency injection.

## Running Tests

### Run All Tests

```bash
dotnet test
```

### Run Tests with Verbose Output

```bash
dotnet test --verbosity normal
```

### Run Tests with Coverage

```bash
dotnet test --collect:"XPlat Code Coverage"
```

### Run Specific Test Class

```bash
dotnet test --filter "FullyQualifiedName~GameLogicServiceTests"
```

### Run Specific Test Method

```bash
dotnet test --filter "FullyQualifiedName~CreateUserAsync_WithNewUsername_ShouldCreateUser"
```

## Test Structure

### TestBase Class

The `TestBase` class provides:
- Dependency injection setup
- In-memory database configuration
- Service registration
- Test data seeding
- Proper disposal of resources

### Test Categories

1. **GameLogicServiceTests** - Tests for the main business logic service
   - User creation and management
   - Game spinning functionality
   - Free spins and cashout operations
   - Error handling

2. **UserModelTests** - Tests for the User model
   - Default values
   - CanPlay logic
   - Cash and free spin operations
   - Property validation

3. **GameModelTests** - Tests for the Game model
   - Win condition checking
   - Property validation
   - User relationship testing

## Test Features

### Dependency Injection

Tests use Microsoft's DI container with:
- In-memory Entity Framework database
- Real service implementations
- Proper service lifetime management

### In-Memory Database

Each test gets a fresh in-memory database:
- Isolated test data
- No external dependencies
- Fast execution
- Automatic cleanup

### Test Data Seeding

Default symbols are automatically seeded for tests that need them.

### Async/Await Support

All tests properly handle async operations with:
- `async Task` test methods
- Proper exception testing
- Async assertion methods

## Best Practices

### Test Naming Convention

Tests follow the pattern: `MethodName_Scenario_ExpectedResult`

Examples:
- `CreateUserAsync_WithNewUsername_ShouldCreateUser`
- `SpinAsync_WithInsufficientBalance_ShouldThrowException`

### Arrange-Act-Assert Pattern

All tests follow the AAA pattern:
- **Arrange**: Set up test data and conditions
- **Act**: Execute the method being tested
- **Assert**: Verify the expected results

### Test Isolation

Each test is completely isolated:
- Fresh database instance
- No shared state
- Independent execution

### Exception Testing

Use `Assert.ThrowsAsync<T>()` for testing exceptions:

```csharp
await Assert.ThrowsAsync<ArgumentException>(() => 
    _gameService.SpinAsync(999, 1.0m));
```

## Adding New Tests

### For New Services

1. Create a new test class inheriting from `TestBase`
2. Inject the service in the constructor
3. Follow the naming convention
4. Use the AAA pattern

### For New Models

1. Create a new test class (no need to inherit from `TestBase`)
2. Test all public methods and properties
3. Include edge cases and error conditions

### For Controllers

1. Use `TestServer` or `WebApplicationFactory`
2. Test HTTP responses and status codes
3. Verify JSON serialization/deserialization

## Continuous Integration

The tests are designed to run in CI/CD pipelines:
- No external dependencies
- Fast execution
- Reliable results
- Good coverage of business logic 