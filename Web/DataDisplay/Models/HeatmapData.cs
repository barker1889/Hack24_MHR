using System.Linq;

namespace DataDisplay.Models
{
    public class HeatmapData
    {
        public HeatmapDataPoint[] DataPoints { get; set; }

        public double[][] ToSimpleHeatData(double width, double height)
        {
            return DataPoints
                .Select(point => new[]
                {
                    width + (point.Valence * width),
                    height + (point.Arousal * height),
                    point.Intensity
                })
                .ToArray();
        }
    }
}