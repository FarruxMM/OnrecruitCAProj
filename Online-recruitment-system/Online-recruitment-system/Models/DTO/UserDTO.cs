using Online_recruitment_system.Models.ViewModels;

namespace Online_recruitment_system.Models.DTO
{
    public class UserDTO
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public bool IsActive { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public PaginationModel PaginationModel { get; set; }

    }
}
