using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewsPortal.BLL.Repositories;
using NewsPortal.BLL.UnitOfWork;
using NewsPortal.DAL.Repositories;
using NHibernate;

namespace NewsPortal.DAL.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        public INewsRepository News { get; }
        public ICommentRepository Comment { get; }

        public UnitOfWork( INewsRepository newsRepository, ICommentRepository commentRepository)
        {
            News = newsRepository;
            Comment = commentRepository;
        }

        public void Dispose()
        {
            
        }
    }
}
