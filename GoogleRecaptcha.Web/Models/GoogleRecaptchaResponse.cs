using Newtonsoft.Json;

namespace GoogleRecaptcha.Web.Models
{
    public class GoogleRecaptchaResponse
    {
        public bool Success { get; set; }
        public DateTime ChallengeTs { get; set; }
        public string Hostname { get; set; }
        public double Score { get; set; }
        public string Action { get; set; }
        
        [JsonProperty("error-codes")]
        public List<string> ErrorCodes { get; set; }

    }
}
