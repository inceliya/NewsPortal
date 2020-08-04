using NewsPortal.BLL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NewsPortal.BLL.IServices
{
    public interface INewsService : IService<NewsItem>
    {
        List<NewsItem> GetAll(string filter, string sort, string search, bool reverse);
    }
}
