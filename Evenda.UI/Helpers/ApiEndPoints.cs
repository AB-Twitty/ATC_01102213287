namespace Evenda.UI.Helpers
{
    public static class ApiEndPoints
    {
        private static string _uri = string.Empty;

        public static string URI
        {
            get => _uri;
            set
            {
                if (!string.IsNullOrEmpty(_uri))
                    throw new ArgumentException("URI cannot be set again.", nameof(value));
                _uri = value;
            }
        }

        public const string REGISTER = "Auth/register";
        public const string LOGIN = "Auth/login";
        public const string REFRESH_TOKEN = "Auth/refresh-token";
        public const string FORGOT_PASSWORD = "Auth/forgot-password";
        public const string RESET_PASSWORD = "Auth/reset-password";


        public const string GET_PAGINATED_EVENTS = "Events/paginated?page={0}&sz={1}&sort={2}&dir={3}";
        public const string GET_FILTERED_EVENTS_PAGINATED = "Events/filter/paginated?page={0}&sz={1}&sort={2}&dir={3}&include-thumbnail={4}";
        public const string GET_EVENT_DETAILS = "Events/{0}";
        public const string CREATE_EVENT = "Events/new";
        public const string EDIT_EVENT = "Events/edit/{0}";
        public const string CANCEL_EVENT = "Events/cancel/{0}";

        public const string GET_TAGS = "Tags?in-use={0}";

        public const string GET_CATEGORIES = "Events/categories?in-use={0}";

        public const string BOOK_EVENT = "Tickets/book";
        public const string GET_MY_BOOKINGS = "Tickets/my-bookings";
        public const string CANCEL_BOOKING = "Tickets/cancel-booking/{0}";
    }
}
