using Sending.Domain.Players;
using Xunit;

namespace Sending.UnitTests.Domain
{
    public class PlayerTests
    {
        [Fact]
        public void Player_successfully_created()
        {
            // Arrange
            var expectedPlayer = new Player
            {
                FirstName = "Test_FirstName",
                LastName = "Test_LastName"
            };

            // Act
            var actualPlayer = new Player
            {
                FirstName = "Test_FirstName",
                LastName = "Test_LastName"
            };

            // Assert
            Assert.Equal(expectedPlayer.FirstName, actualPlayer.FirstName);
            Assert.Equal(expectedPlayer.LastName, actualPlayer.LastName);
        }
    }
}
