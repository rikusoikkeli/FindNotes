using FindNotes.Interfaces;
using System;
using System.IO;

namespace FindNotes.Tests
{
    /// <summary>
    /// Use this class to change the output from console to file for easier testing.
    /// </summary>
    public class WriteToFile : IOutput
    {
        public string FileName { get; set; }
        public string FileLocation = Directory.GetCurrentDirectory() + @"\TestData\TestOutputs";


        public WriteToFile()
        {
            var time = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
            FileName = $"\\testOutput_{time}.txt";
            CreateSaveFolder();
        }

        public void Send(string text)
        {
            using StreamWriter file = new(FileLocation + FileName, true);
            file.Write(text);
        }

        public void CreateSaveFolder()
        {
            bool directoryExists = Directory.Exists(FileLocation);
            if (!directoryExists)
            {
                Directory.CreateDirectory(FileLocation);
            }
        }
    }
}
