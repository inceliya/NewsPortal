﻿using NewsPortal.BLL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsPortal.BLL.IRepositories
{
    public interface ICommentRepository : IRepository<Comment>
    {
        IEnumerable<Comment> GetByNewsId(int id);
    }
}
