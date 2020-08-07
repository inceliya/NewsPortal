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
    public class CommentRepository : Repository<Comment>, ICommentRepository
    {
        public IEnumerable<Comment> GetByNewsId(int id)
        {
            using (ISession session = Hibernate.HibernateHelper.OpenSession())
            {
                var queryResult = session.QueryOver<Comment>().Where(n => n.NewsId == id);
                return queryResult.List();
            }
        }
    }
}
