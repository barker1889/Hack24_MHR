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
        public HeatmapDataPoint[] DataPoints { get; set; }
        public string[] WordCloudModels { get; set; }
    }
}