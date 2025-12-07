namespace ToolBox.Services.Haptics
{
    public class HapticConfig
    {
        public HapticConfig(int durationMilliSeconds, int amplitude)
        {
            DurationMilliSeconds = durationMilliSeconds;
            Amplitude = amplitude;
        }
        
        public int DurationMilliSeconds { get; set; }
        public int Amplitude { get; set; }
    }
}