using Bookify.Domain_.Enums;
using Microsoft.AspNetCore.Identity;

namespace Bookify.Domain_.Entities;

public class User : IdentityUser<Guid>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public override string? Email { get; set; }
    public DateTime BirthDate { get; set; }
    public Gender Gender { get; set; }  
}
