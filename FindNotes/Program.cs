using FindNotes.Interfaces;
using FindNotes.Models;
using FindNotes.Utils;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Runtime.InteropServices;

namespace FindNotes
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Setup DI
                var globalOptions = new GlobalOptions()
                {
                    ProgramName = "FindNotes.exe",
                    InTestMode = false,
                };
                var readWriteSettings = new ReadWriteSettings(globalOptions);
                var serviceProvider = new ServiceCollection()
                    .AddScoped<IFileOpener, FileOpener>()
                    .AddSingleton<IReadWriteSettings>(readWriteSettings)
                    .AddScoped<IOutput, Output>()
                    .AddSingleton<IGlobalOptions>(globalOptions)
                    .BuildServiceProvider();

                IntPtr ptr = GetCommandLine();
                string commandLine = Marshal.PtrToStringAuto(ptr) + "\n";

                var findNotes = new FindNotes(serviceProvider);
                findNotes.Run(commandLine);
            }
            catch (Exception e)
            {
                throw;
            }
        }

        /// <summary>
        /// We use this DLL import instead of args, because we need access to the raw command line string.
        /// Specifically, we need to be able to read quotes.
        /// </summary>
        /// <returns></returns>
        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr GetCommandLine();
    }
}