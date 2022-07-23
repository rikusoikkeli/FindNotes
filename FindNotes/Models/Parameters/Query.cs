using FindNotes.Interfaces;

namespace FindNotes.Models.Parameters
{
    public class Query : Parameter, IParameterWithStringValue
    {
        public string Value { get; init; }

        public Query(string value)
        {
            Value = value;
        }
    }
}
