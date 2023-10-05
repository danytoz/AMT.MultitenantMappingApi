
namespace AMT.Services.Validators
{
    public interface IPasswwordRequirements
    {
        int MinimumLength { get; }
        int MaximumLength { get; }
        int NumberOfUpperCase { get; }
        int NumberOfLowerCase { get; }
        int NumberOfDigits { get; }
        char[]? BlacklistCharacters { get; }
    }
}
