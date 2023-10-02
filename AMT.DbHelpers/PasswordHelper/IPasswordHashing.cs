
namespace AMT.DbHelpers.PasswordHelper
{
    public interface IPasswordHashing
    {
        string HashPassword(string password, int? workFactor);
        bool VerifyPassword(string password, string hashedPassword);
    }
}
