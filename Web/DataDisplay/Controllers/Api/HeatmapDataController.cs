using System;
using System.Collections.Generic;
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

        private readonly HeatmapDataPoint[] _dataPoints;

        public HeatmapDataController()
        {
            _random = new Random();

            string datafile;

            using (var fs = File.OpenRead("C:\\Hack24Input\\alice_DataAverage.json"))
            {
                using (var reader = new StreamReader(fs))
                {
                    datafile = reader.ReadToEnd();
                }
            }

            _dataPoints = JsonConvert.DeserializeObject<HeatmapDataPoint[]>(datafile);
        }

        public IHttpActionResult Get(double width = 1, double height = 1)
        {
            var data = new HeatmapData
            {
                DataPoints = _dataPoints.Where(d => d.Arousal != 0.0d && d.Valence != 0.0d).ToArray()
            };

            //var randomDataPoints = new List<HeatmapDataPoint>();

            //for (var i = 0; i < 1000; i++)
            //{
            //    randomDataPoints.Add(new HeatmapDataPoint
            //    {
            //        Valence = _random.NextDouble() * (RandomBool() ? 1 : -1),
            //        Arousal = _random.NextDouble() * (RandomBool() ? 1 : -1)
            //    });
            //}


            var noData = _dataPoints.Count(d => d.Arousal == 0.0d && d.Valence == 0.0d);
            var poorData = _dataPoints.Count(d => d.Arousal == 0.0d || d.Valence == 0.0d);
            var lowData = _dataPoints.Count(d => d.Arousal < 2.0d && d.Valence < 2.0d);




            var response = data.ToSimpleHeatData(width, height);

            return Ok(response);
        }

        private bool RandomBool()
        {
            return _random.Next(1) == 1;
        }
    }
}
