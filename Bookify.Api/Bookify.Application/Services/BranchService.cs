using AutoMapper;
using Bookify.Application.DTOs;
using Bookify.Application.Interfaces.IServices;
using Bookify.Application.Interfaces.IStores;
using Bookify.Application.Models;
using Bookify.Application.Requests.Services;
using Bookify.Domain_.Entities;
using Bookify.Domain_.Exceptions;
using Bookify.Domain_.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Bookify.Application.Services;

internal sealed class BranchService : IBranchService
{
    private readonly IApplicationDbContext _context;
    private readonly IBranchStore _branchStore;
    private readonly IMapper _mapper;

    public BranchService(IApplicationDbContext context, IBranchStore branchStore, IMapper mapper)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _branchStore = branchStore ?? throw new ArgumentNullException(nameof(branchStore));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<List<CompanyWithBranchesDto>> GetAllAsync()
    {
        var companies = await _context.Companies
            .AsNoTracking()
            .ToListAsync();

        var result = new List<CompanyWithBranchesDto>();

        foreach (var company in companies)
        {
            List<Branches>? branches = null;

            if (company.Projects == Domain_.Enums.Projects.BookingService)
            {
                branches = await _branchStore.GetAllAsync(company.BaseUrl);
            }
            else if (company.Projects == Domain_.Enums.Projects.Onlinet)
            {
                branches = await _branchStore.GetAllForOnlinetAsync(company.BaseUrl);
            }

            result.Add(new CompanyWithBranchesDto(
                Id: company.Id,
                Name: company.Name,
                Projects: company.Projects,
                Color: company.Color,
                BackgroundColor: company.BackgroundColor,
                Branches: branches
            ));
        }

        return result;
    }

    public async Task<CompanyWithBranchesDto> GetByIdAsync(CompanyRequest request)
    {
        var company = await _context.Companies
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == request.Id);

        if (company is null)
        {
            throw new EntityNotFoundException($"Company with id:{request.Id} does not exist.");
        }

        List<Branches> branches = new();

        if (company.Projects == Domain_.Enums.Projects.BookingService)
        {
            branches = await _branchStore.GetAllAsync(company.BaseUrl);
        }
        else if (company.Projects == Domain_.Enums.Projects.Onlinet)
        {
            branches = await _branchStore.GetAllForOnlinetAsync(company.BaseUrl);
        }

        return new CompanyWithBranchesDto(
            Id: company.Id,
            Name: company.Name,
            Projects: company.Projects,
            Color: company.Color,
            BackgroundColor: company.BackgroundColor,
            Branches: branches
        );
    }

    public async Task UpdateDateAsync(CompanyRequest request)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var branches = await _context.Branches
                .Where(b => b.CompanyId == request.Id)
                .ToListAsync();

            if (branches.Any())
            {
                _context.Branches.RemoveRange(branches);
            }

            var newBranches = await GetByIdsAsync(request);

            foreach (var branch in newBranches)
            {
                var result = _mapper.Map<Branch>(branch);
                await _context.Branches.AddAsync(result);
            }

            await _context.SaveChangesAsync();

            await transaction.CommitAsync();
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<List<Branches>> GetByIdsAsync(CompanyRequest request)
    {
        var company = await _context.Companies
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == request.Id);

        if (company is null)
        {
            throw new EntityNotFoundException($"Company with id:{request.Id} does not exist.");
        }

        List<Branches> branches = new();

        if (company.Projects == Domain_.Enums.Projects.BookingService)
        {
            branches = await _branchStore.GetAllAsync(company.BaseUrl);
        }
        else if (company.Projects == Domain_.Enums.Projects.Onlinet)
        {
            branches = await _branchStore.GetAllForOnlinetAsync(company.BaseUrl);
        }

        return branches;
    }
}
