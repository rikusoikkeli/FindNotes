using FindNotes.Interfaces;
using FindNotes.Models;
using FindNotes.Models.Parameters;
using System.Text.RegularExpressions;

namespace FindNotes.Utils
{
    /// <summary>
    /// Parses the command line text into arguments.
    /// </summary>
    public class CmdParser
    {
        private IReadWriteSettings _readWriteSettings;
        private string _programName;

        public CmdParser(IReadWriteSettings readWriteSettings, IGlobalOptions globalOptions)
        {
            _readWriteSettings = readWriteSettings;
            _programName = globalOptions.ProgramName;
        }

        public UserParams FetchArguments(string cmdLine)
        {
            var userParams = new UserParams();

            var cmdLineArray = cmdLine.Split(_programName);
            string command = cmdLineArray[cmdLineArray.Length - 1]; // e.g. -query foo -path c:some\folder

            string path = FetchPath(command);
            string query = FetchQuery(command);
            string savePath = FetchSavePath(command);
            string nickname = FetchNickName(command);
            bool listSavedPaths = FetchListSavedPaths(command);

            if (!string.IsNullOrEmpty(path)) { userParams.Parameters.Add(new Path(path)); }
            if (!string.IsNullOrEmpty(query)) { userParams.Parameters.Add(new Query(query)); }
            if (!string.IsNullOrEmpty(savePath)) { userParams.Parameters.Add(new SavePath(savePath)); }
            if (!string.IsNullOrEmpty(nickname)) { userParams.Parameters.Add(new Nickname(nickname)); }
            if (listSavedPaths == true) { userParams.Parameters.Add(new ListSavedPaths(true)); }

            return userParams;
        }

        private bool FetchListSavedPaths(string command)
        {
            bool result = false;
            string listKeyPattern = RegexPatterns.ListParameterName;
            string listValuePattern = RegexPatterns.ListSavedPaths;
            var regex = new Regex(listKeyPattern);

            if (regex.IsMatch(command))
            {
                regex = new Regex(listValuePattern);
                result = regex.IsMatch(command);
            }

            return result;
        }

        private string FetchNickName(string command)
        {
            string result = "";
            string nicknameKeyPattern = RegexPatterns.NicknameParameterName;
            string nicknameValuePattern = RegexPatterns.NicknameWithOrWithoutQuotes;
            var regex = new Regex(nicknameKeyPattern);

            if (regex.IsMatch(command))
            {
                regex = new Regex(nicknameValuePattern);
                result = regex.Match(command).ToString();
            }

            return result.Trim().Trim('"');
        }

        private string FetchSavePath(string command)
        {
            string result = "";
            string savePathKeyPattern = RegexPatterns.SavePathParameterName;
            string savePathValuePattern = RegexPatterns.SavePathWithOrWithoutQuotes;
            var regex = new Regex(savePathKeyPattern);

            if (regex.IsMatch(command))
            {
                regex = new Regex(savePathValuePattern);
                result = regex.Match(command).ToString();
            }

            return result.Trim().Trim('"');
        }

        private string FetchPath(string command)
        {
            string result = "";
            bool success = false;
            string pathKeyPattern = RegexPatterns.PathParameterName;
            var regex = new Regex(pathKeyPattern);

            // Fetch path using a shortcut
            if (regex.IsMatch(command))
            {
                string pathNicknamePattern = RegexPatterns.PathNicknameValueWithOrWithoutQuotes;
                regex = new Regex(pathNicknamePattern);
                string pathNicknameKey = regex.Match(command).ToString();
                string pathNickNameValue = "";
                var rw = _readWriteSettings.ReadSettings();
                if (rw.SavedPaths != null)
                {
                    success = rw.SavedPaths.TryGetValue(pathNicknameKey.Trim(), out pathNickNameValue);
                }
                if (success && !string.IsNullOrEmpty(pathNickNameValue))
                {
                    return pathNickNameValue;
                }
            }

            // Fetch path in the normal way
            string pathValuePattern = RegexPatterns.PathParameterValueWithQuotes;

            if (regex.IsMatch(command))
            {
                regex = new Regex(pathValuePattern);
                result = regex.Match(command).ToString();
            }

            if (!string.IsNullOrEmpty(result))
            {
                return result.Trim().Trim('"');
            }

            pathValuePattern = RegexPatterns.PathParameterValueWithoutQuotes;
            regex = new Regex(pathKeyPattern);

            if (regex.IsMatch(command))
            {
                regex = new Regex(pathValuePattern);
                result = regex.Match(command).ToString();
            }

            return result.Trim().Trim('"');
        }

        private string FetchQuery(string command)
        {
            string result = "";
            string pattern = RegexPatterns.QueryParameterValueAndNameWithQuotes;
            var regex = new Regex(pattern);

            if (regex.IsMatch(command))
            {
                result = regex.Matches(command)[0].Value.Replace("-query", "").Replace("-q", "");
                return result.Trim().Trim('"');
            }

            pattern = RegexPatterns.QueryParameterValueAndNameWithoutQuotes;
            regex = new Regex(pattern);

            if (regex.IsMatch(command))
            {
                result = regex.Matches(command)[0].Value.Replace("-query", "").Replace("-q", "");
                return result.Trim().Trim('"');
            }

            return "";
        }
    }
}
