using NewsPortal.BLL.Entities;
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
            LuceneHelper.GetRepository<NewsItem>().Save(item);
            base.Add(item);
        }
        public override void Update(NewsItem item)
        {
            LuceneHelper.GetRepository<NewsItem>().Update(item);
            base.Update(item);
        }
        public override void Delete(int id)
        {
            LuceneHelper.GetRepository<NewsItem>().Delete(id);
            base.Delete(id);
        }
    }
}
