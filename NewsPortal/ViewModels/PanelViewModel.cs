using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NewsPortal.ViewModels
{  
    [ModelBinder(typeof(PanelModelBinder))]
    public class PanelViewModel
    {
        public int Page { get; set; }
        public string Filter { get; set; }
        public string Sort { get; set; }
        public string Search { get; set; }
        public bool Reverse { get; set; }
    }
}