using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewsPortal.BLL.IRepositories;
using NewsPortal.BLL.UnitOfWork;
using NewsPortal.DAL.Repositories;
using NHibernate;

namespace NewsPortal.DAL.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        public INewsRepository News { get; }
        public ICommentRepository Comment { get; }

        public ISession Session { get; }

        public UnitOfWork(/*ISession session,*/ INewsRepository newsRepository, ICommentRepository commentRepository)
        {
            //Session = session;

            News = newsRepository;
            Comment = commentRepository;
        }

        public void Commit()
        {
            //using (ITransaction transaction = Session.BeginTransaction())
            //{
            //    transaction.Commit();
            //}
        }

        public void Dispose()
        {
            //Session.Close();
        }
    }
}
