using AutoMapper;
using Bookify.Application.DTOs;
using Bookify.Application.Requests.Services;
using Bookify.Domain_.Entities;

namespace Bookify.Application.Mappings;

internal sealed class CompaniesMappings : Profile
{
    public CompaniesMappings()
    {
        CreateMap<Companies, CompaniesDto>();
        CreateMap<Companies, CompaniesForAdminDto>();
        CreateMap<CreateCompanyRequest, Companies>();
        CreateMap<UpdateCompanyRequest, Companies>();
    }
}
