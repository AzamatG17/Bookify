using AutoMapper;
using AutoMapper.QueryableExtensions;
using Bookify.Application.DTOs;
using Bookify.Application.Interfaces.Services;
using Bookify.Application.Requests.Services;
using Bookify.Domain_.Entities;
using Bookify.Domain_.Exceptions;
using Bookify.Domain_.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Bookify.Application.Services;

public sealed class PredefinedTextService : IPredefinedTextService
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public PredefinedTextService(IApplicationDbContext context, IMapper mapper)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context)); 
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<List<PredefinedTextDto>> GetAllAsync()
    {
        var texts = await _context.PredefinedTexts
            .AsNoTracking()
            .ProjectTo<PredefinedTextDto>(_mapper.ConfigurationProvider)
            .ToListAsync();

        return texts ?? [];
    }

    public async Task<PredefinedTextDto> CreateAsync(PredefinedTextForCreateDto textDto)
    {
        ArgumentNullException.ThrowIfNull(textDto);

        var predefinedText = _mapper.Map<PredefinedText>(textDto);

        _context.PredefinedTexts.Add(predefinedText);
        await _context.SaveChangesAsync();

        return _mapper.Map<PredefinedTextDto>(predefinedText);
    }

    public async Task<PredefinedTextDto> UpdateAsync(PredefinedTextDto textDto)
    {
        ArgumentNullException.ThrowIfNull(textDto);

        var predefinedText = await _context.PredefinedTexts
            .FindAsync(textDto.Id)
            ?? throw new EntityNotFoundException($"PredefinedText with id:{textDto.Id} does not exist.");

        _mapper.Map(textDto, predefinedText);

        await _context.SaveChangesAsync();

        return _mapper.Map<PredefinedTextDto>(predefinedText);
    }

    public async Task DeleteAsync(PredefinedTextRequest predefinedTextRequest)
    {
        var text = await _context.PredefinedTexts
            .FirstOrDefaultAsync(x => x.Id == predefinedTextRequest.Id)
            ?? throw new EntityNotFoundException($"PredefinedText with id:{predefinedTextRequest.Id} does not exist.");

        _context.PredefinedTexts.Remove(text);
        await _context.SaveChangesAsync();
    }
}
