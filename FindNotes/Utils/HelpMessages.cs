using FindNotes.Interfaces;
using FindNotes.Models;
using System;
using System.Text;
using System.Text.RegularExpressions;

namespace FindNotes.Utils
{
    /// <summary>
    /// The function of this class is to store help messages shown to the user in different scenarios.
    /// </summary>
    public class HelpMessages
    {
        private IOutput _output;
        private string _programName;

        public HelpMessages(IOutput output, IGlobalOptions globalOptions)
        {
            _output = output;
            _programName = globalOptions.ProgramName;
        }

        public void PrintNoParamsMessage()
        {
            string programNameFixed;

            var regex = new Regex(RegexPatterns.FilenameExtension);
            programNameFixed = regex.IsMatch(_programName) ? regex.Split(_programName)[0] : null;

            if (String.IsNullOrEmpty(programNameFixed))
            {
                throw new ArgumentNullException();
            }

            using (NotesWriter writer = new NotesWriter(_output))
            {
                var message = new StringBuilder();
                message.AppendLine($"usage: {_programName}");
                message.AppendLine("\t[-q]\t[-query]\tThe search word e.g. \"foo\"");
                message.AppendLine("\t[-p]\t[-path]\t\tThe directory from which to look e.g. \"C:\\folder\\subfolder\"");
                message.AppendLine("\t[-s]\t[-save]\t\tSave a directory to user later e.g. \"C:\\folder\\subfolder\"");
                message.AppendLine("\t[-n]\t[-nickname]\tWhen saving a directory, give it a nickname e.g. \"project1\"");
                message.AppendLine("\t[-l]\t[-list]\t\tList all saved directories e.g. \"-l savedPaths\"");
                Console.WriteLine(message);
            }
        }

        public void PrintNoMatchesFoundMessage()
        {
            using (NotesWriter writer = new NotesWriter(_output))
            {
                var message = new StringBuilder();
                message.Append("\nNo matches found!\n");
                Console.WriteLine(message);
            }
        }

        public void PrintSavedSettingsSuccessfully()
        {
            using (NotesWriter writer = new NotesWriter(_output))
            {
                var message = new StringBuilder();
                message.Append("\nSaved settings successfully!\n");
                Console.WriteLine(message);
            }
        }

        public void ListSavedPaths(Settings settings)
        {
            if (settings.SavedPaths != null && settings.SavedPaths.Count > 0)
            {
                foreach (var path in settings.SavedPaths)
                {
                    using (NotesWriter writer = new NotesWriter(_output))
                    {
                        var line = new StringBuilder();
                        line.Append($"{path.Key}: \"{path.Value}\"");
                        Console.WriteLine(line);
                    }
                }
            }
            else
            {
                using (NotesWriter writer = new NotesWriter(_output))
                {
                    var message = new StringBuilder();
                    message.Append("\nNo saved paths exist!\n");
                    Console.WriteLine(message);
                }
            }
        }
    }
}