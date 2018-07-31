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

        [Theory]
        [InlineData("quit", "Do I look like a Unix console?")]
        [InlineData("exit", "Do I look like a Shell console?")]
        [InlineData("what is sense of life?", "42. Read an Adam's book..")]
        public void FindsAnswerTest(string question, string expectedAnswer)
        {
            // Align
            Mind mind = Mind.Instance;
            // Act
            String answer = mind.Respond(question);
            // Assert
            Assert.Equal(answer, expectedAnswer);
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
