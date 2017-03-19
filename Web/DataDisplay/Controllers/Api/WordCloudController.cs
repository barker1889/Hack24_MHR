using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Http;
using System.Windows;
using DataDisplay.Data;
using DataDisplay.Models;
using Newtonsoft.Json;
using SentenceAnalyserCore;

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


            var positiveWords = analysis.PositiveWords?.OrderByDescending(w => w.Count);
            var negativeWords = analysis.NegativeWords?.OrderByDescending(w => w.Count);
            var arousingWords = analysis.ArousingWords?.OrderByDescending(w => w.Count);
            var boredWords = analysis.BoredWords?.OrderByDescending(w => w.Count);

            var fullResponse = new WordCloudResponseModel
            {
                WordCloudModels = wordCloudResponse.OrderByDescending(w => w.size).Take(100).ToArray(),
                PositiveWords = positiveWords?.ToArray(),
                NegativeWords = negativeWords?.ToArray(),
                ArousingWords = arousingWords?.ToArray(),
                CalmWords = boredWords?.ToArray()
            };

            return Ok(fullResponse);
        }
    }

    public class WordCloudResponseModel
    {
        public WordCloudModel[] WordCloudModels { get; set; }
        public SentenceWordAnalysis[] PositiveWords { get; set; }
        public SentenceWordAnalysis[] NegativeWords { get; set; }
        public SentenceWordAnalysis[] ArousingWords { get; set; }
        public SentenceWordAnalysis[] CalmWords { get; set; }
    }

    public class WordCloudModel
    {
        public string text { get; set; }
        public int size { get; set; }
    }
}