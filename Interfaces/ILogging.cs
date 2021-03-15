using Shed.CoreKit.WebApi;


namespace Interfaces
{
    public interface ILogging
    {
        [Route("post")]
        [HttpPost]
        public string Post([FromBody] Model msg);

        [Route("get")]
        [HttpGet]
        public string Get();
    }


    public class Model
    {
        public string msg { get; set; }
        public System.Guid id { get; set; }
    }
}
