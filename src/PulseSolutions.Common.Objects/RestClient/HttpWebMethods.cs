
using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace PulseSolutions.Common.Objects.RestClient
{
    public static class HttpWebMethods
    {
        private static readonly object HttpClientExtensions;

        /// <summary>
        /// Asynchronous Get method
        /// </summary>
        /// <param name="url">url as complete url.</param>
        /// <returns>json string</returns>
        public static string GetStringAsync(string url)
        {
            return new HttpClient().GetStringAsync(url).Result;
        }

        /// <summary>
        /// Asynchronous post method. 
        /// </summary>
        /// <param name="baseAddess">Base address of api</param>
        /// <param name="endpoint">Endpoint refers to the name of api calls</param>
        /// <param name="postedData">data to be posted as an object</param>
        /// <returns>Output as Json</returns>

        public static object PostAsync(string baseAddess, string endpoint, object postedData)
        {
            object result = new object();
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(baseAddess);
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                System.Net.Http.HttpClientExtensions.PostAsJsonAsync<object>(httpClient, endpoint, postedData).ContinueWith(task =>
                {
                    if (!task.IsCompleted)
                        return;
                    HttpResponseMessage taskResult = task.Result;
                    if (taskResult.IsSuccessStatusCode)
                        result = (object)taskResult.Content.ReadAsStringAsync().Result;
                }).Wait();
            }
            return result;
        }
    }
}
