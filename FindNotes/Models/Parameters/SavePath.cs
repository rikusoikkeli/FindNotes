using FindNotes.Interfaces;

namespace FindNotes.Models.Parameters
{
    public class SavePath : Parameter, IParameterWithStringValue
    {
        public string Value { get; init; }

        public SavePath(string value)
        {
            Value = value;
        }
    }
}
