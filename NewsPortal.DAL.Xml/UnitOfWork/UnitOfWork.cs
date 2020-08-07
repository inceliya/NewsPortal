using NewsPortal.BLL.Repositories;
using NewsPortal.BLL.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsPortal.DAL.Xml.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        public INewsRepository News { get; }
        public ICommentRepository Comment { get; }

        public UnitOfWork(INewsRepository newsRepository, ICommentRepository commentRepository)
        {
            News = newsRepository;
            Comment = commentRepository;
        }

        public void Dispose()
        {

        }
    }
}
