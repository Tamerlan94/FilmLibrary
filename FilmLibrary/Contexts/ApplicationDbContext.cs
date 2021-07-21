using FilmLibrary.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmLibrary.Contexts
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public virtual DbSet<Film> Films { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            #region CreateDefaultAdmin\UserAndRole

            Guid ADMIN_ID = Guid.NewGuid();
            Guid ROLE_ID = ADMIN_ID;
            const string password = "admin123";

            builder.Entity<ApplicationRole>().HasData(
                new ApplicationRole("admin")
                {
                    Id = ROLE_ID,
                    Name = "admin",
                    NormalizedName = "ADMIN"
                },
                new ApplicationRole("member") 
                { 
                    Id = Guid.NewGuid(),
                    Name = "member",
                    NormalizedName = "MEMBER"
                });

            var hasher = new PasswordHasher<ApplicationUser>();

            builder.Entity<ApplicationUser>().HasData(new ApplicationUser
            {
                Id = ADMIN_ID,
                UserName = "admin",
                NormalizedUserName = "ADMIN",
                Email = "admin@mail.ru",
                NormalizedEmail = "ADMIN@MAIL.RU",
                Name = "Admin",
                Lastname ="Admin",
                Birthdate = new DateTime(2000, 1, 1),
                EmailConfirmed = true,
                PasswordHash = hasher.HashPassword(null, password),
                SecurityStamp = new Guid().ToString("D")
            });

            builder.Entity<IdentityUserRole<Guid>>().HasData(new IdentityUserRole<Guid>
            {
                RoleId = ROLE_ID,
                UserId = ADMIN_ID
            });

            #endregion

            builder.Entity<Film>()
                .HasOne(x => x.User)
                .WithMany(x => x.Films)
                .OnDelete(DeleteBehavior.NoAction);      

            base.OnModelCreating(builder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            if (!builder.IsConfigured)
            {
                builder.UseSqlServer("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename='|DataDirectory|\\FilmDb.mdf';Integrated Security=True;Connect Timeout=30");
            }
            base.OnConfiguring(builder);
        }
    }
}
