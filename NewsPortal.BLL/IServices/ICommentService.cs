using NewsPortal.BLL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsPortal.BLL.IServices
{
    public interface ICommentService : IService<Comment>
    {
        List<Comment> GetByNewsId(int id);
        List<Comment> GetAll();
    }
}
