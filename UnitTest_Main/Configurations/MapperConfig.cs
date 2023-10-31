using AutoMapper;
using RappidPayTest.Application.Mapping;


namespace RappidPayTest.UnitTest_Main.Configurations
{
    public static class MapperConfig
    {

        public static IMapper GetMappers()
        {
            return new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new BaseMappings());//Your autoMappers

            }).CreateMapper();

        }

    }
}
