using NewsPortal.BLL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsPortal.BLL.Repositories
{
    public interface INewsRepository : IRepository<NewsItem>
    {
        IEnumerable<NewsItem> GetAllByFilter(System.Linq.Expressions.Expression<Func<NewsItem, bool>> filter);
    }
}
