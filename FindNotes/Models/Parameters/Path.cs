using FindNotes.Interfaces;

namespace FindNotes.Models.Parameters
{
    public class Path : Parameter, IParameterWithStringValue
    {
        public string Value { get; init; }

        public Path(string value)
        {
            Value = value.Replace('/', '\\');
        }
    }
}
