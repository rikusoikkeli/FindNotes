using FindNotes.Interfaces;

namespace FindNotes.Models
{
    public class GlobalOptions : IGlobalOptions
    {
        public string ProgramName { get; set; }
        public bool InTestMode { get; set; }
    }
}
