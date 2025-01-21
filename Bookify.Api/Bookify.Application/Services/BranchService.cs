using Bookify.Application.Interfaces.IStores;
using Bookify.Application.Models;
using Bookify.Domain_.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Bookify.Application.Services;

internal sealed class BranchService
{
    private readonly IApplicationDbContext _context;
    private readonly IBranchStore _branchStore;

    public BranchService(IApplicationDbContext context, IBranchStore branchStore)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _branchStore = branchStore ?? throw new ArgumentNullException(nameof(branchStore));
    }

    public async Task<List<Branches>> GetAllAsync()
    {
        var companies = await _context.Companies.ToListAsync();

        List<Branches> result = [];
        foreach (var company in companies)
        {
            var branches = await _branchStore.GetAllAsync(company.BaseUrl);
            result.AddRange(branches);
        }

        return result;
    }
}
