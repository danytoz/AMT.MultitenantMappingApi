

namespace AMT.Services.MappedObjects
{
    public class UserDto
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public bool MFAEnabled { get; set; }
        public bool Verified { get; set; }
    }
}
