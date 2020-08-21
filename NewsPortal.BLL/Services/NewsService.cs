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
            NewsRepository.Refresh(GetAll());
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
            
            List<NewsItem> news = null;
            using (IUnitOfWork unitOfWork = UnitOfWorkFactory.Create())
            {
                news = NewsRepository.GetAllByFilter(Filter(filter), Sort(sort), search, reverse).ToList();
            }

            return news;
        }

        private Expression<Func<NewsItem, bool>> Filter(string filter)
        {
            switch (filter)
            {
                case "today":
                    return n => n.PublicationDate.Day == DateTime.Today.Day;
                case "yesterday":
                    return n => n.PublicationDate.Day == DateTime.Today.AddDays(-1).Day;
                case "week":
                    return n => n.PublicationDate.Day >= DateTime.Today.AddDays(-7).Day && n.PublicationDate.Day <= DateTime.Today.Day;
                case "all":
                default:
                    return n => n.PublicationDate <= DateTime.MaxValue;
            }
        }

        private Expression<Func<NewsItem, object>> Sort(string sort)
        {
            Expression<Func<NewsItem, object>> sortParam;
            switch (sort)
            {
                case "title":
                    sortParam = n => n.Title;
                    break;
                case "description":
                    sortParam = n => n.Description;
                    break;
                case "date":
                default:
                    sortParam = n => n.PublicationDate;
                    break;
            }
            return sortParam;
        }

        private string GetText(string text)
        {
            var r = new Regex(@"<[^>]*>");
            var matches = r.Matches(text);
            for (int i = 0; i < matches.Count; i++)
            {
                int index = text.IndexOf(matches[i].Value.ToString());
                text = text.Remove(index, matches[i].Value.Length);

            }
            return text;
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
