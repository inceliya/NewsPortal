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
    public class LoginRepository : Repository<Login>, ILoginRepository
    {
        public Login GetUserByLogin(string login)
        {
            using (ISession session = Hibernate.HibernateHelper.OpenSession())
            {
                var queryResult = session.QueryOver<Login>().Where(n => n.UserName == login).SingleOrDefault();
                return queryResult;
            }
        }
    }
}
