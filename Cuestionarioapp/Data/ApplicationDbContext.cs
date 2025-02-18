using Cuestionarioapp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Cuestionarioapp.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Question> Questions { get; set; }
        public DbSet<UserResponse> UserResponses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed initial 
            var seedDate = new DateTime(2025, 1, 1);

            modelBuilder.Entity<Question>().HasData(
                new Question
                {
                    Id = 1,
                    Text = "¿Qué es un delegado (delegate) en .NET y cómo se utiliza?",
                    CreatedAt = seedDate
                },
                new Question
                {
                    Id = 2,
                    Text = "¿Qué es LINQ y cómo se utiliza en .NET? Proporcione un ejemplo.",
                    CreatedAt = seedDate
                },
                new Question
                {
                    Id = 3,
                    Text = "Explique el patrón de diseño Singleton y nombre tres patrones más.",
                    CreatedAt = seedDate
                },
                new Question
                {
                    Id = 4,
                    Text = "¿Cuáles son los Principios SOLID? Explique cada uno brevemente.",
                    CreatedAt = seedDate
                },
                new Question
                {
                    Id = 5,
                    Text = "¿Cómo se implementa la herencia múltiple en .NET?",
                    CreatedAt = seedDate
                }
            );

            // Relación entre UserResponse y Question
            modelBuilder.Entity<UserResponse>()
                .HasOne(ur => ur.Question)
                .WithMany(q => q.UserResponses)
                .HasForeignKey(ur => ur.QuestionId);
        }


    }
}
