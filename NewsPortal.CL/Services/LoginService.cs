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
    public class LoginService
    {
        private CacheRepository<Login> CacheRepository { get; }
        private BLL.Services.LoginService LoginServiceBLL { get; }

        public LoginService(IUnitOfWorkFactory unitOfWorkFactory, ILoginRepository loginRepository)
        {
            CacheRepository = new CacheRepository<Login>();
            LoginServiceBLL = new BLL.Services.LoginService(unitOfWorkFactory, loginRepository);
        }

        public Login Get(int id)
        {
            var user = CacheRepository.Get($"User-{id}");
            if (user == null)
            {
                user = LoginServiceBLL.Get(id);
                CacheRepository.Add(user, $"User-{id}");
            }
            return user;
        }

        public Login GetUserByLogin(string login)
        {
            return LoginServiceBLL.GetUserByLogin(login);
        }

        public List<Login> GetAll()
        {
            var users = CacheRepository.GetSeveral("AllUsers");
            if (users == null)
            {
                users = LoginServiceBLL.GetAll();
                CacheRepository.Add(users, "AllUsers");
            }
            return users;
        }

        public void Add(Login user)
        {
            LoginServiceBLL.Add(user);
            CacheRepository.Add(user, $"User-{user.Id}");
            CacheRepository.Delete("AllUsers");
        }

        public void Update(Login user)
        {
            LoginServiceBLL.Update(user);
            CacheRepository.Update(user, $"User-{user.Id}");
            CacheRepository.Delete("AllUsers");
        }

        public void Delete(int id)
        {
            LoginServiceBLL.Delete(id);
            CacheRepository.Delete($"User-{id}");
            CacheRepository.Delete("AllUsers");
        }
    }
}
