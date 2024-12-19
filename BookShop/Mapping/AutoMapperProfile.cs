using AutoMapper;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        // Mapping giữa Stock và StockDTO
        CreateMap<Stock, StockDTO>().ReverseMap();

        // Mapping giữa Book và BookDTO
        CreateMap<Book, BookDTO>().ReverseMap();

        // Mapping giữa Genre và GenreDTO
        CreateMap<Genre, GenreDTO>();
        CreateMap<GenreDTO, Genre>();


    }
}