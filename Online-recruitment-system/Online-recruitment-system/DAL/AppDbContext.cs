using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Online_recruitment_system.Models.Entities;

namespace Online_recruitment_system.DAL
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<Conference> Conferences { get; set; }
        public DbSet<CompanyImages> CompanyImages { get; set; }
        public DbSet<Banner> Banners { get; set; }
        
        public DbSet<Candidate> Candidates { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Education> Educations { get; set; }
        public DbSet<Experience> Experiences { get; set; }
        public DbSet<Gender> Genders { get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<JobType> JobTypes { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<Profession> Professions { get; set; }
        public DbSet<Skill> Skills { get; set; }
        public DbSet<JobSituation> JobSituations { get; set; }
        public DbSet<SocialLink> SocialLinks { get; set; }
        public DbSet<Email> Emails { get; set; }
        public DbSet<Resume> Resumes { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Branch> Branches { get; set; }
       public DbSet<SkillToCandidate> SkillToCandidates { get; set; }
       public DbSet<Application> Applications { get; set; }
       public DbSet<JobFollowers> JobFollowers { get; set; }

    }
}
