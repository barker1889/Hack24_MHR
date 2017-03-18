using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Http;
using System.Windows;
using DataDisplay.Models;
using Newtonsoft.Json;

namespace DataDisplay.Controllers.Api
{
    public class WordCloudController : ApiController
    {
        public IHttpActionResult Get(string filename)
        {
            var response = new List<WordCloudModel>();

            var inputFile = $"C:\\Hack24Input\\{filename}_Data.json";

            string datafile;

            using (var fs = File.OpenRead(inputFile))
            {
                using (var reader = new StreamReader(fs))
                {
                    datafile = reader.ReadToEnd();
                }
            }

            var datapoints = JsonConvert.DeserializeObject<WordAnalysis>(datafile);

            foreach (var wordCloudModel in datapoints.Words)
            {
                var heatmapDataPoint = datapoints.RankedSentences.FirstOrDefault(d => d.Input.Contains(wordCloudModel));

                if (heatmapDataPoint == null)
                {
                    continue;
                }

                response.Add(new WordCloudModel
                {
                    text = wordCloudModel,
                    size = (int) (new Vector(heatmapDataPoint.Arousal, heatmapDataPoint.Valence).Length*10)
                });
            }

            

            return Ok(response.OrderByDescending(w => w.size).Take(100).ToArray());
        }
    }

    public class WordCloudModel
    {
        public string text { get; set; }
        public int size { get; set; }
    }
}