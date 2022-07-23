using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FindNotes.Interfaces
{
    public interface IGlobalOptions
    {
        public string ProgramName { get; set; }
        public bool InTestMode { get; set; }
    }
}
