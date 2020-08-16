using NewsPortal.BLL.Entities;
using NewsPortal.BLL.Repositories;
using NewsPortal.BLL.UnitOfWork;
using NewsPortal.CL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsPortal.CL.Services
{
    public class NewsService
    {
        private CacheRepository<NewsItem> CacheRepository { get; }
        private BLL.Services.NewsService NewsServiceBLL { get; }

        public NewsService(IUnitOfWorkFactory unitOfWorkFactory, INewsRepository newsRepository)
        {
            CacheRepository = new CacheRepository<NewsItem>();
            NewsServiceBLL = new BLL.Services.NewsService(unitOfWorkFactory, newsRepository);
        }

        public NewsItem Get(int id)
        {
            var newsItem = CacheRepository.Get($"News-{id}");
            if (newsItem == null)
            {
                newsItem = NewsServiceBLL.Get(id);
                CacheRepository.Add(newsItem, $"News-{newsItem.Id}");
            }
            return newsItem;
        }

        public List<NewsItem> GetAll(string filter, string sort, string search, bool reverse)
        {
            var news = NewsServiceBLL.GetAll(filter, sort, search, reverse);
            return news;
        }

        public void Add(NewsItem newsItem)
        {
            NewsServiceBLL.Add(newsItem);
            CacheRepository.Add(newsItem, $"News-{newsItem.Id}");
        }

        public void Update(NewsItem newsItem)
        {
            NewsServiceBLL.Update(newsItem);
            CacheRepository.Update(newsItem, $"News-{newsItem.Id}");
        }

        public void Delete(int id)
        {
            NewsServiceBLL.Delete(id);
            CacheRepository.Delete($"News-{id}");
        }
    }
}
