using Owin;

namespace MT.Logging
{
    public static class HttpLoggingBuilderExtensions
    {
        public static IAppBuilder UseHttpLogging(this IAppBuilder builder, HttpLoggingOptions options)
        {
            return builder.Use<HttpLogging>(options);
        }
    }
}
