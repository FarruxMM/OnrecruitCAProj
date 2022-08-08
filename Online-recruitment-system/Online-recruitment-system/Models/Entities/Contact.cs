using System.ComponentModel.DataAnnotations;

namespace Online_recruitment_system.Models.Entities
{
    public class Contact:BaseEntity
    {
        [Required, MaxLength(150)]
        public string Name { get; set; }

        [Required, MaxLength(150)]
        public string Email  { get; set; }

        [Required, MaxLength(150)]
        public string Phone { get; set; }

        [Required, MaxLength(550)]
        public string Subject { get; set; }
        [Required, MaxLength(550)]
        public string Message { get; set; }
    }

}
