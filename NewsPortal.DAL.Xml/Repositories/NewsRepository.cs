using NewsPortal.BLL.Entities;
using NewsPortal.BLL.IRepositories;
using System;
using System.Collections.Generic;
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
        private string FilePath = HttpContext.Current.Server.MapPath("~/App_Data/XmlData/NewsData.xml");
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
                               Title = t.Element("title").Value,
                               Description = t.Element("description").Value,
                               Image = t.Element("image").Value,
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
                newsItem.Id = (int)(from S in ItemsData.Descendants("item") orderby (int)S.Element("id") descending select (int)S.Element("id")).FirstOrDefault() + 1;
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
            XElement node = ItemsData.Root.Elements("item").Where(i => (int)i.Element("id") == newsItem.Id).FirstOrDefault();

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
