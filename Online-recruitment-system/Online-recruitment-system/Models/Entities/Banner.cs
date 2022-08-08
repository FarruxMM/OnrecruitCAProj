using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Online_recruitment_system.Models.Entities
{
    public class Banner : BaseEntity
    {
        [Required, MaxLength(150)]
        public string Title { get; set; }
        [ MaxLength(500)]
        public string Text { get; set; }
        [Required, MaxLength(200)]
        public string Subtitle { get; set; }

        //public List<string> Subtitles { get; set; }
        public string Image { get; set; }
        [NotMapped]
        public IFormFile Img { get; set; }
    }
}
