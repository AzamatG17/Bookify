using Bookify.Application.Models;

namespace Bookify.Application.Interfaces.IStores;

public interface IBranchStore
{
    Task<List<Branches>> GetAllAsync(string baseUrl);
}
