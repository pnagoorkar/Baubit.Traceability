using FluentResults;

namespace Baubit.Traceability.Tests.Successes.ASuccess
{
    public class Test
    {
        private class TestSuccess : Traceability.Successes.ASuccess
        {
            public TestSuccess() : base() { }

            public TestSuccess(string message, Dictionary<string, object> metadata)
                : base(message, metadata)
            {
            }
        }

        [Fact]
        public void DefaultConstructor_CreatesSuccessWithEmptyValues()
        {
            // Arrange & Act
            var success = new TestSuccess();

            // Assert
            Assert.NotNull(success);
            Assert.Equal(string.Empty, success.Message);
            Assert.NotNull(success.Metadata);
            Assert.NotNull(success.CreationTime);
        }

        [Fact]
        public void ParameterizedConstructor_SetsAllProperties()
        {
            // Arrange
            var message = "Test success message";
            var metadata = new Dictionary<string, object> { { "key1", "value1" }, { "key2", 42 } };

            // Act
            var success = new TestSuccess(message, metadata);

            // Assert
            Assert.Equal(message, success.Message);
            Assert.Equal(metadata, success.Metadata);
            Assert.NotNull(success.CreationTime);
        }

        [Fact]
        public void ASuccess_ImplementsISuccess()
        {
            // Arrange
            var success = new TestSuccess();

            // Assert
            Assert.IsAssignableFrom<ISuccess>(success);
        }

        [Fact]
        public void ToString_ReturnsMessage()
        {
            // Arrange
            var message = "Test success message";
            var success = new TestSuccess(message, new Dictionary<string, object>());

            // Act
            var result = success.ToString();

            // Assert
            Assert.Equal(message, result);
        }
    }
}
