using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace nzbget_silk.Service
{
    public class JsonRequest
    {
        private const string CONTENTTYPE_JSON = "application/json";

        private static async Task<string> PostJson(string url, string json)
        {
            using (var client = new HttpClient())
            {
                using (var message = new HttpRequestMessage(HttpMethod.Post, url))
                {
                    message.Content = new StringContent(json);
                    message.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse(CONTENTTYPE_JSON));
                    message.Headers.AcceptEncoding.Add(StringWithQualityHeaderValue.Parse("UTF-8"));

                    var result = await client.SendAsync(message);

                    if (result.IsSuccessStatusCode)
                    {
                        return await result.Content.ReadAsStringAsync();
                    }
                }
            }

            return null;
        }

        private static async Task<string> GetJson(string url)
        {
            using (var client = new HttpClient())
            {
                using (var message = new HttpRequestMessage(HttpMethod.Get, url))
                {
                    message.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse(CONTENTTYPE_JSON));
                    message.Headers.AcceptEncoding.Add(StringWithQualityHeaderValue.Parse("UTF-8"));

                    var result = await client.SendAsync(message);

                    if (result.IsSuccessStatusCode)
                    {
                        return await result.Content.ReadAsStringAsync();
                    }
                }
            }

            return null;
        }

        public static T DeserializeJson<T>(string jsonData) where T : class
        {
            T value = default(T);

            if (!string.IsNullOrEmpty(jsonData))
            {
                try { value = JsonConvert.DeserializeObject<T>(jsonData); }
                catch { }
            }

            return value;
        }

        public static string SerializeJson(object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        public static async Task<T> Post<T>(string url, object request) where T : class
        {
            T response = null;
            string requestJson = SerializeJson(request);
            string data = await PostJson(url, requestJson);

            if (data != null)
            {
                response = JsonConvert.DeserializeObject<T>(data);
            }

            return response;
        }

        public static async Task<T> Get<T>(string url) where T : class
        {
            T response = null;
            string data = await GetJson(url);

            if (data != null)
            {
                try
                {
                    response = JsonConvert.DeserializeObject<T>(data);
                }
                catch (Exception)
                {

                    throw;
                }
            }

            return response;
        }
    }
}
