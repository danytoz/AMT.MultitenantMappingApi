using AMT.Services.MappedObjects.Response;

namespace AMT.Services.MappedObjects
{
    public class UserDto: CustomResponseV1
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public bool MFAEnabled { get; set; }
        public bool Verified { get; set; }
    }

    public class CreateUserDtoIn
    {
        public string Username { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public DateTime BirthDate { get; set; }
    }
}
