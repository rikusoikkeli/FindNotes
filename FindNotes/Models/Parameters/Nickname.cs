using FindNotes.Interfaces;

namespace FindNotes.Models.Parameters
{
    public class Nickname : Parameter, IParameterWithStringValue
    { 
        public string Value { get; init; }

        public Nickname(string value)
        {
            Value = value;
        }
    }
}
