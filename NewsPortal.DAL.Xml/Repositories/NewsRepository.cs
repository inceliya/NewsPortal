using NewsPortal.BLL.Entities;
using NewsPortal.BLL.Repositories;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Linq;

namespace NewsPortal.DAL.Xml.Repositories
{
    public class NewsRepository : INewsRepository
    {
        private string FilePath = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["NewsXmlFilePath"]);
        protected List<NewsItem> News;
        protected XDocument ItemsData;

        public NewsRepository()
        {
            News = new List<NewsItem>();
            ItemsData = XDocument.Load(FilePath);
            if (!string.IsNullOrEmpty(ItemsData.Root.Value))
            {
                var news = from t in ItemsData.Descendants("item")
                           select new NewsItem()
                           {
                               Id = (int)t.Element("id"),
                               Title = (string)t.Element("title"),
                               Description = (string)t.Element("description"),
                               Image = (string)t.Element("image"),
                               PublicationDate = (DateTime)t.Element("publication_date"),
                               Visibility = (bool)t.Element("visibility")
                           };
                News.AddRange(news.ToList());
            }
        }

        public NewsItem Get(int id)
        {
            return News.Find(n => n.Id == id);
        }

        public IEnumerable<NewsItem> GetAll()
        {
            return News;
        }

        public IEnumerable<NewsItem> GetAllByFilter(Expression<Func<NewsItem, bool>> filter)
        {
            return News.AsQueryable().Where(filter);
        }

        public void Add(NewsItem newsItem)
        {
            if (!string.IsNullOrEmpty(ItemsData.Root.Value))
            {
                newsItem.Id = (int)(ItemsData.Root.Element("max_id")) + 1;
                ItemsData.Root.SetElementValue("max_id", newsItem.Id);
            }
            else
            {
                newsItem.Id = 0;
            }

            ItemsData.Root.Add(new XElement("item",
                new XElement("id", newsItem.Id),
                new XElement("title", newsItem.Title),
                new XElement("description", newsItem.Description),
                new XElement("image", newsItem.Image),
                new XElement("publication_date", newsItem.PublicationDate),
                new XElement("visibility", newsItem.Visibility)));

            ItemsData.Save(FilePath);
        }

        public void Update(NewsItem newsItem)
        {
            XElement node = ItemsData.Root.Elements("item").Where(i => (int)i.Element("id") == newsItem.Id).Single();

            node.SetElementValue("title", newsItem.Title);
            node.SetElementValue("description", newsItem.Description);
            node.SetElementValue("image", newsItem.Image);
            node.SetElementValue("publication_date", newsItem.PublicationDate);
            node.SetElementValue("visibility", newsItem.Visibility);
            ItemsData.Save(FilePath);
        }

        public void Delete(int id)
        {
            ItemsData.Root.Elements("item").Where(i => (int)i.Element("id") == id).Remove();

            ItemsData.Save(FilePath);
        }
    }
}
