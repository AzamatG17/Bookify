using Bogus;
using Bookify.Domain_.Entities;
using Bookify.Domain_.Enums;
using Bookify.Domain_.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Bookify.Api.Helpers;

public static class DatabaseSeeder
{
    private static readonly Faker _faker = new();

    public static void SeedDatabase(IApplicationDbContext context)
    {
        CreateRoles(context);
        CreateAdminUser(context);

        context.SaveChanges();
    }

    private static void CreateAdminUser(IApplicationDbContext context)
    {
        if (context.Users.Any())
        {
            return;
        }

        var adminUser = new User
        {
            Email = "azamatgiasov04@gmail.com",
            UserName = "Admin",
            BirthDate = new DateTime(1990, 1, 1),
            Gender = Gender.Male,
        };

        context.Users.Add(adminUser);

        var adminRole = context.Roles.FirstOrDefault(r => r.Name == "Admin");
        if (adminRole != null)
        {
            context.UserRoles.Add(new IdentityUserRole<Guid>
            {
                UserId = adminUser.Id,
                RoleId = adminRole.Id
            });
        }
    }

    private static void CreateRoles(IApplicationDbContext context)
    {
        if (context.Roles.Any())
        {
            return;
        }

        var roles = new[]
        {
            new IdentityRole<Guid> { Id = Guid.NewGuid(), Name = "Admin"},
            new IdentityRole<Guid> { Id = Guid.NewGuid(), Name = "User"}
        };

        context.Roles.AddRange(roles);
    }
}
