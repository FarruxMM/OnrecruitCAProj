using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;

namespace Online_recruitment_system.Models.Entities
{
    public class Resume : BaseEntity, IValidatableObject
    {
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var resume = ((Resume)validationContext.ObjectInstance).Resume_Upload;
            var extension = Path.GetExtension(resume.FileName);
            var size = resume.Length;

            if (!extension.ToLower().Equals(".doc") && !extension.ToLower().Equals(".docx") && !extension.ToLower().Equals(".pdf"))
                yield return new ValidationResult("File extension is not valid.");

            if (size > (5 * 1024 * 1024))
                yield return new ValidationResult("File size is bigger than 5MB.");
        }


        public string Resume_Uploading { get; set; }

        [NotMapped]
        [DataType(DataType.Upload)]
        public IFormFile Resume_Upload { get; set; }

        public int CandidateId { get; set; }
        public Candidate Candidate { get; set; }

        [Required]
        public string Resume_Bio { get; set; }

        [Required]
        public string Resume_Title { get; set; }
        public string Resume_Ext { get; set; }
        public string Resume_Name { get; set; }
    }
}
