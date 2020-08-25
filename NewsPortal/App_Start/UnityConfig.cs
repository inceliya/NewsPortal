using NewsPortal.BLL.Repositories;
using NewsPortal.BLL.UnitOfWork;
using System.Web.Mvc;
using Unity;
using Unity.Mvc5;
using System.Configuration;
using NewsPortal.BLL.Helpers;

namespace NewsPortal
{
    public static class UnityConfig
    {
        private static readonly string Connection = ConfigurationManager.AppSettings["SaveData"];

        public static void RegisterComponents()
        {
            var container = new UnityContainer();

            container.RegisterType<ILuceneHelper, LL.LuceneHelper>();
            switch (Connection)
            {
                case "xml":
                    container.RegisterType<INewsRepository, DAL.Xml.Repositories.NewsRepository>();
                    container.RegisterType<ICommentRepository, DAL.Xml.Repositories.CommentRepository>();
                    container.RegisterType<ILoginRepository, DAL.Xml.Repositories.LoginRepository>();
                    container.RegisterType<IUnitOfWorkFactory, DAL.Xml.UnitOfWork.UnitOfWorkFactory>();
                    break;
                case "db":
                default:
                    container.RegisterType<INewsRepository, DAL.Repositories.NewsRepository>();
                    container.RegisterType<ICommentRepository, DAL.Repositories.CommentRepository>();
                    container.RegisterType<ILoginRepository, DAL.Repositories.LoginRepository>();
                    container.RegisterType<IUnitOfWorkFactory, DAL.UnitOfWork.UnitOfWorkFactory>();
                    break;
            }

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}