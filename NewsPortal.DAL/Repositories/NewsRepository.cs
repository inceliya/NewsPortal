using NewsPortal.BLL.Entities;
using NewsPortal.BLL.Repositories;
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
        public IEnumerable<NewsItem> GetAllByFilter(Expression<Func<NewsItem, bool>> filter)
        {
            using (ISession session = Hibernate.HibernateHelper.OpenSession())
            {
                var queryResult = session.QueryOver<NewsItem>().Where(filter);
                return queryResult.List();
            }
        }
    }
}
