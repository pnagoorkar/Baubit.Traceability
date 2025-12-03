# Baubit.Traceability

[![CircleCI](https://dl.circleci.com/status-badge/img/circleci/TpM4QUH8Djox7cjDaNpup5/2zTgJzKbD2m3nXCf5LKvqS/tree/master.svg?style=svg)](https://dl.circleci.com/status-badge/redirect/circleci/TpM4QUH8Djox7cjDaNpup5/2zTgJzKbD2m3nXCf5LKvqS/tree/master)
[![codecov](https://codecov.io/gh/pnagoorkar/Baubit.Traceability/branch/master/graph/badge.svg)](https://codecov.io/gh/pnagoorkar/Baubit.Traceability)<br/>
[![NuGet](https://img.shields.io/nuget/v/Baubit.Traceability.svg)](https://www.nuget.org/packages/Baubit.Traceability/)
![.NET Standard 2.0](https://img.shields.io/badge/.NET%20Standard-2.0-512BD4?logo=dotnet&logoColor=white)<br/>
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
[![Known Vulnerabilities](https://snyk.io/test/github/pnagoorkar/Baubit.Traceability/badge.svg)](https://snyk.io/test/github/pnagoorkar/Baubit.Traceability)

A lightweight .NET library for building robust error handling and result tracing in .NET applications using the FluentResults pattern.

## Overview

**Baubit.Traceability** provides a comprehensive set of abstractions and extension methods for working with operation results, errors, and success states. It extends the popular [FluentResults](https://github.com/altmann/FluentResults) library with additional traceability features, making it easier to track, understand, and debug complex operation chains in your applications.

## Features

- 🎯 **Abstract Base Classes** - Ready-to-use base classes for errors, reasons, and successes
- 🔍 **Rich Traceability** - Track operation results with metadata and timestamps
- 🛡️ **Type-Safe Error Handling** - Leverage .NET type system for robust error handling
- 🔄 **FluentResults Integration** - Seamlessly extends FluentResults with additional functionality
- 📦 **Zero Configuration** - Works out of the box with sensible defaults

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

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Acknowledgments

See [ACKNOWLEDGEMENT.md](./ACKNOWLEDGEMENT.md) for details on libraries and ideas that support this project.

## Support

- 📖 [Documentation](https://github.com/pnagoorkar/Baubit.Traceability)
- 🐛 [Issue Tracker](https://github.com/pnagoorkar/Baubit.Traceability/issues)
- 💬 [Discussions](https://github.com/pnagoorkar/Baubit.Traceability/discussions)

---

**Copyright © 2025 Prashant Nagoorkar**
