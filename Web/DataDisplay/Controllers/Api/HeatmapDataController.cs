using System.Linq;
using System.Web.Http;
using DataDisplay.Data;
using DataDisplay.Models;

namespace DataDisplay.Controllers.Api
{
    public class HeatmapDataController : ApiController
    {
        public IHttpActionResult Get(double width = 1.0d, double height = 1.0d, double scale = 10.0d, string filename = "eroticnovel")
        {
            var analysis = DataFile.GetContents(filename);

            var data = new HeatmapData
            {
                DataPoints = analysis.RankedSentences.Where(sentence => sentence.Arousal != 0.0d && sentence.Valence != 0.0d).ToArray()
            };
            
            //var noData = datapoints.Count(d => d.Arousal == 0.0d && d.Valence == 0.0d);
            //var poorData = datapoints.Count(d => d.Arousal == 0.0d || d.Valence == 0.0d);
            //var lowData = datapoints.Count(d => d.Arousal < 2.0d && d.Valence < 2.0d);

            var actualScale = scale / 10.0d;

            var response = data.ToSimpleHeatData(width, height, actualScale);

            return Ok(response);
        }
    }
}
