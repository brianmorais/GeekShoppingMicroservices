using AutoMapper;

namespace GeekShopping.CartApi.Config
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMapps()
        {
            var mappingConfig = new MapperConfiguration(config => 
            {
                
            });

            return mappingConfig;
        }
    }
}