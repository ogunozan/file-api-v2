using AutoMapper;
using Dto;
using Dal;

namespace Dto
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<File, FileDto>();

            CreateMap<RequestAddFileDto, File>();
        }
    }
}
