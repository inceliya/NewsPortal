using NewsPortal.BLL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NewsPortal.BLL.Repositories
{
    public interface INewsRepository : IRepository<NewsItem>
    {
        IEnumerable<NewsItem> GetAllByFilter(Expression<Func<NewsItem, bool>> filter, Expression<Func<NewsItem, object>> sort, string search, bool reverse);
    }
}
