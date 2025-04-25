using AutoMapper;
using Bookify.Application.DTOs;
using Bookify.Domain_.Entities;

namespace Bookify.Application.Mappings;

public class ServiceRatingMapper : Profile
{
    public ServiceRatingMapper()
    {
        CreateMap<ServiceRating, ServiceRatingDto>();
        CreateMap<ServiceRatingForCreateDto, ServiceRating>();
        CreateMap<ServiceRatingForUpdateDto, ServiceRating>();
    }
}
