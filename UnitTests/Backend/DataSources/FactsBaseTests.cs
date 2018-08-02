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
        [InlineData(null, " ")]
        [InlineData(null, "   ")]
        [InlineData("", null)]
        [InlineData("", "")]
        [InlineData("", " ")]
        [InlineData("", "   ")]
        [InlineData(" ", null)]
        [InlineData(" ", "")]
        [InlineData(" ", " ")]
        [InlineData(" ", "   ")]
        [InlineData("   ", null)]
        [InlineData("   ", "")]
        [InlineData("   ", " ")]
        [InlineData("   ", "   ")]
        public void AddWrongTest(string key, string value)
        {
            // Align
            var facts = new FactsBase();
            // Act
            var resultOfAdd = facts.Add(key, value);
            // Assert
            Assert.False(resultOfAdd);
        }
        
        [Fact]
        public void AddKeepsCasesTest()
        {
            // Align
            var facts = new FactsBase();
            var today = DateTime.Today;
            var coolCaps = "SuperDuperYellowDuckyBot";
            // Act
            var resultOfAdd = facts.Add(today.DayOfYear.ToString(), coolCaps);           
            var resultOfRead = facts.Read(today.DayOfYear.ToString());
            // Assert
            Assert.Equal(coolCaps, resultOfRead);
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
        public void CountsTest()
        {
            // Align
            var facts = new FactsBase();
            var today = DateTime.Today;
            // Act
            var countBeforeAdd = facts.Count();
            var resultOfAdd = facts.Add(today.DayOfWeek.ToString(), today.ToString());
            var countAfterAdd = facts.Count();
            var resultOfRead = facts.Read(today.DayOfWeek.ToString());
            var countAfterRead = facts.Count();
            var resultOfRemove = facts.Remove(today.DayOfWeek.ToString());
            var countAfterRemoval = facts.Count();
            // Assert
            Assert.NotEqual(countBeforeAdd, countAfterAdd);
            Assert.NotEqual(countAfterRead, countAfterRemoval);
            Assert.Equal(countBeforeAdd, countAfterRemoval);
            Assert.Equal(countAfterAdd, countAfterRead);
        }
        
        [Fact]
        public void ExistsTest()
        {
            // Align
            var facts = new FactsBase();
            var today = DateTime.Today;
            // Act
            var resultOfAdd = facts.Add(today.DayOfYear.ToString(), today.ToString());
            var actual = facts.Exists(today.DayOfYear.ToString(), today.ToString());
            
            // Assert
            Assert.True(actual);
        }
        
        [Theory]
        [InlineData(null, null)]
        [InlineData(null, "")]
        [InlineData(null, " ")]
        [InlineData(null, "   ")]
        // TODO Add missing cases
        [InlineData("NonExistingFact", null)]
        [InlineData("NonExistingFact", " ")]
        [InlineData("NonExistingFact", "   ")]
        [InlineData("NonExistingFact", "itsvalue")]
        public void ExistsWrongTest(string key, string value)
        {
            // Align
            var facts = new FactsBase();
            // Act
            var resultOfAdd = facts.Add("test_key_1", "test_value_1");
            var actual = facts.Exists(key, value);
            
            // Assert
            Assert.False(actual);
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
    }
}