using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quizz.Auth0Token
{
    public static class Auth0Token
    {
        public static async Task<string> ClaimAccessTokenAuth0()
        {
            //Get TOKEN from API Auth0. T for Token
            var tClient = new RestClient("https://dev-hdysdy74.eu.auth0.com/oauth/token");
            var tRequest = new RestRequest(Method.POST);
            tRequest.AddHeader("content-type", "application/json");
            tRequest.AddParameter("application/json", "{\"client_id\":\"???\",\"client_secret\":\"???\",\"audience\":\"https://dev-hdysdy74.eu.auth0.com/api/v2/\",\"grant_type\":\"client_credentials\"}", ParameterType.RequestBody);
            var tResponse = await tClient.ExecuteAsync(tRequest);
            var token = JsonConvert.DeserializeObject<Token>(tResponse.Content);
            return token.access_token;
        }
        public class Token //Get token from IRestResponse
        {
            public string access_token { get; set; }
        }
    }
}
