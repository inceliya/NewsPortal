using NewsPortal.BLL.Entities;
using NewsPortal.BLL.Repositories;
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
        private IUnitOfWorkFactory UnitOfWorkFactory { get; }
        private ICommentRepository CommentRepository { get; }

        public CommentService(IUnitOfWorkFactory uowf, ICommentRepository commentRepository)
        {
            UnitOfWorkFactory = uowf;
            CommentRepository = commentRepository;
        }

        public Comment Get(int id)
        {
            Comment comment = null;
            using (IUnitOfWork unitOfWork = UnitOfWorkFactory.Create())
            {
                comment = CommentRepository.Get(id);
            }
            return comment;
        }

        public List<Comment> GetByNewsId(int id)
        {
            List<Comment> comments = null;
            using (IUnitOfWork unitOfWork = UnitOfWorkFactory.Create())
            {
                comments = CommentRepository.GetByNewsId(id).ToList();
            }
            return comments;
        }

        public List<Comment> GetAll()
        {
            var comments = CommentRepository.GetAll();
            return comments.ToList();
        }

        public void Add(Comment comment)
        {
            using (IUnitOfWork unitOfWork = UnitOfWorkFactory.Create())
            {
                CommentRepository.Add(comment);
                unitOfWork.Commit();
            }
        }

        public void Update(Comment comment)
        {
            using (IUnitOfWork unitOfWork = UnitOfWorkFactory.Create())
            {
                CommentRepository.Update(comment);
                unitOfWork.Commit();
            }
        }

        public void Delete(int id)
        {
            using (IUnitOfWork unitOfWork = UnitOfWorkFactory.Create())
            {
                CommentRepository.Delete(id);
                unitOfWork.Commit();
            }
        }
    }
}
