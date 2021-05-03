using Shed.CoreKit.WebApi;
using System.Threading.Tasks;

namespace Messages
{
    public class MessagesImpl 
    {
        [Route("messages/get")]
        [HttpGet]
        public async Task<string> Get()
        {
            return "NotImplementedException";
        }
    }
}
