using NewsPortal.BLL.Entities;
using NewsPortal.BLL.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsPortal.BLL.Services
{
    public class CommentService
    {
        public Comment Get(int id)
        {
            var comment = UnitOfWorkHelper.UnitOfWork.Comment.Get(id);
            return comment;
        }

        public List<Comment> GetByNewsId(int id)
        {
            var comments = UnitOfWorkHelper.UnitOfWork.Comment.GetByNewsId(id);
            return comments.ToList();
        }

        public List<Comment> GetAll()
        {
            var comments = UnitOfWorkHelper.UnitOfWork.Comment.GetAll();
            return comments.ToList();
        }

        public void Add(Comment comment)
        {
            UnitOfWorkHelper.UnitOfWork.Comment.Add(comment);
        }

        public void Update(Comment comment)
        {
            UnitOfWorkHelper.UnitOfWork.Comment.Update(comment);
        }

        public void Delete(int id)
        {
            UnitOfWorkHelper.UnitOfWork.Comment.Delete(id);
        }
    }
}
