namespace Atea.Core.Validators
{
    public class DateValidator : IDateValidator
    {
        public string ErrorMessage { get; private set; }

        public bool IsValid(string from, string to)
        {
            if (IsEmpty(from, to))
            {
                ErrorMessage = "Date From and Date To is required!";
                return false;
            }

            if (!IsValidDateTime(from))
            {
                ErrorMessage = "Date From is not a valid date!";
                return false;
            }

            if (!IsValidDateTime(to))
            {
                ErrorMessage = "Date To is not a valid date!";
                return false;
            }

            if (!IsValidInterval(from, to))
            {
                ErrorMessage = "Date interval is not valid!";
                return false;
            }

            return true;
        }

        private bool IsEmpty(string from, string to)
        {
            return string.IsNullOrEmpty(from) || string.IsNullOrEmpty(to);
        }

        private bool IsValidDateTime(string value)
        {
            DateTime date;

            DateTime.TryParse(value, out date);

            return date != default;
        }

        private bool IsValidInterval(string from, string to)
        {
            DateTime.TryParse(from, out var fromDateTime);
            DateTime.TryParse(to, out var toDateTime);

            return fromDateTime < toDateTime;
        }
    }
}
