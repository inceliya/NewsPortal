﻿using AutoMapper;
using NewsPortal.BLL.Entities;
using NewsPortal.BLL.Helpers;
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
        private ILuceneHelper LuceneHelper { get; }

        public NewsService(IUnitOfWorkFactory uowf, INewsRepository newsRepository, ILuceneHelper luceneHelper)
        {
            UnitOfWorkFactory = uowf;
            NewsRepository = newsRepository;
            LuceneHelper = luceneHelper;
            Refresh(GetAll());
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
                if (string.IsNullOrEmpty(search))
                    news = NewsRepository.GetAllByFilter(Filter(filter), Sort(sort), search, reverse).ToList();
                else
                    news = LuceneHelper.GetRepository<NewsItem>().Search(filter, sort, search, reverse).ToList();
            }

            return news;
        }

        private Expression<Func<NewsItem, bool>> Filter(string filter)
        {
            switch (filter)
            {
                case "today":
                    return n => n.PublicationDate.Date == DateTime.Today;
                case "yesterday":
                    return n => n.PublicationDate.Date == DateTime.Today.AddDays(-1);
                case "week":
                    return n => n.PublicationDate.Date >= DateTime.Today.AddDays(-7);
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

        public void Add(NewsItem newsItem)
        {
            using (IUnitOfWork unitOfWork = UnitOfWorkFactory.Create())
            {
                NewsRepository.Add(newsItem);
                unitOfWork.Commit();
                LuceneHelper.GetRepository<NewsItem>().Save(newsItem);
            }
        }

        public void Update(NewsItem newsItem)
        {
            using (IUnitOfWork unitOfWork = UnitOfWorkFactory.Create())
            {
                NewsRepository.Update(newsItem);
                unitOfWork.Commit();
                LuceneHelper.GetRepository<NewsItem>().Save(newsItem);
            }
        }

        public void Delete(int id)
        {
            using (IUnitOfWork unitOfWork = UnitOfWorkFactory.Create())
            {
                NewsRepository.Delete(id);
                unitOfWork.Commit();
                LuceneHelper.GetRepository<NewsItem>().Delete(id);
            }
        }
        public void Refresh(IEnumerable<NewsItem> list)
        {
            var lucene = LuceneHelper.GetRepository<NewsItem>();
            lucene.DeleteAll();
            foreach (var item in list)
                lucene.Save(item);
        }
    }
}
