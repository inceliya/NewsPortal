using AutoMapper;
using NewsPortal.BLL.Entities;
using NewsPortal.BLL.Repositories;
using NewsPortal.BLL.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NewsPortal.BLL.Services
{
    public class NewsService
    {
        private IUnitOfWorkFactory UnitOfWorkFactory { get; }
        private INewsRepository NewsRepository { get; }

        public NewsService(IUnitOfWorkFactory uowf, INewsRepository newsRepository)
        {
            UnitOfWorkFactory = uowf;
            NewsRepository = newsRepository;
        }

        public NewsItem Get(int id)
        {
            NewsItem newsItem = null;
            using (IUnitOfWork unitOfWork = UnitOfWorkFactory.Create())
            {
                newsItem = NewsRepository.Get(id);
            }
            return newsItem;
        }

        public List<NewsItem> GetAll(string filter = "all", string sort = "date", string search = "", bool reverse = true)
        {
            var news = NewsRepository.GetAllByFilter(Filter(filter)).ToList();

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
            Func<NewsItem, object> sortParam;
            switch (sort)
            {
                case "title":
                    sortParam = n => n.Title.ToLower();
                    break;
                case "description":
                    sortParam = n =>GetText(n.Description.ToLower());
                    break;
                case "date":
                default:
                    sortParam = n => n.PublicationDate;
                    break;
            }
            news.OrderBy(sortParam);
        }
        private string GetText(string text)
        {
            Regex r = new Regex(@"<[^>]*>");
            MatchCollection matches = r.Matches(text);
            for (int i = 0; i < matches.Count; i++)
            {
                int index = text.IndexOf(matches[i].Value.ToString());
                text = text.Remove(index, matches[i].Value.Length);

            }
            return text;
        }

        private void Search(string search, ref List<NewsItem> news)
        {
            if (!string.IsNullOrEmpty(search))
                news = news.Where(n => n.Title.ToLower().Contains(search.ToLower()) || GetText(n.Description.ToLower()).Contains(search.ToLower())).ToList();
        }

        public void Add(NewsItem newsItem)
        {
            using (IUnitOfWork unitOfWork = UnitOfWorkFactory.Create())
            {
                NewsRepository.Add(newsItem);
                unitOfWork.Commit();
            }
        }

        public void Update(NewsItem newsItem)
        {
            using (IUnitOfWork unitOfWork = UnitOfWorkFactory.Create())
            {
                NewsRepository.Update(newsItem);
                unitOfWork.Commit();
            }
        }

        public void Delete(int id)
        {
            using (IUnitOfWork unitOfWork = UnitOfWorkFactory.Create())
            {
                NewsRepository.Delete(id);
                unitOfWork.Commit();
            }
        }
    }
}
