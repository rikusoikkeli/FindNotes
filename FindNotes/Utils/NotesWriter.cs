using FindNotes.Interfaces;
using System;
using System.IO;

namespace FindNotes.Utils
{
    /// <summary>
    /// Use with a using statement. All Console.WriteLine calls will be saved into Console memory.
    /// After closing bracket, the messages from memory will be sent into the IOutput object and the
    /// messages removed from memory.
    /// </summary>
    public class NotesWriter : IDisposable
    {
        private StringWriter _stringWriter = new StringWriter();
        private IOutput _output;
        private ConsoleColor _color;
        private ConsoleColor _defaultColor = ConsoleColor.Black;

        public NotesWriter(IOutput output)
        {
            _output = output;
            StartSavingConsoleMessagesToMemory();
        }

        public NotesWriter(IOutput output, ConsoleColor color)
        {
            _output = output;
            _color = color;
            StartSavingConsoleMessagesToMemory();
        }

        void StartSavingConsoleMessagesToMemory()
        {
            Console.SetOut(_stringWriter);
        }

        void ReleaseMessagesFromMemory()
        {
            Console.BackgroundColor = _color;
            _output.Send(_stringWriter.ToString());
        }

        void IDisposable.Dispose()
        {
            ReleaseMessagesFromMemory();
            Console.BackgroundColor = _defaultColor;
        }
    }
}
