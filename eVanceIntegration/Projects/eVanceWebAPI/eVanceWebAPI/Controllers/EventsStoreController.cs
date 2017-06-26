using System.Web.Http;

namespace eVanceWebAPI.Controllers
{
    public class EventsStoreController : ApiController
    {
        [HttpPost]
        public bool SendEventData(string jsonFormattedEvent)
        {
            try
            {
               
            }
            catch
            {
                return false;
            }
            return true;
        }
    }
}
