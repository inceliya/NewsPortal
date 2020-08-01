using NewsPortal.BLL.Entities;
using NewsPortal.BLL.Services;
using NewsPortal.ExceptionLogger;
using NewsPortal.Filters;
using NewsPortal.ViewModels;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Event.Default;
using NHibernate.Tool.hbm2ddl;
using PagedList;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace NewsPortal.Controllers
{
    [Culture]
    [ExceptionLogger]
    public class NewsController : Controller
    {
        private NewsService NewsService;
        private CommentService CommentService;

        public NewsController()
        {
            NewsService = new NewsService();
            CommentService = new CommentService();
        }

        [HttpGet]
        public ActionResult Index(int? page = 1, string filter = "all", string sort = "date", string search = "", bool reverse = true)
        {
            //var cfg = new Configuration();
            //cfg.Configure();
            //cfg.AddAssembly(typeof(NewsItem).Assembly);
            //cfg.AddAssembly(typeof(Comment).Assembly);
            //new SchemaExport(cfg).Execute(true, true, false);

            int pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["PageSizing"]); 

            var news = NewsService.GetAll(filter, sort, search, reverse);
            var newsViewModel = new List<NewsViewModel>();

            foreach (var newsItem in news)
            {
                var newsItemViewModel = new NewsViewModel(newsItem, CommentService.GetByNewsId(newsItem.Id));
                if (newsItemViewModel.Visibility)
                    newsViewModel.Add(newsItemViewModel);
            }

            IEnumerable<NewsViewModel> NewsPerPages = newsViewModel.Skip(((int)page - 1) * pageSize).Take(pageSize);
            PageInfo pageInfo = new PageInfo { PageNumber = (int)page, PageSize = pageSize, TotalItems = newsViewModel.Count };
            IndexViewModel ivm = new IndexViewModel { PageInfo = pageInfo, NewsViewModels = NewsPerPages };
            return View(ivm);

            //return View(newsViewModel.ToPagedList(pageNumber, pageSize));


        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            var newsItem = NewsService.Get(id);

            var newsItemViewModel = new NewsViewModel(newsItem, CommentService.GetByNewsId(newsItem.Id));

            if (newsItemViewModel == null)
            {
                return HttpNotFound();
            }

            return View(newsItemViewModel);
        }

        public ActionResult ChangeCulture(string language)
        {
            List<string> cultures = new List<string>() { "uk", "en", "ru" };
            if (!cultures.Contains(language))
            {
                language = "en";
            }
            return RedirectToAction("", new { language });
        }
    }
}