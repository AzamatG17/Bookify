using Bookify.Application.Constants;
using Bookify.Domain_.Entities;
using Bookify.Domain_.Enums;
using Bookify.Domain_.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Bookify.Infrastructure.Persistence;

public class ApplicationDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
        //Database.Migrate();
    }

    public DbSet<Companies> Companies { get; set; }
    public DbSet<Services> Services { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(modelBuilder);

        #region Identity

        modelBuilder.Entity<User>(e =>
        {
            e.ToTable("User");
        });

        modelBuilder.Entity<IdentityUserClaim<Guid>>(e =>
        {
            e.ToTable("UserClaim");
        });

        modelBuilder.Entity<IdentityUserLogin<Guid>>(e =>
        {
            e.ToTable("UserLogin");
        });

        modelBuilder.Entity<IdentityUserToken<Guid>>(e =>
        {
            e.ToTable("UserToken");
        });

        modelBuilder.Entity<IdentityRole<Guid>>(e =>
        {
            e.ToTable("Role");
        });

        modelBuilder.Entity<IdentityRoleClaim<Guid>>(e =>
        {
            e.ToTable("RoleClaim");
        });

        modelBuilder.Entity<IdentityUserRole<Guid>>(e =>
        {
            e.ToTable("UserRole");
        });

        #endregion

        var adminRoleId = Guid.NewGuid();
        var adminUserId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        modelBuilder.Entity<IdentityRole<Guid>>().HasData(
            new IdentityRole<Guid>
            {
                Id = adminRoleId,
                Name = RoleConsts.Admin,
                NormalizedName = RoleConsts.Admin.ToUpper()
            },
            new IdentityRole<Guid>
            {
                Id = userId,
                Name = RoleConsts.User,
                NormalizedName = RoleConsts.User.ToUpper()
            }
        );

        var adminUser = new User
        {
            Id = adminUserId,
            FirstName = "admin",
            LastName = "admin",
            UserName = "admin",
            NormalizedUserName = "ADMIN",
            Email = "admin@example.com",
            NormalizedEmail = "ADMIN@EXAMPLE.COM",
            EmailConfirmed = true,
        };

        modelBuilder.Entity<User>().HasData(adminUser);

        modelBuilder.Entity<IdentityUserRole<Guid>>().HasData(
            new IdentityUserRole<Guid>
            {
                UserId = adminUserId,
                RoleId = adminRoleId
            }
        );
    }
}
