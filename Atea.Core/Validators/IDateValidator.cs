namespace Atea.Core.Validators
{
    public interface IDateValidator
    {
        string ErrorMessage { get; }

        bool IsValid(string from, string to);
    }
}
