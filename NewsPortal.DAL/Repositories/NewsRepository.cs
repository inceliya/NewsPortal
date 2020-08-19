﻿using NewsPortal.BLL.Entities;
using NewsPortal.BLL.Repositories;
using NewsPortal.LL;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NewsPortal.DAL.Repositories
{
    public class NewsRepository : Repository<NewsItem>, INewsRepository
    {
        public IEnumerable<NewsItem> GetAllByFilter(Expression<Func<NewsItem, bool>> filter, Expression<Func<NewsItem, object>> sort, string search, bool reverse)
        {
            if (!string.IsNullOrEmpty(search.Trim()))
                if (reverse)
                    return LuceneHelper.GetRepository<NewsItem>().Search(search).AsQueryable().Where(filter).OrderByDescending(sort);
                else
                    return LuceneHelper.GetRepository<NewsItem>().Search(search).AsQueryable().Where(filter).OrderBy(sort);

            if (reverse)
                return Session.QueryOver<NewsItem>().Where(filter).OrderBy(sort).Desc.List();
            else
                return Session.QueryOver<NewsItem>().Where(filter).OrderBy(sort).Asc.List();
        }
        public override void Add(NewsItem item)
        {
            base.Add(item);
            LuceneHelper.GetRepository<NewsItem>().Save(item);
        }
        public override void Update(NewsItem item)
        {
            base.Update(item);
            LuceneHelper.GetRepository<NewsItem>().Save(item);
        }
        public override void Delete(int id)
        {
            base.Delete(id);
            LuceneHelper.GetRepository<NewsItem>().Delete(id);
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
