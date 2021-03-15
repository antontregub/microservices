using Interfaces;
using Newtonsoft.Json;
using Shed.CoreKit.WebApi;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Facade
{
    public class FacadeImpl : IFacade
    {
        public string Get()
        {
                
            Model mod = new Model();
            mod.msg = "dd";
            mod.id = System.Guid.NewGuid();
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
            // var a =  client.PostAsync("http://localhost:55051/api/logging/get", new StringContent(JsonConvert.SerializeObject(""), Encoding.UTF8, "application/json"));
            var a = client.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();
            // var contents = "";// a.Result.Content.ReadAsStringAsync();
            var contents = a.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();

            var client1 = new HttpClient();

            //var a1 =  client.PostAsync("http://localhost:55051/api/messages/get", new StringContent(JsonConvert.SerializeObject(""), Encoding.UTF8, "application/json"));
            //var contents1 =  a1.Result.Content.ReadAsStringAsync();

            var a1 = client1.SendAsync(request1).ConfigureAwait(false).GetAwaiter().GetResult();
            // var contents = "";// a.Result.Content.ReadAsStringAsync();
            var contents1 = a1.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();

            var result = contents + contents1;
            result = result.Replace(@"\", "");
            return result;
        }

        public void Post( string msg)
        {
            Model mod = new Model();
            mod.msg = msg;
            mod.id = System.Guid.NewGuid();
            var client = new HttpClient();

            var a =  client.PostAsync("http://localhost:55051/api/logging/post", new StringContent(JsonConvert.SerializeObject(mod), Encoding.UTF8, "application/json"));
        }
    }
}
