using FindNotes.Interfaces;
using FindNotes.Models;
using System.Diagnostics;

namespace FindNotes.Utils
{
    public class FileOpener : IFileOpener
    {
        /// <summary>
        /// Opens the text file from where a match was found using Notepad.
        /// </summary>
        /// <param name="match"></param>
        public void OpenFile(Line match)
        {
            var p = new Process();
            p.StartInfo = new ProcessStartInfo("notepad.exe", match.FilePath)
            {
                UseShellExecute = true
            };
            p.Start();
        }
    }
}