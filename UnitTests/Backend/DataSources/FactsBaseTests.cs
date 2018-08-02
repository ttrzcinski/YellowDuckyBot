using System;
using Xunit;
using YellowDuckyBot.Backend;
using YellowDuckyBot.Backend.DataSources;

namespace UnitTests.Backend.DataSources
{
    /// <summary>
    /// Tests of class Mind.
    /// </summary>
    public class FactsBaseTests
    {
        [Fact]
        public void AddCountTest()
        {
            // Align
            var facts = new FactsBase();
            var today = DateTime.Today;
            // Act
            var countBefore = facts.Count();
            var resultOfAdd = facts.Add(today.DayOfYear.ToString(), today.ToString());
            var countAfter = facts.Count();
            // Assert
            Assert.True(countBefore < countAfter);
        }
        
        [Fact]
        public void AddConfirmationTest()
        {
            // Align
            var facts = new FactsBase();
            var today = DateTime.Today;
            // Act
            var resultOfAdd = facts.Add(today.DayOfYear.ToString(), today.ToString());
            // Assert
            Assert.True(resultOfAdd);
        }
        
        [Theory]
        [InlineData(null, null)]
        [InlineData(null, "")]
        [InlineData("", null)]
        public void AddWrongTest(string key, string value)
        {
            //TODO Maybe add also white spaces
            // Align
            var facts = new FactsBase();
            // Act
            var resultOfAdd = facts.Add(key, value);
            // Assert
            Assert.False(resultOfAdd);
        }
        
        [Fact]
        public void ReadValueTest()
        {
            // Align
            var facts = new FactsBase();
            var today = DateTime.Today;
            // Act
            var resultOfAdd = facts.Add(today.DayOfYear.ToString(), today.ToString());
            var actual = facts.Read(today.DayOfYear.ToString());
            
            // Assert
            Assert.Equal(today.ToString(), actual);
        }
        
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(":")]
        [InlineData("NonExistingFact")]
        public void ReadWrongKeyTest(string key)
        {
            // Align
            var facts = new FactsBase();
            var today = DateTime.Today;
            // Act
            var actual = facts.Read(key);
            
            // Assert
            Assert.Null(actual);
        }
        
        [Fact]
        public void RemoveConfirmationTest()
        {
            // Align
            var facts = new FactsBase();
            var today = DateTime.Today;
            // Act
            var resultOfAdd = facts.Add(today.DayOfYear.ToString(), today.ToString());
            var countBefore = facts.Count();
            var resultOfRemoval = facts.Remove(today.DayOfYear.ToString());
            var countAfter = facts.Count();
            // Assert
            Assert.True(resultOfRemoval);
        }
        
        [Fact]
        public void RemoveCountTest()
        {
            // Align
            var facts = new FactsBase();
            var today = DateTime.Today;
            // Act
            var resultOfAdd = facts.Add(today.DayOfYear.ToString(), today.ToString());
            var countBefore = facts.Count();
            var resultOfRemoval = facts.Remove(today.DayOfYear.ToString());
            var countAfter = facts.Count();
            // Assert
            Assert.True(countBefore > countAfter);
        }
        
        [Fact]
        public void AddAndOverrideTest()
        {
            // Align
            var facts = new FactsBase();
            var today = DateTime.Today;
            // Act
            var resultOfAdd = facts.Add(today.DayOfYear.ToString(), today.ToString());
            var oldValue = facts.Read(today.DayOfYear.ToString());
            var resultOfOverride = facts.Add(today.DayOfYear.ToString(), today.ToLongDateString());
            var laterValue = facts.Read(today.DayOfYear.ToString());
            
            // Assert
            Assert.True(resultOfAdd);
            Assert.True(resultOfOverride);
            Assert.NotEqual(oldValue, laterValue);
        }
        
        [Fact]
        public void AddReadAndRemoveTest()
        {
            // Align
            var facts = new FactsBase();
            var today = DateTime.Today;
            // Act and Assert
            var resultOfAdd = facts.Add(today.DayOfWeek.ToString(), today.ToString());
            Assert.True(resultOfAdd);
            
            var resultOfRead = facts.Read(today.DayOfWeek.ToString());
            Assert.NotNull(resultOfRead);
            
            var resultOfRemove = facts.Remove(today.DayOfWeek.ToString());
            Assert.True(resultOfRemove);
        }

        /*[Theory]
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
        }*/
    }
}