# GitHub Copilot Instructions

This file contains instructions for GitHub Copilot to help you work more effectively with the Baubit.Traceability project.

## About This Project

Baubit.Traceability is a standalone .NET library extracted from the Baubit framework. It provides robust error handling and result tracing capabilities using the FluentResults pattern. The library includes:

- Abstract base classes for errors, reasons, and successes
- Extension methods for result manipulation and traceability
- Exception types for failed operations
- CircleCI integration for build, test, and publish workflows
- Code coverage reporting with Codecov
- NuGet package publishing

## Project Structure

- `Baubit.Traceability/` - Main library source code
  - `Errors/` - Error abstraction classes
  - `Exceptions/` - Custom exception types
  - `Reasons/` - Reason abstraction classes  
  - `Successes/` - Success abstraction classes
  - `TraceabilityExtensions.cs` - Extension methods for FluentResults
- `Baubit.Traceability.Tests/` - Comprehensive unit tests (100% coverage)
- `.circleci/config.yml` - CircleCI pipeline configuration
- `codecov.yml` - Code coverage configuration
- `README.md` - Project documentation

## Key Configuration Files

- `.circleci/config.yml` - CircleCI pipeline with Baubit.Traceability configuration
- `codecov.yml` - Code coverage targeting 100%
- `Baubit.Traceability/Baubit.Traceability.csproj` - Main project file with FluentResults dependency
- `Baubit.Traceability.Tests/Baubit.Traceability.Tests.csproj` - Test project file

## Building and Testing

```bash
# Build the solution
dotnet build

# Run tests
dotnet test

# Run tests with coverage
dotnet test --collect:"XPlat Code Coverage" --results-directory ./TestResults/
```

## CircleCI Workflow

The pipeline includes these jobs:
- **build**: Compiles the Baubit.Traceability solution
- **test**: Runs tests and uploads code coverage to Codecov
- **pack_and_publish**: Packages and publishes NuGet packages (master branch only)
- **release**: Publishes to NuGet.org (release branch only)

## Important Notes

- The library has 100% code coverage with 40 comprehensive tests
- The test job expects a Codecov token named `CODECOV_TOKEN_Baubit_Traceability` in CircleCI
- All jobs require the CircleCI context `Context_Prashant` to be configured with necessary credentials
- The library depends on FluentResults v3.16.0
