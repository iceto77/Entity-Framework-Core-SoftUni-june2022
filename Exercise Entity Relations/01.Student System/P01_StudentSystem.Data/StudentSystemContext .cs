namespace P01_StudentSystem.Data
{
    using Microsoft.EntityFrameworkCore;
    using Models;
    public class StudentSystemContext : DbContext
    {
        public StudentSystemContext()
        {
        }

        public StudentSystemContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Resource> Resources { get; set; }
        public DbSet<Homework> HomeworkSubmissions { get; set; }
        public DbSet<StudentCourse> StudentCourses { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(Config.ConnectionString);
            }

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<StudentCourse>(e =>
                {
                    e.HasKey(sc => new { sc.StudentId, sc.CourseId });
                });
            modelBuilder
               .Entity<Student>()
                .Property(s => s.Name).IsUnicode(true);
            modelBuilder
               .Entity<Student>()
                .Property(s => s.PhoneNumber).IsUnicode(false);
            modelBuilder
               .Entity<Course>()
                .Property(c => c.Name).IsUnicode(true);
            modelBuilder
               .Entity<Course>()
                .Property(c => c.Description).IsUnicode(true);
            modelBuilder
               .Entity<Resource>().
               Property(r => r.Name).IsUnicode(true);
            modelBuilder
               .Entity<Resource>().
               Property(r => r.Url).IsUnicode(true);
            modelBuilder
               .Entity<Homework>()
                .Property(h => h.Content).IsUnicode(false);
        }
    }
}
