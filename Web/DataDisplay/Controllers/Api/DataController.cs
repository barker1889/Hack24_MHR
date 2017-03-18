using System.Web.Http;

namespace DataDisplay.Controllers.Api
{
    public class DataController : ApiController
    {
        [HttpPost]
        public IHttpActionResult PostData(string data)
        {
            // Write data to a file? Then refresh I guess.
            return Ok();
        }
    }
}
