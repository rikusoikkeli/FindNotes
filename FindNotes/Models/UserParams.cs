using FindNotes.Interfaces;
using FindNotes.Models.Parameters;
using System.Collections.Generic;
using System.Linq;

namespace FindNotes.Models
{
    /// <summary>
    /// Comprises the arguments the user has inputted. Also offers related functionality like fetching 
    /// the value of a specific type of parameter when asked. Or computing the operation to perform on 
    /// the set of given arguments.
    /// </summary>
    public class UserParams
    {
        public List<Parameter> Parameters { get; set; }

        public FindNotesOperation OperationToPerform
        {
            get { return GetOperationToPerform(); }
        }

        public UserParams()
        {
            Parameters = new List<Parameter>() { };
        }

        private FindNotesOperation GetOperationToPerform()
        {
            var query = GetFirstParameterOfType<Query>();
            var path = GetFirstParameterOfType<Path>();
            if (ParameterHasValue(query) && ParameterHasValue(path))
            {
                return FindNotesOperation.Search;
            }

            var nickname = GetFirstParameterOfType<Nickname>();
            var savePath = GetFirstParameterOfType<SavePath>();
            if (ParameterHasValue(nickname) && ParameterHasValue(savePath))
            {
                return FindNotesOperation.SavePath;
            }

            var showSavedPaths = GetFirstParameterOfType<ListSavedPaths>();
            if (ParameterHasValue(showSavedPaths))
            {
                return FindNotesOperation.DisplaySavedPaths;
            }

            return FindNotesOperation.None;
        }

        public T GetFirstParameterOfType<T>() where T : Parameter
        {
            var param = Parameters.FirstOrDefault(p => p.GetType() == typeof(T));
            return (T)param;
        }

        private bool ParameterHasValue<T>(T param) where T : Parameter
        {
            if (param == null)
            {
                return false;
            }

            if (param is IParameterWithStringValue)
            {
                var value = ((IParameterWithStringValue)param).Value;
                if (!string.IsNullOrEmpty(value))
                {
                    return true;
                }
            }

            if (param is IParameterWithBoolValue)
            {
                var value = ((IParameterWithBoolValue)param).Value;
                if (value == true)
                {
                    return true;
                }
            }
            
            return false;
        }
    }
}
