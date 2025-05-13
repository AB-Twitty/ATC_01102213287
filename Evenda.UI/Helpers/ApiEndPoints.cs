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


        public const string GET_PAGINATED_EVENTS = "Events/paginated?page={0}&pageSize={1}";
        public const string GET_EVENT_DETAILS = "Events/{0}";
        public const string CREATE_EVENT = "Events/new";

        public const string GET_TAGS = "Tags?in-use={0}";
    }
}
