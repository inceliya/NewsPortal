using NewsPortal.BLL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsPortal.BLL.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        //INewsRepository News { get; }
        //ICommentRepository Comment { get; }
        void Commit();
    }
}
