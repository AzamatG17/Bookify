using Bookify.Domain_.Entities;

namespace Bookify.Application.Interfaces.IStores;

public interface IBranchStore
{
    Task<List<Branch>> GetAllAsync(Companies companies);
    Task<List<Branch>> GetAllForOnlinetAsync(Companies companies);
}
