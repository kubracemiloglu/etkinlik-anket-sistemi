using EtkinlikAnketSistemi.Models;
using Microsoft.EntityFrameworkCore;

namespace EtkinlikAnketSistemi.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<AdminUser> AdminUsers { get; set; }
        public DbSet<EventSurvey> EventSurveys { get; set; }
        public DbSet<SurveyQuestion> SurveyQuestions { get; set; }
        public DbSet<SurveyResponse> SurveyResponses { get; set; }
        public DbSet<SurveyAnswer> SurveyAnswers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AdminUser>()
                .HasIndex(x => x.Email)
                .IsUnique();

            modelBuilder.Entity<SurveyQuestion>()
                .HasOne(x => x.EventSurvey)
                .WithMany(x => x.Questions)
                .HasForeignKey(x => x.EventSurveyId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<SurveyResponse>()
                .HasOne(x => x.EventSurvey)
                .WithMany()
                .HasForeignKey(x => x.EventSurveyId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<SurveyAnswer>()
                .HasOne(x => x.SurveyResponse)
                .WithMany(x => x.Answers)
                .HasForeignKey(x => x.SurveyResponseId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<SurveyAnswer>()
                .HasOne(x => x.SurveyQuestion)
                .WithMany()
                .HasForeignKey(x => x.SurveyQuestionId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}