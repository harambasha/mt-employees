namespace MT.WebAPI.Helpers
{
    internal static class ProgramConstants
    {
        public const string BASE_ADRESS = "http://localhost:9000/";
        public const string EMPLOYEE_ENDPOINT = "api/employees";

        public const string HTTP_CLIENT_ERROR_MESSAGE =
            "Http Client cannot connect to service, check your configuration";

        public const string RESULT = "Service connection content results: ";
        public const string STOP_AND_CLOSE_MESSAGE = "Press ENTER if you want to stop the server and close app...";
    }
}