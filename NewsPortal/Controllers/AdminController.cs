﻿using NewsPortal.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using NewsPortal.ExceptionLogger;
using NewsPortal.Filters;
using System.IO;
using NewsPortal.BLL.Services;
using NewsPortal.BLL.Entities;
using System.Configuration;
using NewsPortal.BLL.IServices;

namespace NewsPortal.Controllers
{
    [Authorize]
    [ExceptionLogger]
    [Culture]
    public class AdminController : Controller
    {
        private INewsService NewsService { get; set; }
        private ICommentService CommentService { get; set; }

        public AdminController(INewsService ns, ICommentService cs)
        {
            NewsService = ns;
            CommentService = cs;
        }

        [HttpGet]
        public ActionResult Index(int? page = 1, string filter = "all", string sort = "date", string search = "", bool reverse = true)
        {
            int pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["PageSizing"]);

            var news = NewsService.GetAll(filter, sort, search, reverse);
            var newsViewModel = new List<NewsViewModel>();

            foreach (var newsItem in news)
            {
                var newsItemViewModel = new NewsViewModel(newsItem, CommentService.GetByNewsId(newsItem.Id));
                newsViewModel.Add(newsItemViewModel);
            }

            IEnumerable<NewsViewModel> NewsPerPages = newsViewModel.Skip(((int)page - 1) * pageSize).Take(pageSize);
            PageInfo pageInfo = new PageInfo { PageNumber = (int)page, PageSize = pageSize, TotalItems = newsViewModel.Count };
            IndexViewModel ivm = new IndexViewModel { PageInfo = pageInfo, NewsViewModels = NewsPerPages };
            return View(ivm);
        }

        [HttpGet]
        [ExceptionLogger]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ExceptionLogger]
        public ActionResult Create(NewsViewModel newsItemViewModel, HttpPostedFileBase newsImg)
        {
            if (newsImg != null)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetFileName(newsImg.FileName);
                var directoryToSave = Server.MapPath(Url.Content("~/Pictures"));

                var pathToSave = Path.Combine(directoryToSave, fileName);
                newsImg.SaveAs(pathToSave);
                newsItemViewModel.Image = "/Pictures/" + fileName;
            }

            if (ModelState.IsValid)
            {
                newsItemViewModel.PublicationDate = newsItemViewModel.PublicationDate.ToUniversalTime();

                var newsItem = new NewsItem()
                {
                    Id = newsItemViewModel.Id,
                    Title = newsItemViewModel.Title,
                    Description = newsItemViewModel.Description,
                    Image = newsItemViewModel.Image,
                    PublicationDate = newsItemViewModel.PublicationDate,
                    Visibility = newsItemViewModel.Visibility
                };

                NewsService.Add(newsItem);
                return RedirectToAction("Index");
            }

            return View("Error");

        }

        [HttpGet]
        [ExceptionLogger]
        public ActionResult Edit(int id)
        {
            var news = NewsService.Get(id);

            var newsViewModel = new NewsViewModel(news, CommentService.GetByNewsId(news.Id));

            if (newsViewModel == null)
            {
                return HttpNotFound();
            }
            return View(newsViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ExceptionLogger]
        public ActionResult Edit(NewsViewModel newsItemViewModel, HttpPostedFileBase newsImg)
        {
            if (newsImg != null && !string.IsNullOrEmpty(newsImg.FileName))
            {
                
                var fileName = Guid.NewGuid().ToString() + Path.GetFileName(newsImg.FileName);
                var directoryToSave = Server.MapPath(Url.Content("~/Pictures"));
                if(!string.IsNullOrEmpty(newsItemViewModel.Image))
                {
                    Directory.GetCurrentDirectory();
                    if (System.IO.File.Exists(directoryToSave+"\\"+ newsItemViewModel.Image.Replace("/Pictures/", "")))
                        System.IO.File.Delete(directoryToSave +"\\"+ newsItemViewModel.Image.Replace("/Pictures/", ""));
                }
                var pathToSave = Path.Combine(directoryToSave, fileName);
                newsImg.SaveAs(pathToSave);
                newsItemViewModel.Image = "/Pictures/" + fileName;
            }

            if (ModelState.IsValid)
            {
                var newsItem = new NewsItem()
                {
                    Id = newsItemViewModel.Id,
                    Title = newsItemViewModel.Title,
                    Description = newsItemViewModel.Description,
                    Image = newsItemViewModel.Image,
                    PublicationDate = newsItemViewModel.PublicationDate,
                    Visibility = newsItemViewModel.Visibility
                };
                NewsService.Update(newsItem);
                return RedirectToAction("Index");
            }
            return View(newsItemViewModel);
        }

        [HttpGet]
        [ExceptionLogger]
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


        [HttpGet]
        [ExceptionLogger]
        public ActionResult Delete(int id)
        {
            var newsItem = NewsService.Get(id);
            var newsItemViewModel = new NewsViewModel(newsItem, CommentService.GetByNewsId(newsItem.Id));

            if (newsItemViewModel == null)
            {
                return HttpNotFound();
            }
            return View(newsItemViewModel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [ExceptionLogger]
        public ActionResult DeleteConfirmed(int id)
        {
            NewsService.Delete(id);
            return RedirectToAction("Index");
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