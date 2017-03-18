using System.Linq;

namespace DataDisplay.Models
{
    public class HeatmapData
    {
        public HeatmapDataPoint[] DataPoints { get; set; }

        public double[][] ToSimpleHeatData(double width, double height)
        {
            // width = 600
            // height = 600

            // input = 0, output = 300

            var valanceRange = 3;
            var arousalRange = 4;

            // arousal

            return DataPoints
                .Select(point => new[]
                {
                    ((point.Valence + valanceRange) / (valanceRange * 2)) * width,
                    height - ((point.Arousal / arousalRange) * height),
                    1
                })
                .ToArray();
        }
    }
}