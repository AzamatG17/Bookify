namespace Bookify.Application.Interfaces.IServices;

public interface ICurrentUserService
{
    Guid GetUserId();
    string GetUserName();
}
