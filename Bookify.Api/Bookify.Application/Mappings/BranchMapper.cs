using AutoMapper;
using Bookify.Application.Models;
using Bookify.Domain_.Entities;

namespace Bookify.Application.Mappings;

public class BranchMapper : Profile
{
    public BranchMapper()
    {
        CreateMap<Branches, Branch>();
        CreateMap<Branch, Branches>();
    }
}
