using System;

namespace YellowDuckyBot.Backend.Model
{
    // Represents a single retort item from responses tree.
    /// <summary>
    /// Represents a single retort item from responses tree.
    /// </summary>
    class Retort
    {
        public int Id { get; set; }
        public String Question { get; set; }
        public String Answer { get; set; }

        // Retruns concatenated form of retort to list it.
        /// <summary>
        /// Retruns concatenated form of retort to list it.
        /// </summary>
        /// <returns>id) retort line</returns>
        public string asStackEntry()
        {
            return $"{Id}) {Question}: {Answer}";
        }
    }
}
