using Newtonsoft.Json;
using Shed.CoreKit.WebApi;
using System;
using System.Net.Http;
using System.Text;

namespace Facade
{
    public class FacadeImpl //: IFacade
    {
        [Route("get")]
        [HttpGet]
        public string Get()
        {
                
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("http://localhost:55051/api/logging/get"),
                Content = new StringContent(JsonConvert.SerializeObject(""), Encoding.UTF8, "application/json"),
            };
            var request1 = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("http://localhost:55051/api/messages/get"),
                Content = new StringContent(JsonConvert.SerializeObject(""), Encoding.UTF8, "application/json"),
            };
            var req = client.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();
            var contents = req.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();

            var client1 = new HttpClient();


            var req1 = client1.SendAsync(request1).ConfigureAwait(false).GetAwaiter().GetResult();
            var contents1 = req1.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();

            var result = contents + contents1;
            result = result.Replace(@"\", "");
            return result;
        }

        [Route("post/{msg}")]
        [HttpPost]
        public void Post( string msg)
        {
            Model mod = new Model();
            mod.msg = msg;
            mod.id = System.Guid.NewGuid();
            var client = new HttpClient();

            var post =  client.PostAsync("http://localhost:55051/api/logging/post", new StringContent(JsonConvert.SerializeObject(mod), Encoding.UTF8, "application/json"));
        }
    }
}
