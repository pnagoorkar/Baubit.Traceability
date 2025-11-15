using FluentResults;

namespace Baubit.Traceability.Tests.Exceptions.FailedOperationException
{
    public class Test
    {
        [Fact]
        public void Constructor_StoresResultAndSetsMessage()
        {
            // Arrange
            var result = Result.Fail("Test failure");

            // Act
            var exception = new Traceability.Exceptions.FailedOperationException(result);

            // Assert
            Assert.NotNull(exception.Result);
            Assert.Equal(result, exception.Result);
            Assert.Contains("Test failure", exception.Message);
        }

        [Fact]
        public void Constructor_WorksWithSuccessResult()
        {
            // Arrange
            var result = Result.Ok();

            // Act
            var exception = new Traceability.Exceptions.FailedOperationException(result);

            // Assert
            Assert.NotNull(exception.Result);
            Assert.Equal(result, exception.Result);
        }

        [Fact]
        public void Constructor_WorksWithGenericResult()
        {
            // Arrange
            var result = Result.Fail<int>("Generic failure");

            // Act
            var exception = new Traceability.Exceptions.FailedOperationException(result);

            // Assert
            Assert.NotNull(exception.Result);
            Assert.Equal(result, exception.Result);
            Assert.Contains("Generic failure", exception.Message);
        }
    }
}
