using FindNotes.Interfaces;
using FindNotes.Models;
using Newtonsoft.Json;
using System.IO;
using System.Text;

namespace FindNotes.Utils
{
    /// <summary>
    /// Reads and writes settings to a JSON file called settings.json.
    /// </summary>
    public class ReadWriteSettings : IReadWriteSettings
    {
        private string _path = "settings.json";
        private string _testPath = "testsettings.json";
        private bool _inTestMode;

        public ReadWriteSettings(IGlobalOptions globalOptions)
        {
            _inTestMode = globalOptions.InTestMode;
        }

        public Settings ReadSettings()
        {
            StringBuilder serialisedSettings = new StringBuilder();

            if (!File.Exists(_inTestMode ? _testPath : _path))
            {
                ResetSettings();
            }

            using (StreamReader sr = new StreamReader(_inTestMode ? _testPath : _path))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    serialisedSettings.Append(line);
                }
            }

            return JsonConvert.DeserializeObject<Settings>(serialisedSettings.ToString());
        }

        public void SaveSettings(Settings settings)
        {
            if (File.Exists(_inTestMode ? _testPath : _path))
            {
                string s = JsonConvert.SerializeObject(settings);
                using (StreamWriter sw = File.CreateText(_inTestMode ? _testPath : _path))
                {
                    sw.WriteLine(s);
                }
            }
        }

        public bool ResetSettings()
        {
            var s = new Settings();
            string settings = JsonConvert.SerializeObject(s);
            using (StreamWriter sw = File.CreateText(_inTestMode ? _testPath : _path))
            {
                sw.WriteLine(settings);
            }
            return true;
        }
    }
}
