using System;
using System.Collections.Generic;
using System.Web.Http;
using DataDisplay.Models;

namespace DataDisplay.Controllers.Api
{
    public class HeatmapDataController : ApiController
    {
        private readonly Random _random;

        public HeatmapDataController()
        {
            _random = new Random();
        }

        public IHttpActionResult Get(double width = 1, double height = 1)
        {
            var randomData = new HeatmapData();

            var randomDataPoints = new List<HeatmapDataPoint>();

            for (var i = 0; i < 1000; i++)
            {
                randomDataPoints.Add(new HeatmapDataPoint
                {
                    Valence = _random.NextDouble() * (RandomBool() ? 1 : -1),
                    Arousal = _random.NextDouble() * (RandomBool() ? 1 : -1)
                });
            }

            randomData.DataPoints = randomDataPoints.ToArray();

            return Ok(randomData.ToSimpleHeatData(width, height));
        }

        private bool RandomBool()
        {
            return _random.Next(1) == 1;
        }
    }
}
