using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Newtonsoft.Json;
using Saubian.EmailPoller.Helpers;

namespace Saubian.EmailPoller.Functions
{
    public class GetKeyVaultValue
    {
        const string KEYVAUL_URL_SETTINGS_KEY = "KeyVaultUrl";

        [FunctionName(nameof(GetKeyVaultValue))]
        public async Task<string> Run([HttpTrigger(AuthorizationLevel.Anonymous, "POST", Route = null)] HttpRequestMessage req)
        {
            var key = JsonConvert.DeserializeObject<string>(await req.Content.ReadAsStringAsync());

            if (string.IsNullOrEmpty(key))
                return "No key has been supplied.";

            try
            {
                //Connect to the KeyVault by using the token and get the secret
                var azureServiceTokenProvider = new AzureServiceTokenProvider();
                KeyVaultClient keyVaultClient = new KeyVaultClient(
                    new KeyVaultClient.AuthenticationCallback(azureServiceTokenProvider.KeyVaultTokenCallback)
                    );

                var keyvaultUri = GetYerMawOnTheBlower.GetEnvironmentVariable(KEYVAUL_URL_SETTINGS_KEY);
                var secretUrl = $"{keyvaultUri}/secrets/{key}";

                var secret = await keyVaultClient.GetSecretAsync(secretUrl);

                return secret.Value;
            }
            catch (KeyNotFoundException keyVaultException)
            {
                return keyVaultException.Message;
            }
        }
    }
}
