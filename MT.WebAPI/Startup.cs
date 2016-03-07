using System;
using System.IO;
using System.Web.Http;
using Microsoft.Owin.Cors;
using MT.Logging;
using MT.WebAPI.IoC;
using Owin;
using MT.WebAPI.HttpPipeline;
using Newtonsoft.Json.Serialization;

namespace MT.WebAPI
{
    public class Startup
    {
        public void Configuration(IAppBuilder appBuilder)
        {

            // Configure Web API for self-host. 
            HttpConfiguration configuration = new HttpConfiguration();
            configuration.MapHttpAttributeRoutes();

            // Configure CORS
            appBuilder.UseCors(CorsOptions.AllowAll);

            // Configure Log4net logger
            var log4NetSettings = new FileInfo(Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase.Replace("bin", "Config"), "Log4Net.xml"));
            log4net.Config.XmlConfigurator.Configure(log4NetSettings);

            // Configure HTTP Logging feature
            appBuilder.UseHttpLogging(new HttpLoggingOptions
                {
                    TrackingStore = new HttpLoggingStore(),
                    TrackingIdPropertyName = "x-tracking-id",
                    MaximumRecordedRequestLength = 64 * 1024,
                    MaximumRecordedResponseLength = 64 * 1024
                });

           

            // Resolve dependencies with Autofac
            Bootstrapper.Run(appBuilder, configuration);

            // Set up serialization settings and formatter settings
            var json = configuration.Formatters.JsonFormatter;
            json.SerializerSettings.PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.Objects;
            json.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            configuration.Formatters.Remove(configuration.Formatters.XmlFormatter);

            // Handle all exceptions the same way by returning JSON data (serialized ErrorMessage object)
            configuration.Filters.Add(new ApiExceptionFilterAttribute());
            configuration.Routes.MapHttpRoute("DefaultApi", "api/{controller}/{id}", new { id = RouteParameter.Optional });
            configuration.EnsureInitialized();
            appBuilder.UseWebApi(configuration);
        }
    }
}