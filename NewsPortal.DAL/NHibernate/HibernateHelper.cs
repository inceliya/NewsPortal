using NewsPortal.BLL.Entities;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Context;
using NHibernate.Tool.hbm2ddl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewsPortal.Hibernate
{
    public class HibernateHelper
    {
        private static readonly object LockObject = new object();
        private static ISessionFactory _sessionFactory;

        private static ISessionFactory SessionFactory
        {
            get
            {
                if (_sessionFactory == null)
                {
                    lock (LockObject)
                    {
                        var cfg = new Configuration();
                        cfg.Configure();
                        cfg.CurrentSessionContext<WebSessionContext>();
                        cfg.AddFile(HttpContext.Current.Server.MapPath(@"~\HibernateMapping\NewsItem.hbm.xml"));
                        cfg.AddFile(HttpContext.Current.Server.MapPath(@"~\HibernateMapping\Comment.hbm.xml"));
                        cfg.AddFile(HttpContext.Current.Server.MapPath(@"~\HibernateMapping\Login.hbm.xml"));
                        new SchemaUpdate(cfg).Execute(true, true);
                        _sessionFactory = cfg.BuildSessionFactory();
                    }
                }
                return _sessionFactory;
            }
        }

        public static ISession GetSession()
        {
            return SessionFactory.GetCurrentSession();
        }

        public static ISession OpenSession()
        {
            return SessionFactory.OpenSession();
        }
    }
}