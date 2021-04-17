using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Saubian.EmailPoller.Helpers
{
    internal class GetYerMawOnTheBlower
    {
        internal async Task<string> Honk(string functionName, string key)
        {
            try
            {
                HttpClient newClient = new HttpClient();
                HttpResponseMessage responseFromAnotherFunction = await newClient.GetAsync($"http://localhost:7071/api/{functionName}/{key}");
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

        internal async Task<dynamic> Honk(string functionName)
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
    }
}
