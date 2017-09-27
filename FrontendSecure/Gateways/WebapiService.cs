using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;

namespace FrontendSecure.Gateways
{
    public class WebapiService
    {
        private WebapiService(string baseUri)
        {
            BaseUri = baseUri;
        }

        private static WebapiService _instance;
        public string BaseUri { get; private set; }

        public static WebapiService instance
        {
            get
            {
                return _instance??(_instance = new WebapiService(ConfigurationManager.AppSettings["WebApiUri"]));
            } 
        }
        public async Task<T> AuthedicateAsync<T>(string userName, string password)
        {
            using (var client = new HttpClient())
            {
                var result =
                    await
                        client.PostAsync(ActionUri("/Token"),
                            new FormUrlEncodedContent(new List<KeyValuePair<string, string>>
                            {
                                new KeyValuePair<string, string>("grant_type", "password"),
                                new KeyValuePair<string, string>("userName", userName),
                                new KeyValuePair<string, string>("password", password)
                            }));
                return await DeserializeObject<T>(result);
            }
        }

        private static async Task<T> DeserializeObject<T>(HttpResponseMessage result)
        {
            
            string json = await result.Content.ReadAsStringAsync();
            if (result.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<T>(json);
            }
            throw new ApiException(result.StatusCode, json);
        }

        private string ActionUri(string action)
        {
            var c = BaseUri + action;
            return c;
        }

        public async Task<T> GetAsync<T>(string action, string authToken = null)
        {
            using (var client = new HttpClient())
            {
                SetAuthHeader(authToken, client);


                using (Stream s = client.GetStreamAsync(ActionUri(action)).Result)
                using (StreamReader sr = new StreamReader(s))
                using (JsonReader reader = new JsonTextReader(sr))
                {
                    JsonSerializer serializer = new JsonSerializer();

                    // read the json from a stream
                    // json size doesn't matter because only a small piece is read at a time from the HTTP request
                    T t = serializer.Deserialize<T>(reader);
                    return t;
                }

                
                //var result = client.GetAsync(ActionUri(action)).Result;
               // return await DeserializeObject<T>(result);
            }
        }

        private static void SetAuthHeader(string authToken, HttpClient client)
        {
            if (!authToken.IsNullOrWhiteSpace())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse("Bearer " + authToken);
            }
        }

        public async Task<bool> PutAsync<T>(string action, T data, string authToken = null)
        {
            using (var client = new HttpClient())
            {
                SetAuthHeader(authToken, client);
                var result =  client.PutAsJsonAsync(ActionUri(action), data).Result;
                string json = await result.Content.ReadAsStringAsync();
                if (result.IsSuccessStatusCode)
                {
                    return true;
                }
                throw new ApiException(result.StatusCode, json);
            }
        }

        public async Task<T> PostAsync<T>(string action, T data, string authToken = null)
        {
            using (var client = new HttpClient())
            {
                SetAuthHeader(authToken, client);
                var result = client.PostAsJsonAsync(ActionUri(action), data).Result;
                return await DeserializeObject<T>(result);
            }
        }

        public async Task<bool> DeleteAsync<T>(string action, string authToken = null)
        {
            using (var client = new HttpClient())
            {
                SetAuthHeader(authToken, client);
                var result = client.DeleteAsync(ActionUri(action)).Result;
                string json = await result.Content.ReadAsStringAsync();
                if (result.IsSuccessStatusCode)
                {
                    return true;
                }
                throw new ApiException(result.StatusCode, json);
            }
        }
    }
}