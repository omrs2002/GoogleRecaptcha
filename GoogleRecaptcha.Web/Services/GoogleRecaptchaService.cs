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
        public async Task<bool> VirifyTokenAsync(string token , bool ValidateIamNotRobot = false)
        {
            string secretkey = ValidateIamNotRobot ? _googleCaptcha.CurrentValue.V2Secretkey : _googleCaptcha.CurrentValue.Secretkey;

            var url = $"{_googleCaptcha.CurrentValue.Url}{secretkey}&response={token}";
            using var client = new HttpClient();
            var response = await client.GetAsync(url);
            if(response.StatusCode != System.Net.HttpStatusCode.OK)
                return false;
            
            var tokenResponse = await response.Content.ReadAsStringAsync(); 
            var captchaResponse = JsonConvert.DeserializeObject<GoogleRecaptchaResponse>(tokenResponse);
            if (captchaResponse != null)
            {
                if (captchaResponse.Success) {return true;}
                else
                {
                    var error = captchaResponse.ErrorCodes[0].ToLower();
                    var errorMessage = error switch
                    {
                        ("missing-input-secret") => "The secret key parameter is missing.",
                        ("invalid-input-secret") => "The given secret key parameter is invalid.",
                        ("missing-input-response") => "The g-recaptcha-response parameter is missing.",
                        ("invalid-input-response") => "The given g-recaptcha-response parameter is invalid.",
                        _ => "reCAPTCHA Error. Please try again!",
                    };
                    return false;
                }
            }
            return false;
        }
    }
}
