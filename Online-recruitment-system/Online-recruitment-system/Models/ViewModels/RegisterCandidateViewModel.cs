using System;
using System.ComponentModel.DataAnnotations;

namespace Online_recruitment_system.Models.ViewModels
{
    public class RegisterCandidateViewModel
    {
        [Required, MaxLength(200)]
        public string Name { get; set; }

        [Required, MaxLength(200)]
        public string Surname { get; set; }

        [Required]
        public int UserType { get; set; }

        [Required, EmailAddress, DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required, DataType(DataType.Password)]
        public string Password { get; set; }

        [Required, DataType(DataType.Password), Compare(nameof(Password), ErrorMessage = "Passwords are not match")]
        public string PasswordConfirm { get; set; }
    }
}
