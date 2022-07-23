using FindNotes.Interfaces;
using FindNotes.Models;
using FindNotes.Models.Parameters;
using FindNotes.Utils;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace FindNotes
{
    /// <summary>
    /// The main class comprising the FindNotes program.
    /// </summary>
    public class FindNotes
    {
        private IOutput _output;
        private IGlobalOptions _globalOptions;
        private IReadWriteSettings _readWriteSettings;
        private IFileOpener _fileOpener;
        private HelpMessages _helpMessages;
        private CmdParser _cmdParser;
        private Printer _printer;

        public FindNotes(IServiceProvider serviceProvider)
        {
            _output = serviceProvider.GetService<IOutput>();
            _globalOptions = serviceProvider.GetService<IGlobalOptions>();
            _readWriteSettings = serviceProvider.GetService<IReadWriteSettings>();
            _fileOpener = serviceProvider.GetService<IFileOpener>();
            _helpMessages = new HelpMessages(_output, _globalOptions);
            _cmdParser = new CmdParser(_readWriteSettings, _globalOptions);
            _printer = new Printer(_output, _fileOpener, _globalOptions);
        }

        public void Run(string cmdLine)
        {
            var userParams = _cmdParser.FetchArguments(cmdLine);

            switch (userParams.OperationToPerform)
            {
                case FindNotesOperation.Search:
                    var query = userParams.GetFirstParameterOfType<Query>();
                    var path = userParams.GetFirstParameterOfType<Path>();
                    var results = Finder.FindMatchesFromDir(query.Value, path.Value);

                    if (results == null || results.Count < 1)
                    {
                        _helpMessages.PrintNoMatchesFoundMessage();
                        break;
                    }

                    _printer.StartPrinting(results);
                    break;

                case FindNotesOperation.SavePath:
                    var nickname = userParams.GetFirstParameterOfType<Nickname>();
                    var savePath = userParams.GetFirstParameterOfType<SavePath>();
                    SaveSetting(nickname.Value, savePath.Value);
                    _helpMessages.PrintSavedSettingsSuccessfully();
                    break;

                case FindNotesOperation.DisplaySavedPaths:
                    _readWriteSettings.ReadSettings();
                    _helpMessages.ListSavedPaths(_readWriteSettings.ReadSettings());
                    break;

                case FindNotesOperation.None:
                    _helpMessages.PrintNoParamsMessage();
                    break;

                default:
                    break;
            }
        }

        public void SaveSetting(string nickname, string savePath)
        {
            var s = _readWriteSettings.ReadSettings();

            if (s.SavedPaths == null)
            {
                s.SavedPaths = new Dictionary<string, string>();
            }

            bool containsKey = s.SavedPaths.ContainsKey(nickname);
            if (!containsKey)
            {
                s.SavedPaths.Add(nickname, savePath);
                _readWriteSettings.SaveSettings(s);
            }
        }
    }
}
