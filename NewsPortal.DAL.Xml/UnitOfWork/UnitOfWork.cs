using NewsPortal.BLL.Repositories;
using NewsPortal.BLL.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml;
using System.Xml.Linq;

namespace NewsPortal.DAL.Xml.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private XDocument OldItemsData { get; set; }
        private XDocument NewItemsData { get; set; }
        private bool IsCommited { get; set; }
        private string PathToMain { get; }
        private string PathToTemp { get; }

        public UnitOfWork()
        {
            PathToMain = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["XmlFilePath"]);
            PathToTemp = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["TempXmlFilePath"]);

            OldItemsData = XDocument.Load(PathToMain);
            NewItemsData = new XDocument(OldItemsData);

            NewItemsData.Save(PathToTemp);

            IsCommited = false;
        }

        public void Commit()
        {
            NewItemsData = XDocument.Load(PathToTemp);
            NewItemsData.Save(PathToMain);
            IsCommited = true;
        }

        public void Dispose()
        {
            if (!IsCommited)
            {
                OldItemsData.Save(PathToMain);
            }
        }
    }
}
