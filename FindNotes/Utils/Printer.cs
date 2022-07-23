using FindNotes.Interfaces;
using FindNotes.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FindNotes.Utils
{
    /// <summary>
    /// Contains the logic of producing output to the UI.
    /// </summary>
    public class Printer
    {
        private IOutput _output;
        private IFileOpener _fileOpener;
        private bool _inTestMode;

        public Printer(IOutput output, IFileOpener fileOpener, IGlobalOptions globalOptions)
        {
            _output = output;
            _fileOpener = fileOpener;
            _inTestMode = globalOptions.InTestMode;
        }

        public void StartPrinting(List<Line> matches)
        {
            if (!_inTestMode)
            {
                PrintSearchParams(matches.FirstOrDefault());
            }
            PrintMatches(matches);
        }

        private void PrintMatches(List<Line> matches)
        {
            PrintHeaders();

            int filenameMaxLength = 30;
            int textMaxLength = 80;
            int countInPage = 0;
            int totalCount = 0;

            foreach (var match in matches)
            {
                PrintLineExceptText(match, filenameMaxLength);
                PrintLineTextWithHightlighting(match, textMaxLength);

                countInPage++;
                totalCount++;

                if (totalCount > 0 && countInPage % 10 == 0 || totalCount == matches.Count)
                {
                    if (!_inTestMode)
                    {
                        WaitForInput(matches);
                    }
                    using (NotesWriter writer = new NotesWriter(_output))
                    {
                        Console.WriteLine();
                    }
                    countInPage = 0;
                }
            }
        }

        /// <summary>
        /// Stops the scrolling down of matches. User can give input and choose to scroll down further, 
        /// open a file or kill the program.
        /// </summary>
        /// <param name="matches"></param>
        /// <exception cref="OperationCanceledException"></exception>
        private void WaitForInput(List<Line> matches)
        {
            using (NotesWriter writer = new NotesWriter(_output))
            {
                Console.WriteLine("Press <Enter> to continue scrolling.");
                Console.WriteLine("Or input an index number to open the matching file.");
                Console.Write("Input: ");
            }

            while (true)
            {
                var input = Console.ReadLine();

                int index;
                char command;
                bool intSuccess = int.TryParse(input, out index);
                bool charSuccess = char.TryParse(input, out command);

                if (intSuccess)
                {
                    var match = matches.Where(m => m.Index == index).FirstOrDefault();
                    _fileOpener.OpenFile(match);
                    using (NotesWriter writer = new NotesWriter(_output))
                    {
                        Console.Write("Input: ");
                    }
                }

                if (charSuccess && command == 'q')
                {
                    throw new OperationCanceledException("User stopped the program.");
                }

                if (intSuccess == false && charSuccess == false)
                {
                    return;
                }
            }
        }

        /// <summary>
        /// Notifies the user which query parameters were used to perform the search operation.
        /// </summary>
        /// <param name="line"></param>
        private void PrintSearchParams(Line line)
        {
            var path = Path.GetDirectoryName(line.FilePath);
            var query = line.Query;

            using (NotesWriter writer = new NotesWriter(_output))
            {
                Console.WriteLine();
                Console.WriteLine($"Searching string: \"{query}\"");
                Console.WriteLine($"From path: \"{path}\"");
                Console.WriteLine();
            }
        }

        /// <summary>
        /// Prints headers to the console under which the matched lines are going to be printed.
        /// </summary>
        private void PrintHeaders()
        {
            using NotesWriter writer = new NotesWriter(_output);
            Console.WriteLine("{0,0}{1,0}{2,10}{3,10}{4,10}",
                "INDEX".PadRight(8, ' '),
                "DATE".PadRight(10, ' '),
                "FILE",
                "ROW".PadLeft(38, ' '),
                "TEXT"
            );
        }

        /// <summary>
        /// Prints the matched lines to the console excluding Text.
        /// </summary>
        /// <param name="match"></param>
        /// <param name="filenameMaxLength"></param>
        private void PrintLineExceptText(Line match, int filenameMaxLength)
        {
            using NotesWriter writer = new NotesWriter(_output);
            Console.Write("{0,0}{1,10}{2,10}{3,10}",
                match.Index.ToString() + "\t",
                match.LastChanged.ToShortDateString() + "\t",
                (match.FileName.Length > filenameMaxLength ? match.FileName.Substring(0, filenameMaxLength - 3) + "..." : match.FileName) + "\t",
                match.Row + "\t"
            );
        }

        /// <summary>
        /// Prints the Text portion of the matched lines to the console. Also adds highlight to the query parameter used.
        /// </summary>
        /// <param name="match"></param>
        /// <param name="textMaxLength"></param>
        private void PrintLineTextWithHightlighting(Line match, int textMaxLength)
        {
            string text;
            string part;
            var highlightColor = ConsoleColor.DarkMagenta;
            var defaultColor = ConsoleColor.Black;
            int numOfIterations = 1;
            var partsOfTheText = new List<string>() { };
            string newLine = "\t\r\n";

            text = match.Text.Length > textMaxLength ? 
                match.Text.Substring(0, textMaxLength - 3) + "..." : 
                match.Text;
            bool QueryIsOnNontruncatedPortionOfText = text.IndexOf(match.Query) >= 0 ? true : false;
            int indexOfQueryInText = text.IndexOf(match.Query);
            int lengthOfQuery = match.Query.Length;

            /* If the query param is visible within the line being printed, we need to print it in three parts
             * - Part before query without highlight
             * - Part with query with highlight
             * - Part after query without highlight
             */
            if (QueryIsOnNontruncatedPortionOfText)
            {
                numOfIterations = 3;
                string prequery = text.Substring(0, indexOfQueryInText);
                string query = match.Query;
                string postquery = text.Substring(
                    indexOfQueryInText + lengthOfQuery, 
                    text.Length - query.Length - prequery.Length
                    ) + newLine;
                partsOfTheText.AddRange(new List<string>() { prequery, query, postquery });
            }

            // Do the printing either in one or three parts (depending if the query param is visible or not)
            for (int i = 0; i < numOfIterations; i++)
            {
                part = partsOfTheText.Count == 3 ? partsOfTheText[i] : text + newLine;
                ConsoleColor colorToUse = i == 1 ? highlightColor : defaultColor;

                using NotesWriter writer = new NotesWriter(_output, colorToUse);
                Console.Write(part);
            }
        }
    }
}
