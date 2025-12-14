using HolookorBackend.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HolookorBackend.Infrastructure.Context
{
    public class HolookorSystem : DbContext
    {
        public HolookorSystem(DbContextOptions<HolookorSystem> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Tutor> Tutors { get; set; }
        public DbSet<Parent> Parents { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<TutorReview> TutorReviews { get; set; }
        public DbSet<EmailVerification> EmailVerifications { get; set; }




        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>()
                .HasKey(a => a.Id);
            modelBuilder.Entity<User>()
                .HasKey(a => a.Id);
            modelBuilder.Entity<Parent>()
                .HasKey(a => a.Id);
            modelBuilder.Entity<Tutor>()
                .HasKey(a => a.Id);
            modelBuilder.Entity<UserProfile>()
                .HasKey(a => a.Id);
            modelBuilder.Entity<TutorReview>()
                .HasKey(a => a.Id);
            modelBuilder.Entity<EmailVerification>()
                .HasKey(a => a.Id);
        }
    }
}
