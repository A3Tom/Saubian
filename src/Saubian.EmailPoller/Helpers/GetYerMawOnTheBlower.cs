using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Saubian.EmailPoller.Helpers
{
    public static class GetYerMawOnTheBlower
    {
        public static async Task<dynamic> Honk(string functionName)
        {
            try
            {
                HttpClient newClient = new HttpClient();
                HttpResponseMessage responseFromAnotherFunction = await newClient.GetAsync($"http://localhost:7071/api/{functionName}/");
                dynamic response = "";

                if (responseFromAnotherFunction.IsSuccessStatusCode)
                {
                    response = responseFromAnotherFunction.Content.ReadAsStringAsync().Result;
                }

                return response;
            }
            catch (Exception ex)
            {
                return string.Format("Something went wrong, please try agian! Reason:{0}", ex.Message);
            }
        }

        public static async Task<string> Honk(string baseUrl, string functionName, object payload)
        {
            try
            {
                var response = "";
                var url = $"{baseUrl}/api/{functionName}/";

                HttpClient newClient = new HttpClient();
                HttpResponseMessage responseFromAnotherFunction = await newClient.PostAsJsonAsync(url, payload);

                if (responseFromAnotherFunction.IsSuccessStatusCode)
                {
                    response = responseFromAnotherFunction.Content.ReadAsStringAsync().Result;
                }

                return response;
            }
            catch (Exception ex)
            {
                return string.Format("Something went wrong, please try agian! Reason:{0}", ex.Message);
            }
        }

        public static async Task<T> Honk<T>(string baseUrl, string functionName, object payload)
        {
            try
            {
                HttpClient newClient = new HttpClient();
                HttpResponseMessage responseFromAnotherFunction = await newClient.PostAsJsonAsync($"{baseUrl}/api/{functionName}/", payload);
                dynamic response = "";

                if (responseFromAnotherFunction.IsSuccessStatusCode)
                {
                    response = responseFromAnotherFunction.Content.ReadAsStringAsync().Result;
                }

                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Something went wrong, please try agian! Reason:{0}", ex.Message);

                throw ex;
            }
        }

        public static string GetEnvironmentVariable(string name)
        {
            return Environment.GetEnvironmentVariable(name, EnvironmentVariableTarget.Process);
        }
    }
}
