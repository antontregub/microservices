using Newtonsoft.Json;
using Shed.CoreKit.WebApi;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Facade
{
    public class FacadeImpl //: IFacade
    {
        [Route("facade/get")]
        [HttpGet]
        public string Get()
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(GetRandomUri()+"get"),
                Content = new StringContent("", Encoding.UTF8, "application/json"),
            };
            var request1 = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("http://localhost:5003/messages/get"),
                Content = new StringContent("", Encoding.UTF8, "application/json"),
            };
            var req =  client.SendAsync(request).Result;

            string jsonString = req.Content.ReadAsStringAsync()
                                               .Result
                                               .Replace("\\", "")
                                               .Trim(new char[1] { '"' });

            var contents = JsonConvert.DeserializeObject<ResultMod>(jsonString);

            var client1 = new HttpClient();
            var req1 = client1.SendAsync(request1).Result;
            var jsonString1 = req1.Content.ReadAsStringAsync()
                                                .Result
                                                .Replace("\\", "")
                                                .Trim(new char[1] { '"' });
            var contents1 = JsonConvert.DeserializeObject<ResultMod>(jsonString1);

            var result = contents.Result + contents1.Result;
            result = result.Replace(@"\", "");
            return result;
        }

        [Route("facade/post/{msg}")]
        [HttpPost]
        public async void Post( string msg)
        {
            Model mod = new Model();
            mod.msg = msg;
            mod.id = System.Guid.NewGuid();
            var client = new HttpClient();
            var post = await client.PostAsync(GetRandomUri()+"post", new StringContent(JsonConvert.SerializeObject(mod), Encoding.UTF8, "application/json"));
        }


        public string GetRandomUri()
        {
            var list = new List<string>
            {
                "http://localhost:5004/logging/",
                "http://localhost:5005/logging/",
                "http://localhost:5006/logging/"
            };
            Random rnd = new Random();
            return list[rnd.Next(0, 3)];
        }
    }

    [Serializable]
    public class ResultMod
    {
        public string Result { get; set; }
    }
}
