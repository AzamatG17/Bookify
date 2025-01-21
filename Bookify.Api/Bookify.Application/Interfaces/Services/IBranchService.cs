using Bookify.Application.Models;

namespace Bookify.Application.Interfaces.IServices;

public interface IBranchService
{
    Task<List<Branches>> GetAllAsync();
}
