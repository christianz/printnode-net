using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PrintNodeNet.Http;

namespace PrintNodeNet
{
    public sealed class PrintNodeChildAccount
    {
        /// <summary>
        /// Assigned by API. Any value submitted here will be ignored.
        /// </summary>
        [JsonProperty("id")]
        public long? Id { get; set; }

        /// <summary>
        /// The child account user's first name
        /// </summary>
        [JsonProperty("firstname")]
        public string FirstName { get; set; }

        /// <summary>
        /// The child account user's last name
        /// </summary>
        [JsonProperty("lastname")]
        public string LastName { get; set; }

        /// <summary>
        /// A valid email address. This must be unique and would normally be the email address of your customer. 
        /// This email is for actions like account password resets, informing customers of new versions of the 
        /// client, etc. Your email will never be shared with any third party or used for marketing purposes.
        /// </summary>
        [JsonProperty("email")]
        public string Email { get; set; }

        /// <summary>
        /// The password should be plain text. It will be encoded in the PrintNode database and cannot be 
        /// retrieved, only reset.
        /// </summary>
        [JsonProperty("password")]
        public string Password { get; set; }

        /// <summary>
        /// OPTIONAL
        /// 
        /// A unique reference which you can use as a method to identify an account.
        /// </summary>
        [JsonProperty("creatorRef")]
        public string CreatorRef { get; set; }

        /// <summary>
        /// OPTIONAL
        /// 
        /// A array of named API Keys you wish to create for this account.
        /// </summary>
        [JsonProperty("ApiKeys")]
        public string[] ApiKeys { get; set; }

        /// <summary>
        /// OPTIONAL
        /// 
        /// A array of tag names and values to be associated with an account.
        /// </summary>
        [JsonProperty("Tags")]
        public Dictionary<string, string> Tags { get; set; }

        public async Task<PrintNodeChildAccount> CreateAsync(PrintNodeRequestOptions options = null)
        {
            var response = await PrintNodeApiHelper.Post("/account", new
            {
                Account = this,
                ApiKeys,
                Tags
            }, options);

            return JsonConvert.DeserializeObject<PrintNodeChildAccount>(response, new PrintNodeChildAccountCreationResponseConverter());
        }

        public static async Task<string> GetKeyAsync(string clientId, PrintNodeRequestOptions options = null)
        {
            var response = await PrintNodeApiHelper.Get($"/client/key/{clientId}?version=4.7.1&edition=printnode", options);

            return JsonConvert.DeserializeObject<string>(response);
        }

        public static async Task<bool> Exists(PrintNodeRequestOptions options = null)
        {
            try
            {
                var response = await PrintNodeApiHelper.Get("/whoami", options);

                return !string.IsNullOrEmpty(response);
            }
            catch
            {
                return false;
            }
        }

        public async Task<PrintNodeChildAccount> UpdateAsync(PrintNodeRequestOptions options = null)
        {
            var response = await PrintNodeApiHelper.Patch("/account", this, options, new Dictionary<string, string>
            {
                { "X-Child-Account-By-Id", Id.ToString() }
            });

            return JsonConvert.DeserializeObject<PrintNodeChildAccount>(response);
        }

        public static async Task<bool> DeleteAsync(long id, PrintNodeRequestOptions options = null)
        {
            var response = await PrintNodeApiHelper.Delete("/account", options, new Dictionary<string, string>
            {
                { "X-Child-Account-By-Id", id.ToString() }
            });

            return JsonConvert.DeserializeObject<bool>(response);
        }
    }
}
