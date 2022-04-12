using GoogleRecaptcha.Web.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace GoogleRecaptcha.Web.Services
{

    public class GoogleRecaptchaService
    {
        private readonly IOptionsMonitor<GoogleReacaptchaConf> _googleCaptcha;
        public GoogleRecaptchaService(IOptionsMonitor<GoogleReacaptchaConf> googleCaptcha)
        {
            _googleCaptcha = googleCaptcha;
        }

        public async Task<bool> VirifyTokenAsync(string token)
        {
            var url = $"{_googleCaptcha.CurrentValue.Url}{_googleCaptcha.CurrentValue.Secretkey}&response={token}";

            using var client = new HttpClient();
            var response = await client.GetAsync(url);
            if(response.StatusCode != System.Net.HttpStatusCode.OK)
                return false;
            
            var tokenResponse = await response.Content.ReadAsStringAsync(); 
            var googleResult = JsonConvert.DeserializeObject<GoogleRecaptchaResponse>(tokenResponse);
            
            if(googleResult != null)
                return googleResult.Success && googleResult.Score >= 0.5;

            return false;
        }

    }
}
