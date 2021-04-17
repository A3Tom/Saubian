using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Newtonsoft.Json;

namespace Saubian.EmailPoller.Functions
{
    public class GetKeyVaultValue
    {
        const string KEYVAULT_BASE_URI = "https://realmseal-dev.vault.azure.net/";

        [FunctionName(nameof(GetKeyVaultValue))]
        public async Task<string> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequestMessage req)
        {
            var key = JsonConvert.DeserializeObject<string>(await req.Content.ReadAsStringAsync());

            if (string.IsNullOrEmpty(key))
                return "No key has been supplied.";

            string message = "";
            // This is available as "DNS Name" from the overview page of the Key Vault.
            try
            {
                //Connect to the KeyVault by using the token and get the secret
                var azureServiceTokenProvider = new AzureServiceTokenProvider();
                KeyVaultClient keyVaultClient = new KeyVaultClient(
                    new KeyVaultClient.AuthenticationCallback(azureServiceTokenProvider.KeyVaultTokenCallback)
                    );

                var secret = await keyVaultClient.GetSecretAsync(KEYVAULT_BASE_URI + $"secrets/{key}");

                message = secret.Value;

                Console.WriteLine(message);

                return message;
            }
            catch (KeyNotFoundException keyVaultException)
            {
                message = keyVaultException.Message;

                return message;
            }

        }
    }
}
