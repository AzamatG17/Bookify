namespace Bookify.Application.Interfaces;

public interface ICurrentUserService
{
    Guid GetUserId();
    string GetUserName();
}
