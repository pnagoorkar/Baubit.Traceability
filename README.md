# Baubit.Traceability

[![CircleCI](https://dl.circleci.com/status-badge/img/gh/pnagoorkar/Baubit.Traceability/tree/master.svg?style=svg)](https://dl.circleci.com/status-badge/redirect/gh/pnagoorkar/Baubit.Traceability/tree/master)
[![NuGet](https://img.shields.io/nuget/v/Baubit.Traceability.svg)](https://www.nuget.org/packages/Baubit.Traceability)
[![codecov](https://codecov.io/gh/pnagoorkar/Baubit.Traceability/branch/master/graph/badge.svg)](https://codecov.io/gh/pnagoorkar/Baubit.Traceability)

A lightweight .NET library for building robust error handling and result tracing in .NET applications using the FluentResults pattern.

## Overview

**Baubit.Traceability** provides a comprehensive set of abstractions and extension methods for working with operation results, errors, and success states. It extends the popular [FluentResults](https://github.com/altmann/FluentResults) library with additional traceability features, making it easier to track, understand, and debug complex operation chains in your applications.

## Features

- 🎯 **Abstract Base Classes** - Ready-to-use base classes for errors, reasons, and successes
- 🔍 **Rich Traceability** - Track operation results with metadata and timestamps
- 🛡️ **Type-Safe Error Handling** - Leverage .NET type system for robust error handling
- 🔄 **FluentResults Integration** - Seamlessly extends FluentResults with additional functionality
- 📦 **Zero Configuration** - Works out of the box with sensible defaults
- ✅ **100% Test Coverage** - Thoroughly tested with comprehensive unit tests

## Installation

```bash
dotnet add package Baubit.Traceability
```

## Quick Start

### Basic Error Handling

```csharp
using Baubit.Traceability;
using FluentResults;

public class UserService
{
    public Result<User> GetUser(int id)
    {
        var result = ValidateUserId(id);
        
        if (result.IsFailed)
        {
            return result.ToResult<User>()
                .AddReasonIfFailed(new UserNotFoundReason(id));
        }
        
        var user = FetchUserFromDatabase(id);
        return Result.Ok(user);
    }
}
```

### Custom Errors and Reasons

```csharp
using Baubit.Traceability.Errors;
using Baubit.Traceability.Reasons;

public class UserNotFoundError : AError
{
    public UserNotFoundError(int userId) 
        : base([], $"User with ID {userId} was not found", new Dictionary<string, object>
        {
            { "UserId", userId },
            { "Timestamp", DateTime.UtcNow }
        })
    {
    }
}

public class UserNotFoundReason : AReason
{
    public UserNotFoundReason(int userId) 
        : base($"User {userId} does not exist in the database", new Dictionary<string, object>
        {
            { "UserId", userId }
        })
    {
    }
}
```

### Using Extension Methods

```csharp
using Baubit.Traceability;
using FluentResults;

// Throw exception on failure
var result = PerformOperation()
    .ThrowIfFailed();

// Add success markers
var successResult = Result.Ok(data)
    .AddSuccessIfPassed(new OperationSuccess("Data loaded successfully"));

// Unwrap nested reasons
var reasons = complexResult.UnwrapReasons();

// Dispose resources safely
var disposables = new List<IDisposable> { resource1, resource2 };
var disposeResult = disposables.Dispose();
```

### Working with Successes

```csharp
using Baubit.Traceability.Successes;

public class DataSavedSuccess : ASuccess
{
    public DataSavedSuccess(string entityId) 
        : base($"Data saved successfully for entity {entityId}", new Dictionary<string, object>
        {
            { "EntityId", entityId },
            { "SavedAt", DateTime.UtcNow }
        })
    {
    }
}

public Result SaveData(Data data)
{
    // Save logic...
    
    return Result.Ok()
        .AddSuccessIfPassed(new DataSavedSuccess(data.Id));
}
```

## API Reference

### Base Classes

#### `AError`
Abstract base class for custom error types. Provides:
- `Message` - Error message
- `Reasons` - List of nested error reasons
- `Metadata` - Additional context as key-value pairs
- `CreationTime` - Timestamp when error was created

#### `AReason`
Abstract base class for custom reason types. Provides:
- `Message` - Reason description
- `Metadata` - Additional context as key-value pairs
- `CreationTime` - Timestamp when reason was created

#### `ASuccess`
Abstract base class for custom success types. Inherits from `AReason` and implements `ISuccess`.

### Extension Methods

#### Result Extensions

- **`ThrowIfFailed<TResult>()`** - Throws `FailedOperationException` if result is failed
- **`ThrowIfFailed<TResult>(Task<TResult>)`** - Async version of ThrowIfFailed
- **`AddSuccessIfPassed<TResult>(params ISuccess[])`** - Adds success markers when result succeeds
- **`AddReasonIfFailed(params IReason[])`** - Adds reasons when result fails
- **`AddErrorIfFailed<TError>(params TError[])`** - Adds errors when result fails
- **`UnwrapReasons<TResult>()`** - Recursively unwraps all reasons including nested exceptions
- **`GetNonErrors<TResult>()`** - Returns only non-error reasons

#### Collection Extensions

- **`Dispose<TDisposable>(IList<TDisposable>)`** - Safely disposes a collection of disposable objects

### Exceptions

#### `FailedOperationException`
Exception thrown by `ThrowIfFailed()` when a result has failed. Contains the original `IResultBase` result.

## Advanced Scenarios

### Nested Error Unwrapping

```csharp
try
{
    var result = PerformComplexOperation()
        .ThrowIfFailed();
}
catch (FailedOperationException ex)
{
    var allReasons = ex.Result.UnwrapReasons();
    // Process all reasons including those from nested exceptions
}
```

### Conditional Success Tracking

```csharp
var result = Result.Ok(data);

if (shouldLogSuccess)
{
    result = result.AddSuccessIfPassed(
        (r, successes) => r.WithSuccesses(successes),
        new OperationSuccess("Operation completed")
    );
}
```

### Resource Cleanup

```csharp
var resources = new List<IDisposable> 
{ 
    connection, 
    transaction, 
    reader 
};

var cleanupResult = resources.Dispose();

if (cleanupResult.IsFailed)
{
    logger.LogError("Failed to dispose resources", cleanupResult.Errors);
}
```

## Building from Source

```bash
# Clone the repository
git clone https://github.com/pnagoorkar/Baubit.Traceability.git
cd Baubit.Traceability

# Build the solution
dotnet build

# Run tests
dotnet test

# Run tests with coverage
dotnet test --collect:"XPlat Code Coverage"
```

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request. For major changes, please open an issue first to discuss what you would like to change.

### Development Guidelines

1. Ensure all tests pass before submitting PR
2. Add tests for new functionality
3. Maintain 100% code coverage
4. Follow existing code style and conventions
5. Update documentation for API changes

## CI/CD Pipeline

This project uses CircleCI for continuous integration:

1. **Build** - Compiles the solution
2. **Test** - Runs all unit tests with code coverage
3. **Pack & Publish** - Creates and publishes NuGet packages (master branch)
4. **Release** - Publishes to NuGet.org (release branch)

### Required Setup

To set up CI/CD for this repository:

1. **CircleCI Context Configuration**
   - Add `CODECOV_TOKEN_Baubit_Traceability` to `Context_Prashant` context
   - Configure Docker Hub credentials
   - Configure GitHub token for package publishing

2. **Codecov Integration**
   - Import repository at [codecov.io](https://codecov.io)
   - Copy the Codecov token to CircleCI context

3. **Snyk Integration**
   - Import repository at [snyk.io](https://snyk.io)
   - Configure security scanning

4. **GitHub Settings**
   - Enable branch protection for `master` and `release` branches
   - Require status checks to pass before merging
   - Require code review before merging

## Dependencies

- **.NET 9.0** - Target framework
- **FluentResults 3.16.0** - Core result pattern library

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Acknowledgments

This library is part of the [Baubit](https://github.com/pnagoorkar/Baubit) framework ecosystem and was extracted to provide standalone traceability functionality.

## Support

- 📖 [Documentation](https://github.com/pnagoorkar/Baubit.Traceability)
- 🐛 [Issue Tracker](https://github.com/pnagoorkar/Baubit.Traceability/issues)
- 💬 [Discussions](https://github.com/pnagoorkar/Baubit.Traceability/discussions)

---

**Copyright © 2025 Prashant Nagoorkar**