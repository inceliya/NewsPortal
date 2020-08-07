using NewsPortal.BLL.Repositories;
using NewsPortal.BLL.IServices;
using NewsPortal.BLL.Services;
using NewsPortal.BLL.UnitOfWork;
using System.Web.Mvc;
using Unity;
using Unity.Mvc5;
using System.Configuration;

namespace NewsPortal
{
    public static class UnityConfig
    {
        private static readonly string Connection = ConfigurationManager.AppSettings["SaveData"];

        public static void RegisterComponents()
        {
            var container = new UnityContainer();

            switch (Connection)
            {
                case "xml":
                    container.RegisterType<INewsRepository, DAL.Xml.Repositories.NewsRepository>();
                    container.RegisterType<ICommentRepository, DAL.Xml.Repositories.CommentRepository>();
                    container.RegisterType<IUnitOfWork, DAL.Xml.UnitOfWork.UnitOfWork>();
                    break;
                case "db":
                default:
                    container.RegisterType<INewsRepository, DAL.Repositories.NewsRepository>();
                    container.RegisterType<ICommentRepository, DAL.Repositories.CommentRepository>();
                    container.RegisterType<IUnitOfWork, DAL.UnitOfWork.UnitOfWork>();
                    break;
            }

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}