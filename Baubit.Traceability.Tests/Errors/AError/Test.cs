using FluentResults;

namespace Baubit.Traceability.Tests.Errors.AError
{
    public class Test
    {
        private class TestError :Traceability.Errors.AError
        {
            public TestError() : base() { }

            public TestError(List<IError> reasons, string message, Dictionary<string, object> metadata)
                : base(reasons, message, metadata)
            {
            }
        }

        [Fact]
        public void DefaultConstructor_CreatesErrorWithEmptyValues()
        {
            // Arrange & Act
            var error = new TestError();

            // Assert
            Assert.NotNull(error);
            Assert.Empty(error.Reasons);
            Assert.Equal(string.Empty, error.Message);
            Assert.NotNull(error.Metadata);
            Assert.NotNull(error.CreationTime);
        }

        [Fact]
        public void ParameterizedConstructor_SetsAllProperties()
        {
            // Arrange
            var reasons = new List<IError> { new Error("reason1"), new Error("reason2") };
            var message = "Test error message";
            var metadata = new Dictionary<string, object> { { "key1", "value1" }, { "key2", 42 } };

            // Act
            var error = new TestError(reasons, message, metadata);

            // Assert
            Assert.Equal(2, error.Reasons.Count);
            Assert.Equal(message, error.Message);
            Assert.Equal(metadata, error.Metadata);
            Assert.NotNull(error.CreationTime);
        }

        [Fact]
        public void ParameterizedConstructor_HandlesNullReasons()
        {
            // Arrange
            List<IError> reasons = null;
            var message = "Test message";
            var metadata = new Dictionary<string, object>();

            // Act
            var error = new TestError(reasons, message, metadata);

            // Assert
            Assert.NotNull(error.Reasons);
            Assert.Empty(error.Reasons);
        }

        [Fact]
        public void CreationTime_IsSetAutomatically()
        {
            // Arrange
            var before = DateTime.Now.AddSeconds(-1);

            // Act
            var error = new TestError();
            var after = DateTime.Now.AddSeconds(1);

            // Assert
            Assert.True(error.CreationTime >= before);
            Assert.True(error.CreationTime <= after);
        }
    }
}
