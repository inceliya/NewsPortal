using NewsPortal.BLL.Entities;
using NewsPortal.BLL.Repositories;
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
        public T Get(int id)
        {
            using (ISession session = Hibernate.HibernateHelper.OpenSession())
            {
                var queryResult = session.Get<T>(id);
                return queryResult;
            }
        }

        public IEnumerable<T> GetAll()
        {
            using (ISession session = Hibernate.HibernateHelper.OpenSession())
            {
                var queryResult = session.QueryOver<T>();
                return queryResult.List();
            }
        }

        public void Add(T item)
        {
            using (ISession session = Hibernate.HibernateHelper.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.Save(item);
                    transaction.Commit();
                }
            }
        }

        public void Update(T item)
        {
            using (ISession session = Hibernate.HibernateHelper.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {

                    session.Update(item);
                    transaction.Commit();
                }
            }
        }

        public void Delete(int id)
        {
            using (ISession session = Hibernate.HibernateHelper.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    var queryResult = session.Get<T>(id);
                    if (queryResult != null)
                    {
                        session.Delete(queryResult);
                        transaction.Commit();
                    }
                }
            }
        }
    }
}
