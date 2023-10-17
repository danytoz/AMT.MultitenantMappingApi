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
}
