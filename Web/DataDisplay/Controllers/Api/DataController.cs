using System.IO;
using System.Web.Http;
using DataDisplay.Data;

namespace DataDisplay.Controllers.Api
{
    public class DataController : ApiController
    {
        [HttpPost]
        public IHttpActionResult Post()
        {
            var data = GetStringFromBody();

            DataFile.AppendRawContent(data);
            DataFile.UpdateWorkingFile();

            return Ok();
        }

        private string GetStringFromBody()
        {
            using (var contentStream = Request.Content.ReadAsStreamAsync().Result)
            {
                contentStream.Seek(0, SeekOrigin.Begin);
                using (var sr = new StreamReader(contentStream))
                {
                    return sr.ReadToEnd();
                }
            }
        }
    }
}
