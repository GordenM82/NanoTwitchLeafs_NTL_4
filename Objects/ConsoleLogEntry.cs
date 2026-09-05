using System.Windows.Media;

namespace NanoTwitchLeafs.Objects
{
    public sealed class ConsoleLogEntry
    {
        public string Level { get; set; }
        public string Message { get; set; }

        public Brush Foreground => Level switch
        {
            "ERROR" or "FATAL" => Brushes.IndianRed,
            "WARN" => Brushes.DarkOrange,
            "DEBUG" => Brushes.Gray,
            _ => null
        };
    }
}
