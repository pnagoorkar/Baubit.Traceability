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
  - `Errors/AError/` - Tests for AError class
  - `Exceptions/FailedOperationException/` - Tests for FailedOperationException class
  - `Extensions/TraceabilityExtensions/` - Tests for TraceabilityExtensions class
  - `Reasons/AReason/` - Tests for AReason class
  - `Successes/ASuccess/` - Tests for ASuccess class
- `.circleci/config.yml` - CircleCI pipeline configuration
- `codecov.yml` - Code coverage configuration
- `README.md` - Project documentation

## Test Organization Standards

**IMPORTANT:** This project follows a strict folder-based test organization pattern:

1. **Folder Structure**: Test files MUST be organized in folders that mirror the source code structure
   - Each class under test gets its own folder
   - Example: `Baubit.Traceability.Tests/Errors/AError/` contains tests for the `AError` class

2. **Test File Naming**: All test files MUST be named `Test.cs`
   - ? **WRONG**: `AErrorTests.cs`, `TraceabilityExtensionsTests.cs`
   - ? **CORRECT**: `Errors/AError/Test.cs`, `Extensions/TraceabilityExtensions/Test.cs`

3. **Namespace Convention**: Test namespaces should match the folder structure
   - Example: `namespace Baubit.Traceability.Tests.Errors.AError`

4. **When Creating New Tests**:
   - Create a new folder matching the component path: `[ComponentType]/[ComponentName]/`
   - Place a `Test.cs` file inside that folder
   - Use the appropriate namespace matching the folder path

5. **When Modifying Tests**:
   - Always locate tests using the folder structure, not by filename pattern
   - Tests are in `[ComponentType]/[ComponentName]/Test.cs`, not `[ComponentName]Tests.cs`

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
- All tests use xUnit as the testing framework
- Test organization follows a strict folder-based structure with all test files named `Test.cs`
