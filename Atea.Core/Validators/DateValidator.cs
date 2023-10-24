namespace Atea.Core.Validators
{
    public class DateValidator
    {
        private string _from;

        private string _to;

        public string ErrorMessage;

        public DateValidator(string from, string to)
        {
            _from = from;
            _to = to;
            ErrorMessage = string.Empty;
        }

        public bool IsValid()
        {
            if (IsEmpty())
            {
                ErrorMessage = "Date From and Date To is required!";
                return false;
            }

            if (!IsValidDateTime(_from))
            {
                ErrorMessage = "Date From is not a valid date!";
                return false;
            }

            if (!IsValidDateTime(_to))
            {
                ErrorMessage = "Date To is not a valid date!";
                return false;
            }

            if (!IsValidInterval())
            {
                ErrorMessage = "Date interval is not valid!";
                return false;
            }

            return true;
        }

        private bool IsEmpty()
        {
            return string.IsNullOrEmpty(_from) || string.IsNullOrEmpty(_to);
        }

        private bool IsValidDateTime(string value)
        {
            DateTime date;

            DateTime.TryParse(value, out date);

            return date != default;
        }

        private bool IsValidInterval()
        {
            DateTime.TryParse(_from, out var from);
            DateTime.TryParse(_to, out var to);

            return from < to;
        }
    }
}
