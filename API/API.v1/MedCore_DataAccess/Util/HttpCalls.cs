using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;

namespace MedCore_DataAccess.Util
{
     public static class HttpCalls
    {
        #region Get Members
        public static T GetData<T>(HttpClient client, string resource) where T : new()
        {
            if (client == null || string.IsNullOrWhiteSpace(resource))
            {
                return default(T);
            }

            string result = string.Empty;
            var response = client.GetAsync(resource).Result;

            VerifyResponse(response);

            if (response.IsSuccessStatusCode)
            {
                result = response.Content.ReadAsStringAsync().Result;
            }

            return JsonConvert.DeserializeObject<T>(result);
        }
        #endregion

        #region Post Members
        public static T PostData<T>(HttpClient client, string resource, object data) where T : new()
        {
            if (client == null || string.IsNullOrWhiteSpace(resource))
            {
                return default(T);
            }

            string result = string.Empty;
            var jsonSent = data == null ? string.Empty : JsonConvert.SerializeObject(data);
            StringContent content = new StringContent(jsonSent, Encoding.UTF8, "application/json");
            var response = client.PostAsync(resource, content).Result;

            VerifyResponse(response);

            if (response.IsSuccessStatusCode)
            {
                result = response.Content.ReadAsStringAsync().Result;
            }
            var jsonReceived = result;

            return JsonConvert.DeserializeObject<T>(jsonReceived);
        }

        public static String PostData(HttpClient client, string resource, object data)
        {
            if (client == null || string.IsNullOrWhiteSpace(resource))
            {
                return null;
            }

            string result = string.Empty;
            var jsonSent = data == null ? string.Empty : JsonConvert.SerializeObject(data);
            StringContent content = new StringContent(jsonSent, Encoding.UTF8, "application/json");
            var response = client.PostAsync(resource, content).Result;

            VerifyResponse(response);

            if (response.IsSuccessStatusCode)
            {
                result = response.Content.ReadAsStringAsync().Result;
            }

            return result;
        }

        #endregion

        private static void VerifyResponse(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(response.ReasonPhrase);
            }
        }

        public static HttpClient CreateClient(string baseUrl)
        {
            HttpClient client = new HttpClient()
            {
                BaseAddress = new Uri(baseUrl)
            };

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.AcceptCharset.Add(new StringWithQualityHeaderValue("utf-8"));
            
            return client;
        }
    }
}