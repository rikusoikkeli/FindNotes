using System;

namespace FindNotes.Models
{
    /// <summary>
    /// Line comprises a match for a given query and information relevant to it. Lines are printed 
    /// to the console as a result of a query.
    /// </summary>
    public class Line
    {
        public int Index { get; set; }
        public int Row { get; set; }
        public string Text { get; set; }
        public string FileName { get; set; }
        public string Query { get; set; }
        public string FilePath { get; set; }
        public DateTime LastChanged { get; set; }
    }
}
