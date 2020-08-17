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
        public IEnumerable<NewsItem> GetAllByFilter(Expression<Func<NewsItem, bool>> filter, string search)
        {
            if (!string.IsNullOrEmpty(search.Trim())) return LuceneHelper.GetRepository<NewsItem>().Search(search).AsQueryable().Where(filter);
            var queryResult = Session.QueryOver<NewsItem>().Where(filter);
            return queryResult.List();
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
