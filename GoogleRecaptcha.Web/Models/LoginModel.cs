using System.ComponentModel.DataAnnotations;

namespace GoogleRecaptcha.Web.Models
{
    public class LoginModel
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Token { get; set; }


    }

}
