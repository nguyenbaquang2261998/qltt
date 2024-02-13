using Microsoft.EntityFrameworkCore;

namespace InternManagement.Models
{
    public class InternManagementContext : DbContext
    {
        public InternManagementContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Topic> Topics { get; set; }
        public DbSet<Semester> Semesters { get; set; }
        public DbSet<RegisterTopic> RegisterTopics { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<InternshipEvaluation> InternshipEvaluations { get; set; }
        public DbSet<TeacherEvaluation> TeacherEvaluations { get; set; }
    }
}
