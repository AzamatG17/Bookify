using Bookify.Application.Models;

namespace Bookify.Application.Interfaces.IStores;

public interface IBranchStore
{
    Task<List<Branches>> GetAllAsync(string baseUrl);
    Task<List<Branches>> GetAllForOnlinetAsync(string baseUrl);
}
