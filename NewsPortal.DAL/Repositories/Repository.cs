using NewsPortal.BLL.Entities;
using NewsPortal.BLL.Repositories;
using NewsPortal.BLL.UnitOfWork;
using NewsPortal.Hibernate;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsPortal.DAL.Repositories
{
    public class Repository<T> : IRepository<T> where T : Entity
    {
        protected ISession Session
        {
            get
            {
                return HibernateHelper.GetSession();
            }
        }

        public T Get(int id)
        {
            var queryResult = Session.Get<T>(id);
            return queryResult;
        }

        public IEnumerable<T> GetAll()
        {
            var queryResult = Session.QueryOver<T>();
            return queryResult.List();
        }

        public void Add(T item)
        {
            Session.Save(item);
        }

        public void Update(T item)
        {
            Session.Update(item);
        }

        public void Delete(int id)
        {
            var queryResult = Session.Get<T>(id);
            if (queryResult != null)
            {
                Session.Delete(queryResult);
            }
        }
    }
}
