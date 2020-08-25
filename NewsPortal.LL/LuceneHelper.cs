using NewsPortal.BLL.Entities;
using NewsPortal.LL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsPortal.LL
{
    public class LuceneHelper : BLL.Helpers.ILuceneHelper
    {
        public BLL.Repositories.ILuceneRepository<T> GetRepository<T>()
        {
            return new NewsItemLuceneRepository() as BLL.Repositories.ILuceneRepository<T>;
        }
    }
}
