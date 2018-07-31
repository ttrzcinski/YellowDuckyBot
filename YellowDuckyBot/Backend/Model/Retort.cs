using System;

namespace YellowDuckyBot.Backend.Model
{
    // Represents a single retort item from responses tree.
    /// <summary>
    /// Represents a single retort item from responses tree.
    /// </summary>
    internal class Retort
    {
        public int Id { get; set; }
        public String Question { get; set; }
        public String Answer { get; set; }

        // Returns concatenated form of retort to list it.
        /// <summary>
        /// Returns concatenated form of retort to list it.
        /// </summary>
        /// <returns>id) retort line</returns>
        public string AsStackEntry()
        {
            return $"{Id}) {Question}: {Answer}";
        }
    }
}
