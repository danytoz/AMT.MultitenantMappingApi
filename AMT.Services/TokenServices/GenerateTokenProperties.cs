
namespace AMT.Services.TokenServices
{
    public class GenerateTokenProperties
    {
        public string Name { get; set; }
        public string UserId { get; set; }
        public List<string> Roles { get; set; }
    }
}
