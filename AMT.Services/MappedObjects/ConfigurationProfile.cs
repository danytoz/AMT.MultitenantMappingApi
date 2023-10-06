using AMT.UserRepository.Model;
using AutoMapper;

namespace AMT.Services.MappedObjects
{
    public class ConfigurationProfile : Profile
    {
        /// <summary>
        /// Setup the mapping between the entities and the DTOs
        /// </summary>
        public ConfigurationProfile()
        {
            CreateMap<Password, PasswordDto>();
            CreateMap<User, UserDto>();
            CreateMap<ChatMessage, ChatMessageDto>();
        }
    }
}
