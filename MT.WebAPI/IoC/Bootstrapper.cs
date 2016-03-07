using System.Reflection;
using System.Web.Http;
using Autofac;
using MT.Repository.Repositories;
using Owin;
using Autofac.Integration.WebApi;

namespace MT.WebAPI.IoC
{
    public class Bootstrapper
    {
        public static void Run(IAppBuilder app, HttpConfiguration configuration)
        {

            SetAutofacWebApiServices(app, configuration);
        }
        private static void SetAutofacWebApiServices(IAppBuilder app, HttpConfiguration configuration)
        {
            var builder = new ContainerBuilder();
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            builder.RegisterType<EmployeeRepository>().As<IEmployeeRepository>().InstancePerRequest();

            var container = builder.Build();
            configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container);

            app.UseAutofacWebApi(GlobalConfiguration.Configuration);
        }
    }
}