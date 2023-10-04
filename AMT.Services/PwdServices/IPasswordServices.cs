using AMT.UserRepository.Model;

namespace AMT.Services.PwdServices
{
    public interface IPasswordServices
    {
        Password CreatePassword(Guid userId, string password, HashingAlgorithm hashingAlgorithm);
    }
}
