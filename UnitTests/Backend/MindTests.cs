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
        public void LoadsJsonTest()
        {
            // Align
            var mind = Mind.Instance;
            // Act
            //
            // Assert
            Assert.NotEqual(0, mind.CountRetorts());
        }

        [Theory]
        [InlineData("quit", "Do I look like a Unix console?")]
        [InlineData("exit", "Do I look like a Shell console?")]
        [InlineData("what is sense of life?", "42. Read an Adam's book..")]
        public void FindsAnswerTest(string question, string expectedAnswer)
        {
            // Align
            var mind = Mind.Instance;
            // Act
            var answer = mind.Respond(question);
            // Assert
            Assert.Equal(answer, expectedAnswer);
        }

        [Fact]
        public void FindNonExistingAnswerTest()
        {
            // Align
            var mind = Mind.Instance;
            // Act
            var answer = mind.Respond("quit222");
            // Assert
            Assert.Null(answer);
        }
        
        [Theory]
        [InlineData(null, null)]
        [InlineData("smt", null)]
        [InlineData(null, "smt")]
        public void AddRetortWithNullsTest(string question, string answer)
        {
            // Align
            var mind = Mind.Instance;
            // Act
            var actual = mind.AddRetort(question, answer);
            // Assert
            Assert.False(actual);
        }
        
        [Fact]
        public void CountRetortsAreSomeTest()
        {
            // Align
            var mind = Mind.Instance;
            // Act
            var actual = mind.CountRetorts();
            // Assert
            Assert.NotEqual(0, actual);
        }
        
        [Fact]
        public void RetortsMaxIdHasValueTest()
        {
            // Align
            var mind = Mind.Instance;
            // Act
            var actual = mind.RetortsMaxId();
            // Assert
            Assert.NotEqual(0, actual);
        }
        
        //refreshRetortMaxId
        [Fact]
        public void RetortsMaxIdHasRightValueTest()
        {
            // Align
            var mind = Mind.Instance;
            var now = DateTime.Now.ToString("yyyyMMddHHmmssffff");
            // Act
            var valueBefore = mind.RetortsMaxId();
            var added = mind.AddRetort("smt1", now);
            var actual = mind.RetortsMaxId();
            // Assert
            Assert.True(added);
            Assert.Equal(valueBefore, actual - 1);
        }
    }
}
