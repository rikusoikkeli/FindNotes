using FindNotes.Models;

namespace FindNotes.Interfaces
{
    public interface IReadWriteSettings
    {
        public Settings ReadSettings();
        public void SaveSettings(Settings settings);
        public bool ResetSettings();
    }
}
