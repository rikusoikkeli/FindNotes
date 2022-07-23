using FindNotes.Interfaces;

namespace FindNotes.Models.Parameters
{
    public class ListSavedPaths : Parameter, IParameterWithBoolValue
    {
        public bool Value { get; init; }

        public ListSavedPaths(bool value)
        {
            Value = value;
        }
    }
}
