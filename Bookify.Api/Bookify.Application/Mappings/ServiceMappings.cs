using AutoMapper;
using Bookify.Application.DTOs;
using Bookify.Domain_.Entities;

namespace Bookify.Application.Mappings;

public class ServiceMappings : Profile
{
    public ServiceMappings()
    {
        CreateMap<Service, ServiceDto>();
        CreateMap<ServiceDto, Service>();
    }
}
