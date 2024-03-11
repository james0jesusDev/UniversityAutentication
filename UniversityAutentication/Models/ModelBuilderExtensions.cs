using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UniversityAutentication.Models;

public static class ModelBuilderExtensions
{
    public static void Seed(this ModelBuilder modelBuilder)
    {
        //Roles
        List<IdentityRole> roles = new List<IdentityRole>()
        {
            new IdentityRole{Name="Administrator",NormalizedName="ADMINISTRATOR"},
            new IdentityRole{Name="Instructor",NormalizedName="INSTRUCTOR"},
            new IdentityRole{Name="Student",NormalizedName="STUDENT"},
        };
        modelBuilder.Entity<IdentityRole>().HasData(roles);
        //Users
        List<ApplicationUser> users = new List<ApplicationUser>()
        {
            new ApplicationUser
            {
                UserName = "admin@gmail.com",
                NormalizedUserName="ADMIN@GMAIL.COM",
                Email = "admin@gmail.com",
                NormalizedEmail = "ADMIN@GMAIL.COM"
            },
            new ApplicationUser
            {
                UserName = "student@gmail.com",
                NormalizedUserName="STUDENT@GMAIL.COM",
                Email = "student@gmail.com",
                NormalizedEmail = "STUDENT@GMAIL.COM"
            },
            new ApplicationUser
            {
                UserName = "instructor@gmail.com",
                NormalizedUserName="INSTRUCTOR@GMAIL.COM",
                Email = "instructor@gmail.com",
                NormalizedEmail = "INSTRUCTOR@GMAIL.COM"
            }

        };
        modelBuilder.Entity<ApplicationUser>().HasData(users);
        //contraseñas a los usuarios
        var passwordHasher = new PasswordHasher<ApplicationUser>();
        users[0].PasswordHash = passwordHasher.HashPassword(users[0], "$Tajamar00");
        users[1].PasswordHash = passwordHasher.HashPassword(users[1], "$Tajamar00");

        //Agregar rol a usuario
        List<IdentityUserRole<string>> userRoles = new List<IdentityUserRole<string>>();
        userRoles.Add(new IdentityUserRole<string>
        {
            UserId = users[0].Id,
            RoleId = roles.First(q => q.Name == "Administrator").Id
        });
        userRoles.Add(new IdentityUserRole<string>
        {
            UserId = users[1].Id,
            RoleId = roles.First(q => q.Name == "Student").Id
        });
        userRoles.Add(new IdentityUserRole<string>
        {
            UserId = users[2].Id,
            RoleId = roles.First(q => q.Name == "Instructor").Id
        });
        modelBuilder.Entity<IdentityUserRole<string>>().HasData(userRoles);

    }
}