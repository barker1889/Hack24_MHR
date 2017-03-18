using System.Linq;

namespace DataDisplay.Models
{
    public class HeatmapData
    {
        public HeatmapDataPoint[] DataPoints { get; set; }

        public double[][] ToSimpleHeatData(double width, double height, double scale)
        {
            var valanceRange = 3.0d / scale;
            var arousalRange = 3.0d / scale;

            return DataPoints
                .Select(point => new[]
                {
                    GetValencePoint(width, point, valanceRange),
                    GetArousalPoint(height, point, arousalRange),
                    1
                })
                .ToArray();
        }

        private static double GetValencePoint(double width, HeatmapDataPoint point, double valanceRange)
        {
            return ((point.Valence + valanceRange) / (valanceRange * 2)) * width;
        }

        private static double GetArousalPoint(double height, HeatmapDataPoint point, double arousalRange)
        {
            return height - (((point.Arousal + arousalRange) / (arousalRange * 2)) * height);
        }
    }
}