﻿using NewsPortal.BLL.Entities;
using NewsPortal.BLL.Helpers;
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

        public NewsService(IUnitOfWorkFactory unitOfWorkFactory, INewsRepository newsRepository, ILuceneHelper luceneHelper)
        {
            CacheRepository = new CacheRepository<NewsItem>();
            NewsServiceBLL = new BLL.Services.NewsService(unitOfWorkFactory, newsRepository, luceneHelper);
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
            var news = CacheRepository.GetSeveral($"AllNews-{filter}-{sort}-{search}-{reverse}");
            if (news == null)
            {
                CacheRepository.DeleteByPartOfTheKey("AllNews");

                news = NewsServiceBLL.GetAll(filter, sort, search, reverse);
                CacheRepository.Add(news, $"AllNews-{filter}-{sort}-{search}-{reverse}");
            }
            return news;
        }

        public void Add(NewsItem newsItem)
        {
            NewsServiceBLL.Add(newsItem);
            CacheRepository.Add(newsItem, $"News-{newsItem.Id}");

            CacheRepository.DeleteByPartOfTheKey("AllNews");
        }

        public void Update(NewsItem newsItem)
        {
            NewsServiceBLL.Update(newsItem);
            CacheRepository.Update(newsItem, $"News-{newsItem.Id}");

            CacheRepository.DeleteByPartOfTheKey("AllNews");
        }

        public void Delete(int id)
        {
            NewsServiceBLL.Delete(id);
            CacheRepository.Delete($"News-{id}");

            CacheRepository.DeleteByPartOfTheKey("AllNews");
        }
    }
}
