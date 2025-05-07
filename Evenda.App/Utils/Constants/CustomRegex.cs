using System.Text.RegularExpressions;

namespace Evenda.App.Utils.Constants
{
    public static partial class CustomRegex
    {
        private const string EMAIL = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
        private const string PASSWORD = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{8,}$";

        [GeneratedRegex(EMAIL)]
        public static partial Regex EmailRegex();

        [GeneratedRegex(PASSWORD)]
        public static partial Regex PasswordRegex();
    }
}
