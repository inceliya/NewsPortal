using NewsPortal.BLL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewsPortal.ViewModels
{
    public class IndexViewModel
    {
        public IEnumerable<NewsViewModel> NewsViewModels { get; set; }
        public PageInfo PageInfo { get; set; }
    }
}