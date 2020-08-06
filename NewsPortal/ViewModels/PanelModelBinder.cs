using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NewsPortal.ViewModels
{
    public class PanelModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var request = controllerContext.HttpContext.Request;
            var model = new PanelViewModel();
            int tmp; 
           if(!string.IsNullOrEmpty(request.Params["page"]) && int.TryParse(request.Params["page"], out tmp))
            {
                model.Page=tmp-1;
            }
            else
            {
                model.Page = 0;
            }
            model.Filter = (string.IsNullOrEmpty(request.Params["filter"])) ? "all" : request.Params["filter"];
            model.Sort = (string.IsNullOrEmpty(request.Params["sort"])) ? "date" : request.Params["sort"];
            model.Search =(string.IsNullOrEmpty(request.Params["search"])) ? "" : request.Params["search"];
            model.Reverse =((request.Params["reverse"]?? "").ToLower() == "true");
            return model;
        }
    }
}