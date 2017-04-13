using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace CtrlPVALeasing
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //ModelBinders.Binders.Add(typeof(DateTime?), new DateTimeModelBinder());
            //ModelBinders.Binders.Add(typeof(DateTime), new DateTimeModelBinder());
        }
        //public class DateTimeModelBinder : DefaultModelBinder
        //{
        //    public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        //    {
        //        object result = null;

        //        string modelName = bindingContext.ModelName;
        //        string attemptedValue = bindingContext.ValueProvider.GetValue(modelName).AttemptedValue;

        //        if (String.IsNullOrEmpty(attemptedValue))
        //        {
        //            return result;
        //        }

        //        try
        //        {
        //            result = DateTime.Parse(attemptedValue);
        //        }
        //        catch (FormatException e)
        //        {
        //            bindingContext.ModelState.AddModelError(modelName, e);
        //        }

        //        return result;
        //    }
        //}
    }
}
