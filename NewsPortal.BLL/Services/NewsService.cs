using AutoMapper;
using NewsPortal.BLL.Entities;
using NewsPortal.BLL.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NewsPortal.BLL.Services
{
    public class NewsService
    {
        public NewsItem Get(int id)
        {
            var newsItem = UnitOfWorkHelper.UnitOfWork.News.Get(id);
            return newsItem;
        }

        public List<NewsItem> GetAll(string filter = "all", string sort = "date", string search = "", bool reverse = false)
        {
            //var mapper = new MapperConfiguration(cfg => cfg.CreateMap<NewsItem, NewsItem>()).CreateMapper();
            //return mapper.Map<IEnumerable<NewsItem>, IEnumerable<NewsItem>>(UnitOfWork.News.GetAll());
            var news = UnitOfWorkHelper.UnitOfWork.News.GetAllByFilter(Filter(filter)).ToList();

            Sort(sort, ref news);
            Search(search, ref news);

            if (reverse)
                news.Reverse();

            return news;
        }

        private Expression<Func<NewsItem, bool>> Filter(string filter)
        {
            DateTime firstDate = DateTime.Today;
            DateTime secondDate = new DateTime(9999, 12, 31, 23, 59, 59);
            switch (filter)
            {
                case "today":
                    firstDate = DateTime.Today;
                    secondDate = DateTime.Today.AddDays(1);
                    break;
                case "yesterday":
                    firstDate = DateTime.Today.AddDays(-1);
                    secondDate = DateTime.Today;
                    break;
                case "week":
                    firstDate = DateTime.Today.AddDays(-7);
                    secondDate = DateTime.Today.AddDays(+1);
                    break;
                default:
                    firstDate = new DateTime(1754, 1, 1, 0, 0, 0);
                    secondDate = new DateTime(9999, 12, 31, 23, 59, 59);
                    break;
            }
            return n => n.PublicationDate.Date >= firstDate && n.PublicationDate.Date <= secondDate;
        }

        private void Sort(string sort, ref List<NewsItem> news)
        {
            Func<NewsItem, object> sortParam = n => n.PublicationDate;
            switch (sort)
            {
                case "title": sortParam = n => n.Title; break;
                case "description": sortParam = n => n.Description; break;
            }
            news.OrderBy(n => n.Title);
        }

        private void Search(string search, ref List<NewsItem> news)
        {
            if (!string.IsNullOrEmpty(search))
                news = news.Where(n => n.Title.ToLower().Contains(search) || n.Description.ToLower().Contains(search)).ToList();
        }

        public void Add(NewsItem newsItem)
        {
            UnitOfWorkHelper.UnitOfWork.News.Add(newsItem);
        }

        public void Update(NewsItem newsItem)
        {
            UnitOfWorkHelper.UnitOfWork.News.Update(newsItem);
        }

        public void Delete(int id)
        {
            UnitOfWorkHelper.UnitOfWork.News.Delete(id);
        }
    }
}
