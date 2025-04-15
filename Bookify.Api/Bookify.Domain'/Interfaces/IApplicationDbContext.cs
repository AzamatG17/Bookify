using Bookify.Domain_.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Bookify.Domain_.Interfaces;

public interface IApplicationDbContext
{
    DbSet<PredefinedText> PredefinedTexts { get; set; }
    DbSet<ServiceRating> ServiceRatings { get; set; }
    DbSet<User> Users { get; set; }
    DbSet<IdentityRole<Guid>> Roles { get; set; }
    DbSet<IdentityUserRole<Guid>> UserRoles { get; set; }
    DbSet<Companies> Companies { get; set; }
    DbSet<Service> Services { get; set; }
    DbSet<Branch> Branches { get; set; }
    DbSet<OpeningTimeBranch> OpeningTimeBranches { get; set; }
    DbSet<ServiceTranslation> ServiceTranslations { get; set; }
    DbSet<Booking> Bookings { get; set; }
    DbSet<ETicket> Etickets { get; set; }
    DbSet<ServiceGroup> ServiceGroups { get; set; }
    DbSet<ServiceGroupTranslation> ServiceGroupTranslations { get; set; }

    DatabaseFacade Database { get; }
    int SaveChanges();
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
