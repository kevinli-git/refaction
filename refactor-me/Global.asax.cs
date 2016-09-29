using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using System.Web.Routing;
using refactor_me.Tracing;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]

namespace refactor_me
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);

            GlobalConfiguration.Configuration.Services.Add(typeof(IExceptionLogger), new GlobalExceptionLogger());
            GlobalConfiguration.Configuration.Services.Replace(typeof(IExceptionHandler), new GlobalExceptionHandler());
        }
    }
}
