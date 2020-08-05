using NewsPortal.BLL.Entities;
using NewsPortal.BLL.IServices;
using NewsPortal.BLL.Services;
using NewsPortal.ExceptionLogger;
using NewsPortal.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace NewsPortal.Controllers
{
    public class CommentController : Controller
    {
        private INewsService NewsService { get; }
        private ICommentService CommentService { get; }

        public CommentController(INewsService ns, ICommentService cs)
        {
            NewsService = ns;
            CommentService = cs;
        }

        [HttpGet]
        [ExceptionLogger]
        public ActionResult GetComments(int id)
        {
            var newsItem = NewsService.Get(id);
            var newsItemVewModel = new NewsViewModel(newsItem, CommentService.GetByNewsId(newsItem.Id));

            if (newsItemVewModel == null)
            {
                return HttpNotFound();
            }
            return PartialView("_CommentsListPartial", newsItemVewModel.Comments.Reverse());
        }

        [HttpPost]
        [ExceptionLogger]
        public ActionResult AddComment(string author, string text, int id, string controllerName)
        {
            //var repository = new NewsRepository();
            //var news = repository.GetNewsById(id);

            var newsItem = NewsService.Get(id);

            var comment = new Comment()
            {
                Author = author,
                Text = text,
                CreationDate = DateTime.UtcNow,
                NewsId = id
            };

            Regex r = new Regex(@"<[^>]*>");
            if (!string.IsNullOrEmpty(r.Match(comment.Author).Value) || !string.IsNullOrEmpty(r.Match(comment.Text).Value))
                return View("Error");

            CommentService.Add(comment);

            return RedirectToAction("Details", controllerName, new { id = newsItem.Id });
        }

        [HttpGet]
        [ExceptionLogger]
        public ActionResult DeleteComment(int newsId, int commentId)
        {
            CommentService.Delete(commentId);
            return RedirectToAction("Details", "Admin", new { id = newsId });
        }
    }
}