using Bookify.Domain_.Entities;
namespace Bookify.Application.Interfaces;

public interface IJwtTokenHandler
{
    string GenerateToken(User user, IEnumerable<string> roles);
}
