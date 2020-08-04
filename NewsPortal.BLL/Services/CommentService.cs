using NewsPortal.BLL.Entities;
using NewsPortal.BLL.IServices;
using NewsPortal.BLL.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsPortal.BLL.Services
{
    public class CommentService : ICommentService
    {
        private IUnitOfWork UnitOfWork { get; }

        public CommentService(IUnitOfWork uow)
        {
            UnitOfWork = uow;
        }

        public Comment Get(int id)
        {
            var comment = UnitOfWork.Comment.Get(id);
            return comment;
        }

        public List<Comment> GetByNewsId(int id)
        {
            var comments = UnitOfWork.Comment.GetByNewsId(id);
            return comments.ToList();
        }

        public List<Comment> GetAll()
        {
            var comments = UnitOfWork.Comment.GetAll();
            return comments.ToList();
        }

        public void Add(Comment comment)
        {
            UnitOfWork.Comment.Add(comment);
        }

        public void Update(Comment comment)
        {
            UnitOfWork.Comment.Update(comment);
        }

        public void Delete(int id)
        {
            UnitOfWork.Comment.Delete(id);
        }
    }
}
