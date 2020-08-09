using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewsPortal.BLL.Repositories;
using NewsPortal.BLL.UnitOfWork;
using NewsPortal.DAL.Repositories;
using NewsPortal.Hibernate;
using NHibernate;
using NHibernate.Context;

namespace NewsPortal.DAL.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ISession Session;
        private ITransaction Transaction;

        public UnitOfWork(ISession session)
        {
            CurrentSessionContext.Bind(session);

            Session = session;
            Transaction = Session.BeginTransaction();
        }

        public void Commit()
        {
            Transaction.Commit();
        }

        public void Dispose()
        {
            if(!Transaction.WasCommitted && !Transaction.WasRolledBack)
            {
                Transaction.Rollback();
            }
            Transaction.Dispose();

            CurrentSessionContext.Unbind(Session.SessionFactory);
            Session.Dispose();
        }
    }
}
