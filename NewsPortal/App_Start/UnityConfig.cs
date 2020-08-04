using NewsPortal.BLL.IRepositories;
using NewsPortal.BLL.IServices;
using NewsPortal.BLL.Services;
using NewsPortal.BLL.UnitOfWork;
using NewsPortal.DAL.Repositories;
using NewsPortal.DAL.UnitOfWork;
using System.Web.Mvc;
using Unity;
using Unity.Mvc5;

namespace NewsPortal
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();
            container.RegisterType<INewsRepository, NewsRepository>();
            container.RegisterType<ICommentRepository, CommentRepository>();
            container.RegisterType<IUnitOfWork, UnitOfWork>();
            container.RegisterType<INewsService, NewsService>();
            container.RegisterType<ICommentService, CommentService>();
            
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}