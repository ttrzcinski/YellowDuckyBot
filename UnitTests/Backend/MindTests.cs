using System;
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

        [Fact]
        public void FindsAnswerTest()
        {
            // Align
            Mind mind = Mind.Instance;
            // Act
            String answer = mind.Respond("quit");
            // Assert
            Assert.Equal(answer, "Do I look like a Unix console?");
        }

        [Fact]
        public void FindNonwexistingAnswerTest()
        {
            // Align
            Mind mind = Mind.Instance;
            // Act
            String answer = mind.Respond("quit222");
            // Assert
            Assert.Null(answer);
        }
    }
}
