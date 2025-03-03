using Bookify.Application.DTOs;
using Bookify.Application.Interfaces.IServices;
using Bookify.Application.Interfaces.IStores;
using Bookify.Application.Requests.Services;
using Bookify.Domain_.Entities;
using Bookify.Domain_.Exceptions;
using Bookify.Domain_.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Xml;

namespace Bookify.Application.Services;

internal sealed class BranchService(IApplicationDbContext context, IBranchStore branchStore) : IBranchService
{
    private readonly IApplicationDbContext _context = context ?? throw new ArgumentNullException(nameof(context));
    private readonly IBranchStore _branchStore = branchStore ?? throw new ArgumentNullException(nameof(branchStore));

    public async Task<List<CompanyWithBranchesDto>> GetAllAsync()
    {
        var branches = await _context.Companies
            .Include(c => c.Branches)
            .ThenInclude(o => o.OpeningTimeBranches)
            .AsNoTracking()
            .ToListAsync();

        var result = branches.Select(c => new CompanyWithBranchesDto(
            c.Id,
            c.Name,
            c.Color,
            c.BackgroundColor,
            c.Branches.Select(b => new BranchDto(
                b.Id,
                b.BranchId,
                b.Name,
                b.BranchAddres,
                b.CoordinateLatitude,
                b.CoordinateLongitude,
                b.Projects,
                b.OpeningTimeBranches.Select(o => new DTOs.OpeningTimeDto(
                        o.Day,
                        o.OpenTime
                        )).ToList() 
                    )).ToList()
                )).ToList();

        return result ?? [];
    }

    public async Task<CompanyWithBranchesDto> GetByIdAsync(CompanyRequest request)
    {
        var branch = await _context.Companies
            .Include(c => c.Branches)
            .ThenInclude(o => o.OpeningTimeBranches)
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == request.Id)
            ?? throw new EntityNotFoundException($"Company with id does not exist.");

        var result = new CompanyWithBranchesDto(
            branch.Id,
            branch.Name,
            branch.Color,
            branch.BackgroundColor,
            branch.Branches?.Select(b => new BranchDto(
                b.Id,
                b.BranchId,
                b.Name,
                b.BranchAddres,
                b.CoordinateLatitude,
                b.CoordinateLongitude,
                b.Projects,
                b.OpeningTimeBranches?.Select(o => new DTOs.OpeningTimeDto(
                    o.Day,
                    o.OpenTime
                )).ToList()
            )).ToList()
        );

        return result;
    }

    public async Task<List<Branch>> UpdateDateAsync(CompanyRequest request)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var branches = await _context.Branches
                .Include(op => op.OpeningTimeBranches)
                .Where(b => b.CompanyId == request.Id)
                .ToListAsync();

            var newBranchesData = await GetByIdsAsync(request.Id);

            foreach (var branch in newBranchesData)
            {
                var result = branches.FirstOrDefault(x => x.BranchId == branch.BranchId);

                if (result is not null)
                {
                    result.BranchId = branch.BranchId;
                    result.Name = branch.Name;
                    result.BranchAddres = branch.BranchAddres;
                    result.CoordinateLatitude = branch.CoordinateLatitude;
                    result.CoordinateLongitude = branch.CoordinateLongitude;

                    _context.OpeningTimeBranches.RemoveRange(result.OpeningTimeBranches);
                    result.OpeningTimeBranches = branch.OpeningTimeBranches;
                }
                else
                {
                    await _context.Branches.AddAsync(branch);
                }
            }

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return newBranchesData;
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();

            throw new InvalidUpdateDataException("Failed to update company branches. See inner exception for details.", ex);
        }
    }

    private async Task<List<Branch>> GetByIdsAsync(int request)
    {
        var company = await _context.Companies
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == request)
            ?? throw new EntityNotFoundException($"Company with id:{request} does not exist.");

        List<Branch> branches = [];

        if (!string.IsNullOrWhiteSpace(company.BaseUrlForBookingService))
        {
            var result = await _branchStore.GetAllAsync(company);
            branches.AddRange(result);
        }

        if (!string.IsNullOrWhiteSpace(company.BaseUrlForOnlinet))
        {
            var result = await _branchStore.GetAllForOnlinetAsync(company);
            branches.AddRange(result);
        }

        return branches;
    }
}
