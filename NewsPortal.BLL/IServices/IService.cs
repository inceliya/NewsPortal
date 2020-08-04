using NewsPortal.BLL.Entities;
using NewsPortal.BLL.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsPortal.BLL.IServices
{
    public interface IService<T> where T : Entity
    {
        T Get(int id);
        void Add(T item);
        void Update(T item);
        void Delete(int id);
    }
}
