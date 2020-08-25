using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsPortal.BLL.Helpers
{
    public interface ILuceneHelper
    {
       Repositories.ILuceneRepository<T> GetRepository<T>();
    }
}
