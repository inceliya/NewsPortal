﻿using NewsPortal.BLL.Entities;
using NewsPortal.BLL.Repositories;
using NewsPortal.CL.Services;
using NewsPortal.BLL.UnitOfWork;
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
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using NewsPortal.BLL.Helpers;

namespace NewsPortal.Controllers
{
    [Culture]
    [ExceptionLogger]
    public class NewsController : Controller
    {
        private NewsService NewsService;
        private CommentService CommentService;

        public NewsController(IUnitOfWorkFactory unitOfWorkFactory, INewsRepository newsRepository, ICommentRepository commentRepositpry, ILuceneHelper luceneHelper)
        {
            NewsService = new NewsService(unitOfWorkFactory, newsRepository, luceneHelper);
            CommentService = new CommentService(unitOfWorkFactory, commentRepositpry);
        }

        [HttpGet]
        public ActionResult Index(PanelViewModel panel)
        {
            int pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["PageSizing"]); 

            var news = NewsService.GetAll(panel.Filter, panel.Sort, panel.Search, panel.Reverse);
            news = news.Where(n => n.Visibility).ToList();
            var newsViewModel = new List<NewsViewModel>();
            
            foreach (var newsItem in news)
            {
                var newsItemViewModel = new NewsViewModel(newsItem, CommentService.GetByNewsId(newsItem.Id));
                newsViewModel.Add(newsItemViewModel);
            }

            IEnumerable<NewsViewModel> NewsPerPages = newsViewModel.Skip((panel.Page * pageSize)).Take(pageSize);
            PageInfo pageInfo = new PageInfo { PageNumber = panel.Page+1, PageSize = pageSize, TotalItems = newsViewModel.Count };
            IndexViewModel ivm = new IndexViewModel { PageInfo = pageInfo, NewsViewModels = NewsPerPages };
            return View(ivm);
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

        public ActionResult ChangeCulture(string language, string returnUrl)
        {
            List<string> cultures = new List<string>() { "uk", "en", "ru" };
            if (!cultures.Contains(language))
            {
                language = "en";
            }
            if (returnUrl != null && returnUrl.Length > 1)
            {
                string newUrl;
                Regex rg = new Regex("uk|en|ru");
                Match match = rg.Match(returnUrl);
                if(!string.IsNullOrEmpty(match.Value))
                {
                    newUrl = returnUrl.Replace(match.Value, language);
                    
                }
                else
                {
                    newUrl = $"/{language}" + returnUrl;
                }
                return Redirect(newUrl);
            }
            return RedirectToAction("", new { language });
        }
    }
}