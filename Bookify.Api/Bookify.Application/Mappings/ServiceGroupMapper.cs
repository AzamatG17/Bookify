using AutoMapper;
using Bookify.Application.DTOs;
using Bookify.Application.Requests.Services;
using Bookify.Domain_.Entities;

namespace Bookify.Application.Mappings;

internal sealed class ServiceGroupMapper : Profile
{
    public ServiceGroupMapper()
    {
        CreateMap<ServiceGroup, ServiceGroupDto>();
        CreateMap<ServiceGroupDto, ServiceGroup>();
        CreateMap<ServiceGroupRequest, ServiceGroup>();
    }
}
