using AutoMapper;
using Bookify.Application.DTOs;
using Bookify.Domain_.Entities;

namespace Bookify.Application.Mappings;

public class PredefinedTextMapper : Profile
{
    public PredefinedTextMapper()
    {
        CreateMap<PredefinedText, PredefinedTextDto>();
        CreateMap<PredefinedTextDto, PredefinedText>();
        CreateMap<PredefinedTextForCreateDto, PredefinedText>();
    }
}
