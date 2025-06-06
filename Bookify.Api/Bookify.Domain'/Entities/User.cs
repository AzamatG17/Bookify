﻿using Microsoft.AspNetCore.Identity;

namespace Bookify.Domain_.Entities;

public class User : IdentityUser<Guid>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public long ChatId { get; set; }
    public string Language { get; set; }

    public virtual ICollection<ServiceRating> ServiceRatings { get; set; }
    public virtual ICollection<Booking> Bookings { get; set; }
    public virtual ICollection<ETicket> ETickets { get; set; }
}
