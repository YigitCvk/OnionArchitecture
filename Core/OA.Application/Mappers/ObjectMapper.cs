using AutoMapper;

namespace OA.Application.Mappers;

public static class ObjectMapper
{
    private static readonly Lazy<IMapper> lazy = new Lazy<IMapper>(() =>
    {
        var config = new MapperConfiguration(cfg =>
        {

        });

        return config.CreateMapper();
    });

    public static IMapper Mapper => lazy.Value;
}
