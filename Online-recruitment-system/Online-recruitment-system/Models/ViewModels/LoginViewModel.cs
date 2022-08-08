using System.ComponentModel.DataAnnotations;

namespace Online_recruitment_system.Models.ViewModels
{
    public class LoginViewModel
    {
        [Required, MaxLength(100)]
        public string Email { get; set; }

        [Required, DataType(DataType.Password)]
        public string Password { get; set; }

        public bool KeepMeLoggedIn { get; set; }
    }
}
