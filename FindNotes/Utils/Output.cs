using FindNotes.Interfaces;
using System;
using System.IO;

namespace FindNotes.Utils
{
    public class Output : IOutput
    {
        private TextWriter _console = Console.Out;

        public void Send(string text)
        {
            _console.Write(text);
        }
    }
}
