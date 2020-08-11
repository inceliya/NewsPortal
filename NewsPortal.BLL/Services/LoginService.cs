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
    public class LoginService
    {
        private IUnitOfWorkFactory UnitOfWorkFactory { get; }
        private ILoginRepository LoginRepository { get; }

        public LoginService(IUnitOfWorkFactory uowf, ILoginRepository loginRepository)
        {
            UnitOfWorkFactory = uowf;
            LoginRepository = loginRepository;
        }

        public Login Get(int id)
        {
            var user = LoginRepository.Get(id);
            return user;
        }

        public Login GetUserByLogin(string login)
        {
            var user = LoginRepository.GetUserByLogin(login);
            return user;
        }

        public List<Login> GetAll()
        {
            var users = LoginRepository.GetAll();
            return users.ToList();
        }

        public void Add(Login login)
        {
            using (IUnitOfWork unitOfWork = UnitOfWorkFactory.Create())
            {
                LoginRepository.Add(login);
                unitOfWork.Commit();
            }
        }

        public void Update(Login login)
        {
            using (IUnitOfWork unitOfWork = UnitOfWorkFactory.Create())
            {
                LoginRepository.Update(login);
                unitOfWork.Commit();
            }
        }

        public void Delete(int id)
        {
            using (IUnitOfWork unitOfWork = UnitOfWorkFactory.Create())
            {
                LoginRepository.Delete(id);
                unitOfWork.Commit();
            }
        }
    }
}
