using Baubit.Traceability.Reasons;

namespace Baubit.Traceability.Tests.Reasons
{
    public class AReasonTests
    {
        private class TestReason : AReason
        {
            public TestReason() : base() { }
            
            public TestReason(string message, Dictionary<string, object> metadata) 
                : base(message, metadata)
            {
            }
        }

        [Fact]
        public void DefaultConstructor_CreatesReasonWithEmptyValues()
        {
            // Arrange & Act
            var reason = new TestReason();

            // Assert
            Assert.NotNull(reason);
            Assert.Equal(string.Empty, reason.Message);
            Assert.NotNull(reason.Metadata);
            Assert.NotNull(reason.CreationTime);
        }

        [Fact]
        public void ParameterizedConstructor_SetsAllProperties()
        {
            // Arrange
            var message = "Test reason message";
            var metadata = new Dictionary<string, object> { { "key1", "value1" }, { "key2", 42 } };

            // Act
            var reason = new TestReason(message, metadata);

            // Assert
            Assert.Equal(message, reason.Message);
            Assert.Equal(metadata, reason.Metadata);
            Assert.NotNull(reason.CreationTime);
        }

        [Fact]
        public void ToString_ReturnsMessage()
        {
            // Arrange
            var message = "Test reason message";
            var reason = new TestReason(message, new Dictionary<string, object>());

            // Act
            var result = reason.ToString();

            // Assert
            Assert.Equal(message, result);
        }

        [Fact]
        public void CreationTime_IsSetAutomatically()
        {
            // Arrange
            var before = DateTime.Now.AddSeconds(-1);

            // Act
            var reason = new TestReason();
            var after = DateTime.Now.AddSeconds(1);

            // Assert
            Assert.True(reason.CreationTime >= before);
            Assert.True(reason.CreationTime <= after);
        }
    }
}
