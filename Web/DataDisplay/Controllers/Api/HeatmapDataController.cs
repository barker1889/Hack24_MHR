using System;
using System.IO;
using System.Linq;
using System.Web.Http;
using DataDisplay.Models;
using Newtonsoft.Json;

namespace DataDisplay.Controllers.Api
{
    public class HeatmapDataController : ApiController
    {
        private readonly Random _random;
        
        public HeatmapDataController()
        {
            _random = new Random();
        }

        private static HeatmapDataPoint[] GetDataPoints(string fileName)
        {
            var inputFile = $"C:\\Hack24Input\\{fileName}_Data.json";

            string datafile;

            using (var fs = File.OpenRead(inputFile))
            {
                using (var reader = new StreamReader(fs))
                {
                    datafile = reader.ReadToEnd();
                }
            }

            var datapoints = JsonConvert.DeserializeObject<HeatmapDataPoint[]>(datafile);
            return datapoints;
        }

        public IHttpActionResult Get(double width = 1, double height = 1, string filename = "eroticnovel")
        {
            var datapoints = GetDataPoints(filename);

            var data = new HeatmapData
            {
                DataPoints = datapoints.Where(d => d.Arousal != 0.0d && d.Valence != 0.0d).ToArray()
            };
            
            //var noData = datapoints.Count(d => d.Arousal == 0.0d && d.Valence == 0.0d);
            //var poorData = datapoints.Count(d => d.Arousal == 0.0d || d.Valence == 0.0d);
            //var lowData = datapoints.Count(d => d.Arousal < 2.0d && d.Valence < 2.0d);

            var response = data.ToSimpleHeatData(width, height);

            return Ok(response);
        }

        private bool RandomBool()
        {
            return _random.Next(1) == 1;
        }
    }
}
