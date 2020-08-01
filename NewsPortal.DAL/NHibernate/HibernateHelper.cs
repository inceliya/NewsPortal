using NewsPortal.BLL.Entities;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewsPortal.Hibernate
{
    public class HibernateHelper
    {
        private static ISessionFactory _sessionFactory;
        private static ISessionFactory SessionFactory
        {
            get
            {
                if (_sessionFactory == null)
                {
                    var cfg = new Configuration();
                    cfg.Configure();
                    cfg.AddFile(HttpContext.Current.Server.MapPath(@"~\HibernateMapping\NewsItem.hbm.xml"));
                    cfg.AddFile(HttpContext.Current.Server.MapPath(@"~\HibernateMapping\Comment.hbm.xml"));
                    new SchemaUpdate(cfg).Execute(true, true);
                    _sessionFactory = cfg.BuildSessionFactory();
                }
                return _sessionFactory;
            }
        }

        public static ISession OpenSession()
        {
            return SessionFactory.OpenSession();
        }
    }
}