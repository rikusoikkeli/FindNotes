using System.Collections.Generic;

namespace FindNotes.Models
{
    /// <summary>
    /// Represents the settings the user has so far saved for themselves in memory.
    /// </summary>
    public class Settings
    {
        public Dictionary<string, string> SavedPaths;
    }
}
