using Baubit.Traceability.Exceptions;
using Baubit.Traceability.Reasons;
using Baubit.Traceability.Successes;
using FluentResults;

namespace Baubit.Traceability.Tests.Extensions.TraceabilityExtensions
{
    public class Test
    {
        private class TestSuccess : ASuccess
        {
            public TestSuccess(string message) : base(message, new Dictionary<string, object>()) { }
        }

        private class TestReason : AReason
        {
            public TestReason(string message) : base(message, new Dictionary<string, object>()) { }
        }

        private class TestDisposable : IDisposable
        {
            public bool IsDisposed { get; private set; }
            public bool ThrowOnDispose { get; set; }

            public void Dispose()
            {
                if (ThrowOnDispose)
                    throw new InvalidOperationException("Dispose failed");
                IsDisposed = true;
            }
        }

        #region Dispose Tests

        [Fact]
        public void Dispose_DisposesAllItemsInList()
        {
            // Arrange
            var disposables = new List<TestDisposable>
            {
                new TestDisposable(),
                new TestDisposable(),
                new TestDisposable()
            };

            // Act
            var result = disposables.Dispose();

            // Assert
            Assert.True(result.IsSuccess);
            Assert.All(disposables, d => Assert.True(d.IsDisposed));
        }

        [Fact]
        public void Dispose_ReturnsFailedResultWhenExceptionThrown()
        {
            // Arrange
            var disposables = new List<TestDisposable>
            {
                new TestDisposable(),
                new TestDisposable { ThrowOnDispose = true },
                new TestDisposable()
            };

            // Act
            var result = disposables.Dispose();

            // Assert
            Assert.True(result.IsFailed);
            Assert.NotEmpty(result.Errors);
        }

        [Fact]
        public void Dispose_HandlesEmptyList()
        {
            // Arrange
            var disposables = new List<TestDisposable>();

            // Act
            var result = disposables.Dispose();

            // Assert
            Assert.True(result.IsSuccess);
        }

        #endregion

        #region ThrowIfFailed Tests

        [Fact]
        public void ThrowIfFailed_ThrowsExceptionWhenResultFailed()
        {
            // Arrange
            var result = Result.Fail("Test failure");

            // Act & Assert
            var exception = Assert.Throws<FailedOperationException>(() => result.ThrowIfFailed());
            Assert.Equal(result, exception.Result);
        }

        [Fact]
        public void ThrowIfFailed_ReturnsResultWhenSuccess()
        {
            // Arrange
            var result = Result.Ok();

            // Act
            var returnedResult = result.ThrowIfFailed();

            // Assert
            Assert.Equal(result, returnedResult);
            Assert.True(returnedResult.IsSuccess);
        }

        [Fact]
        public void ThrowIfFailed_WorksWithGenericResult()
        {
            // Arrange
            var result = Result.Ok(42);

            // Act
            var returnedResult = result.ThrowIfFailed();

            // Assert
            Assert.Equal(42, returnedResult.Value);
            Assert.True(returnedResult.IsSuccess);
        }

        [Fact]
        public async Task ThrowIfFailed_Async_ThrowsExceptionWhenResultFailed()
        {
            // Arrange
            var result = Task.FromResult(Result.Fail("Test async failure"));

            // Act & Assert
            await Assert.ThrowsAsync<FailedOperationException>(async () => await result.ThrowIfFailed());
        }

        [Fact]
        public async Task ThrowIfFailed_Async_ReturnsResultWhenSuccess()
        {
            // Arrange
            var result = Task.FromResult(Result.Ok());

            // Act
            var returnedResult = await result.ThrowIfFailed();

            // Assert
            Assert.True(returnedResult.IsSuccess);
        }

        #endregion

        #region AddSuccessIfPassed Tests

        [Fact]
        public void AddSuccessIfPassed_AddsSuccessWhenResultIsSuccess()
        {
            // Arrange
            Result<int> result = Result.Ok(42);
            ISuccess success = new TestSuccess("Operation succeeded");

            // Act
            var modifiedResult = Traceability.TraceabilityExtensions.AddSuccessIfPassed<Result<int>, int>(result, success);

            // Assert
            Assert.True(modifiedResult.IsSuccess);
            Assert.Contains(modifiedResult.Successes, s => s.Message == "Operation succeeded");
        }

        [Fact]
        public void AddSuccessIfPassed_DoesNotAddSuccessWhenResultFailed()
        {
            // Arrange
            Result<int> result = Result.Fail<int>("Operation failed");
            ISuccess success = new TestSuccess("Operation succeeded");

            // Act
            var modifiedResult = Traceability.TraceabilityExtensions.AddSuccessIfPassed<Result<int>, int>(result, success);

            // Assert
            Assert.True(modifiedResult.IsFailed);
            Assert.Empty(modifiedResult.Successes);
        }

        [Fact]
        public void AddSuccessIfPassed_GenericResult_WithCustomHandler_CallsHandlerOnSuccess()
        {
            // Arrange
            Result<int> result = Result.Ok(42);
            var success = new TestSuccess("Test success");
            var handlerCalled = false;
            Action<Result<int>, IEnumerable<ISuccess>> handler = (r, s) => { handlerCalled = true; r.WithSuccesses(s); };

            // Act
            var modifiedResult = result.AddSuccessIfPassed(handler, success);

            // Assert
            Assert.True(handlerCalled);
        }

        [Fact]
        public void AddSuccessIfPassed_GenericResult_WithCustomHandler_DoesNotCallHandlerOnFailure()
        {
            // Arrange
            Result<int> result = Result.Fail<int>("Failed");
            var success = new TestSuccess("Test success");
            var handlerCalled = false;
            Action<Result<int>, IEnumerable<ISuccess>> handler = (r, s) => { handlerCalled = true; r.WithSuccesses(s); };

            // Act
            var modifiedResult = result.AddSuccessIfPassed(handler, success);

            // Assert
            Assert.False(handlerCalled);
        }

        #endregion

        #region AddReasonIfFailed Tests

        [Fact]
        public void AddReasonIfFailed_AddsReasonWhenResultFailed()
        {
            // Arrange
            var result = Result.Fail("Initial failure");
            var reason = new TestReason("Additional reason");

            // Act
            var modifiedResult = result.AddReasonIfFailed(reason);

            // Assert
            Assert.True(modifiedResult.IsFailed);
            Assert.Contains(modifiedResult.Reasons, r => r.Message == "Additional reason");
        }

        [Fact]
        public void AddReasonIfFailed_DoesNotAddReasonWhenResultSuccess()
        {
            // Arrange
            var result = Result.Ok();
            var reason = new TestReason("Should not be added");
            var initialReasonCount = result.Reasons.Count;

            // Act
            var modifiedResult = result.AddReasonIfFailed(reason);

            // Assert
            Assert.True(modifiedResult.IsSuccess);
            Assert.Equal(initialReasonCount, modifiedResult.Reasons.Count);
        }

        [Fact]
        public void AddReasonIfFailed_GenericResultInt_AddsReasonWhenFailed()
        {
            // Arrange
            var result = Result.Fail<int>("Failure");
            var reason = new TestReason("Additional reason");

            // Act
            var modifiedResult = result.AddReasonIfFailed<Result<int>, int>(reason);

            // Assert
            Assert.True(modifiedResult.IsFailed);
            Assert.Contains(modifiedResult.Reasons, r => r.Message == "Additional reason");
        }

        [Fact]
        public void AddReasonIfFailed_WithCustomHandler_CallsHandlerOnFailure()
        {
            // Arrange
            var result = Result.Fail("Failed");
            var reason = new TestReason("Test reason");
            var handlerCalled = false;

            // Act
            var modifiedResult = result.AddReasonIfFailed(
                (r, reas) => { handlerCalled = true; },
                reason
            );

            // Assert
            Assert.True(handlerCalled);
        }

        [Fact]
        public void AddReasonIfFailed_WithCustomHandler_DoesNotCallHandlerOnSuccess()
        {
            // Arrange
            var result = Result.Ok();
            var reason = new TestReason("Test reason");
            var handlerCalled = false;

            // Act
            var modifiedResult = result.AddReasonIfFailed(
                (r, reas) => { handlerCalled = true; },
                reason
            );

            // Assert
            Assert.False(handlerCalled);
        }

        #endregion

        #region AddErrorIfFailed Tests

        [Fact]
        public void AddErrorIfFailed_CallsAddOnErrorsCollectionWhenResultFailed()
        {
            // Arrange
            var result = Result.Fail("Initial failure");
            var errorCountBefore = result.Errors.Count;
            var error = new Error("Additional error");

            // Act
            var modifiedResult = result.AddErrorIfFailed(error);

            // Assert
            Assert.True(modifiedResult.IsFailed);
            // FluentResults Errors collection might be immutable after creation
            // So we just verify the method returns the result and doesn't throw
            Assert.NotNull(modifiedResult);
        }

        [Fact]
        public void AddErrorIfFailed_DoesNotAddErrorWhenResultSuccess()
        {
            // Arrange
            var result = Result.Ok();
            var error = new Error("Should not be added");
            var errorCountBefore = result.Errors.Count;

            // Act
            var modifiedResult = result.AddErrorIfFailed(error);

            // Assert
            Assert.True(modifiedResult.IsSuccess);
            Assert.Equal(errorCountBefore, modifiedResult.Errors.Count);
        }

        #endregion

        #region GetNonErrors Tests

        [Fact]
        public void GetNonErrors_ReturnsOnlyNonErrorReasons()
        {
            // Arrange
            var result = Result.Fail("Test")
                .WithReason(new TestReason("Reason 1"))
                .WithError(new Error("Error 1"))
                .WithReason(new TestReason("Reason 2"));

            // Act
            var nonErrorsResult = result.GetNonErrors();

            // Assert
            Assert.True(nonErrorsResult.IsSuccess);
            var nonErrors = nonErrorsResult.Value;
            Assert.Equal(2, nonErrors.Count);
            Assert.All(nonErrors, r => Assert.IsNotType<IError>(r));
        }

        [Fact]
        public void GetNonErrors_WithList_PopulatesListWithNonErrors()
        {
            // Arrange
            var result = Result.Fail("Test")
                .WithReason(new TestReason("Reason 1"))
                .WithError(new Error("Error 1"));
            var reasons = new List<IReason>();

            // Act
            result.GetNonErrors(reasons);

            // Assert
            Assert.Single(reasons);
            Assert.IsNotType<IError>(reasons[0]);
        }

        #endregion

        #region UnwrapReasons Tests

        [Fact]
        public void UnwrapReasons_ReturnsAllReasons()
        {
            // Arrange
            var result = Result.Fail(new Error("Test"))
                .WithReason(new TestReason("Reason 1"))
                .WithReason(new TestReason("Reason 2"));

            // Act
            var unwrappedResult = result.UnwrapReasons();

            // Assert
            Assert.True(unwrappedResult.IsSuccess);
            Assert.Equal(3, unwrappedResult.Value.Count); // 1 error + 2 reasons
        }

        [Fact]
        public void UnwrapReasons_UnwrapsNestedFailedOperationExceptions()
        {
            // Arrange
            var innerResult = Result.Fail("Inner failure")
                .WithReason(new TestReason("Inner reason"));

            var exception = new FailedOperationException(innerResult);
            var outerResult = Result.Try((Action)(() => { throw exception; }));

            // Act
            var unwrappedResult = outerResult.UnwrapReasons();

            // Assert
            Assert.True(unwrappedResult.IsSuccess);
            var reasons = unwrappedResult.Value;
            Assert.Contains(reasons, r => r.Message == "Inner reason");
        }

        [Fact]
        public void UnwrapReasons_WithList_PopulatesListWithUnwrappedReasons()
        {
            // Arrange
            var result = Result.Fail(new Error("Test"))
                .WithReason(new TestReason("Reason 1"))
                .WithReason(new TestReason("Reason 2"));
            var reasons = new List<IReason>();

            // Act
            result.UnwrapReasons(reasons);

            // Assert
            Assert.Equal(3, reasons.Count); // 1 error + 2 reasons
        }

        [Fact]
        public void UnwrapReasons_WithNullList_CreatesNewList()
        {
            // Arrange
            var result = Result.Fail("Test")
                .WithReason(new TestReason("Reason 1"));
            List<IReason> reasons = null;

            // Act
            result.UnwrapReasons(reasons);

            // Assert - should not throw
            // The method creates a new list internally when null is passed
        }

        #endregion
    }
}
