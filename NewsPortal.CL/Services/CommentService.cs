using NewsPortal.BLL.Entities;
using NewsPortal.BLL.Repositories;
using NewsPortal.BLL.UnitOfWork;
using NewsPortal.CL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsPortal.CL.Services
{
    public class CommentService
    {
        private CacheRepository<Comment> CacheRepository { get; }
        private BLL.Services.CommentService CommentServiceBLL { get; }

        public CommentService(IUnitOfWorkFactory unitOfWorkFactory, ICommentRepository commentRepository)
        {
            CacheRepository = new CacheRepository<Comment>();
            CommentServiceBLL = new BLL.Services.CommentService(unitOfWorkFactory, commentRepository);
        }

        public Comment Get(int id)
        {
            var comment = CacheRepository.Get($"Comment-{id}");
            if (comment == null)
            {
                comment = CommentServiceBLL.Get(id);
                CacheRepository.Add(comment, $"Comment-{comment.Id}");
            }
            return comment;
        }

        public List<Comment> GetByNewsId(int id)
        {
            var comments = CacheRepository.GetSeveral($"Comments-{id}");
            if (comments == null)
            {
                comments = CommentServiceBLL.GetByNewsId(id);
                CacheRepository.Add(comments, $"Comments-{id}");
            }
            return comments;
        }

        public List<Comment> GetAll()
        {
            var comments = CommentServiceBLL.GetAll();
            return comments;
        }

        public void Add(Comment comment)
        {
            CommentServiceBLL.Add(comment);
            CacheRepository.Add(comment, $"Comment-{comment.Id}");
            CacheRepository.Delete($"Comments-{comment.NewsId}");
        }

        public void Update(Comment comment)
        {
            CommentServiceBLL.Update(comment);
            CacheRepository.Update(comment, $"Comment-{comment.Id}");
            CacheRepository.Delete($"Comments-{comment.NewsId}");
        }

        public void Delete(int id)
        {
            var comment = CacheRepository.Get($"Comment-{id}");

            if (comment == null)
            {
                comment = CommentServiceBLL.Get(id);
            }

            CacheRepository.Delete($"Comments-{comment.NewsId}");

            CommentServiceBLL.Delete(id);
            CacheRepository.Delete($"Comment-{id}");
        }
    }
}
