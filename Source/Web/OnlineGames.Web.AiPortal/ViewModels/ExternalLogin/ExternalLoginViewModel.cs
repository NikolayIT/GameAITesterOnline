namespace OnlineGames.Web.AiPortal.ViewModels.ExternalLogin
{
    using System.ComponentModel.DataAnnotations;

    public class ExternalLoginViewModel
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
