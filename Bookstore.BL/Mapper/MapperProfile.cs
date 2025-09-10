using AutoMapper;
using Bookstore.BL.Dto;
using Bookstore.BL.Dto.Book;
using Bookstore.BL.Dto.File;
using Bookstore.DAL.Entities;
using File = Bookstore.DAL.Entities.File;

namespace Bookstore.BL.Mapper;

public class MapperProfile: Profile
{
    public MapperProfile()
    {
        CreateMap<File, FileDto>()
            .ForMember(f => f.Size,
                opt => opt.MapFrom(src => src.FileSize))
            .ForMember(f => f.Extension, opt => opt.MapFrom(src => src.FileType))
            .ReverseMap();

        CreateMap<Author, AuthorDto>().ReverseMap();

        CreateMap<Book, BookDto>().ReverseMap();
    }
}
