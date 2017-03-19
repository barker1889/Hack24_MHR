using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Http;
using System.Windows;
using DataDisplay.Data;
using DataDisplay.Models;
using Newtonsoft.Json;

namespace DataDisplay.Controllers.Api
{
    public class WordCloudController : ApiController
    {
        public IHttpActionResult Get(string filename)
        {
            var analysis = DataFile.GetContents(filename);

            var wordCloudResponse = new List<WordCloudModel>();
            
            foreach (var wordCloudModel in analysis.Words)
            {
                var heatmapDataPoint = analysis.RankedSentences.FirstOrDefault(d => d.Input.Contains(wordCloudModel));

                if (heatmapDataPoint == null)
                {
                    continue;
                }

                wordCloudResponse.Add(new WordCloudModel
                {
                    text = wordCloudModel,
                    size = (int) (new Vector(heatmapDataPoint.Arousal, heatmapDataPoint.Valence).Length*10)
                });
            }

            return Ok(wordCloudResponse.OrderByDescending(w => w.size).Take(100).ToArray());
        }
    }

    public class WordCloudModel
    {
        public string text { get; set; }
        public int size { get; set; }
    }
}