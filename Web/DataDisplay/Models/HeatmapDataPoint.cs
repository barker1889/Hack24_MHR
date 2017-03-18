namespace DataDisplay.Models
{
    public class HeatmapDataPoint
    {
        public double Valence { get; set; }
        public double Arousal { get; set; }
        public string Input { get; set; }
    }

    public class WordAnalysis
    {
        public HeatmapDataPoint[] RankedSentences { get; set; }
        public string[] Words { get; set; }
    }
}