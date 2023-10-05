
namespace AMT.Services.Validators
{
    public class DefaultPasswordRequirements : IPasswwordRequirements
    {
        public int MinimumLength => 6;

        public int MaximumLength => 20;

        public int NumberOfUpperCase => 1;

        public int NumberOfLowerCase => 1;

        public int NumberOfDigits => 1;

        public char[]? BlacklistCharacters => null;
    }
}
