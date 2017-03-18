using System.Collections.Generic;
using System.Web.Http;

namespace DataDisplay.Controllers.Api
{
    public class WordCloudController : ApiController
    {
        public IHttpActionResult Get(string filename)
        {
            var response = new List<WordCloudModel>
            {
                new WordCloudModel { text = "word", size = 100},
                new WordCloudModel { text = "yet", size = 90},
                new WordCloudModel { text = "another", size = 85},
                new WordCloudModel { text = "for", size = 80},
                new WordCloudModel { text = "the", size = 70},
                new WordCloudModel { text = "cloud", size = 60},
                new WordCloudModel { text = "of", size = 40},
                new WordCloudModel { text = "words", size = 200},
                new WordCloudModel { text = "that", size = 30},
                new WordCloudModel { text = "are", size = 100}
            };

            return Ok(response.ToArray());
        }
    }

    public class WordCloudModel
    {
        public string text { get; set; }
        public int size { get; set; }
    }
}