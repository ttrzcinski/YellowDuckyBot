using Xunit;
using YellowDuckyBot.Backend;

namespace UnitTests.Backend
{
    /// <summary>
    /// Tests of class Mind.
    /// </summary>
    public class MindTests
    {
        [Fact]
        public void LoadsJSONTest()
        {
            // Align
            Mind mind = Mind.Instance;
            // Act
            //
            // Assert
            Assert.NotEqual(mind.CountRetorts(), 0);
        }
    }
}
