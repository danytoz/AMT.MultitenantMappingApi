
namespace AMT.DbHelpers.PasswordHelper
{
    public class BCryptPasswordHashing : IPasswordHashing
    {
        private readonly int workFactor;

        public BCryptPasswordHashing(int workFactor)
        {
            this.workFactor = workFactor;
        }
        public string HashPassword(string password, int? workFactor) => BCrypt.Net.BCrypt.EnhancedHashPassword(password, workFactor?? this.workFactor, BCrypt.Net.HashType.SHA512);

        public bool VerifyPassword(string password, string hashedPassword) => BCrypt.Net.BCrypt.EnhancedVerify(password, hashedPassword, BCrypt.Net.HashType.SHA512);
    }
}
