using NewsPortal.BLL.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NewsPortal.Helpers
{
    public class PictureDelete
    {
        public static void Delete(HttpServerUtilityBase server, UrlHelper url, NewsService newsService, CommentService commentService, int id)
        {
            var newsItem = newsService.Get(id);
            if (newsItem == null) return;
            var directoryToSave = server.MapPath(url.Content("~/Pictures"));
            if (!string.IsNullOrEmpty(newsItem.Image))
            {
                Directory.GetCurrentDirectory();
                if (System.IO.File.Exists(directoryToSave + "\\" + newsItem.Image.Replace("/Pictures/", "")))
                    System.IO.File.Delete(directoryToSave + "\\" + newsItem.Image.Replace("/Pictures/", ""));
            }
        }
    }
}