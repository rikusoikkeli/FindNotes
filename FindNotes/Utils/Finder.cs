using FindNotes.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FindNotes.Utils
{
    /// <summary>
    /// Contains the logic for browsing through files and finding text.
    /// </summary>
    public static class Finder
    {
        public static List<Line> FindMatchesFromDir(string query, string dirPath)
        {
            var matches = new List<Line>() { };

            var files = new DirectoryInfo(dirPath)
                .GetFiles()
                .Where(f => f.Name.EndsWith(".txt"))
                .OrderByDescending(f => f.LastWriteTime)
                .ToList();

            foreach (var file in files)
            {
                    var tempMatches = FindMatchesFromFile(query, file, matches.Count);
                    if (tempMatches.Count > 0)
                    {
                        matches.AddRange(tempMatches);
                    }
            }
            return matches;
        }

        private static List<Line> FindMatchesFromFile(string query, FileInfo file, int currentIteration)
        {
            string line;
            int count = 0;
            var matches = new List<Line>() { };
            using (StreamReader sr = new StreamReader(file.FullName))
            {
                while ((line = sr.ReadLine()) != null)
                {
                    count++;
                    if (line.ToLower().Contains(query))
                    {
                        var match = new Line();
                        match.Index = currentIteration++;
                        match.Row = count;
                        match.Text = line.ToLower().Trim();
                        match.FileName = file.Name;
                        match.Query = query;
                        match.FilePath = file.FullName;
                        match.LastChanged = file.LastWriteTime;
                        matches.Add(match);
                    }
                }
            }
            return matches;
        }
    }
}
