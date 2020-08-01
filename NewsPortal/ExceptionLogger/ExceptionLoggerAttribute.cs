using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;


namespace NewsPortal.ExceptionLogger
{
    public class ExceptionLoggerAttribute : FilterAttribute, IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            ExceptionDetail exceptionDetail = new ExceptionDetail()
            {
                ExceptionMessage = filterContext.Exception.Message,
                StackTrace = filterContext.Exception.StackTrace,
                ControllerName = filterContext.RouteData.Values["controller"].ToString(),
                ActionName = filterContext.RouteData.Values["action"].ToString(),
                Date = DateTime.Now
            };

            string path = HostingEnvironment.MapPath(ConfigurationManager.AppSettings["ExceptionLoggerPath"].ToString());

            using (StreamWriter sw = new StreamWriter(path, true, System.Text.Encoding.Default))
            {
                sw.WriteLine();
                sw.WriteLine(exceptionDetail.ExceptionMessage);
                sw.WriteLine(exceptionDetail.StackTrace);
                sw.WriteLine(exceptionDetail.ControllerName);
                sw.WriteLine(exceptionDetail.ActionName);
                sw.WriteLine(exceptionDetail.Date);
            }
        }
    }
}