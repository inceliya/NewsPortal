using NewsPortal.BLL.Entities;
using NewsPortal.BLL.Repositories;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Linq;

namespace NewsPortal.DAL.Xml.Repositories
{
    public class LoginRepository : ILoginRepository
    {
        private string FilePath = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["LoginXmlFilePath"]);
        protected List<Login> Users;
        protected XDocument ItemsData;

        public LoginRepository()
        {
            Users = new List<Login>();
            ItemsData = XDocument.Load(FilePath);
            if (!string.IsNullOrEmpty(ItemsData.Root.Value))
            {
                var users = from t in ItemsData.Descendants("item")
                           select new Login()
                           {
                               Id = (int)t.Element("id"),
                               UserName = (string)t.Element("username"),
                               Password = (string)t.Element("password"),
                           };
                Users.AddRange(users.ToList());
            }
        }

        public Login Get(int id)
        {
            return Users.Find(n => n.Id == id);
        }

        public IEnumerable<Login> GetAll()
        {
            return Users;
        }

        public Login GetUserByLogin(string login)
        {
            return Users.Find(n => n.UserName == login);
        }


        public void Add(Login login)
        {
            if (!string.IsNullOrEmpty(ItemsData.Root.Value))
            {
                login.Id = (int)(ItemsData.Root.Element("max_id")) + 1;
                ItemsData.Root.SetElementValue("max_id", login.Id);
            }
            else
            {
                login.Id = 0;
            }

            ItemsData.Root.Add(new XElement("item",
                new XElement("id", login.Id),
                new XElement("username", login.UserName),
                new XElement("password", login.Password)));

            ItemsData.Save(FilePath);
        }

        public void Update(Login login)
        {
            XElement node = ItemsData.Root.Elements("item").Where(i => (int)i.Element("id") == login.Id).Single();

            node.SetElementValue("username", login.UserName);
            node.SetElementValue("password", login.Password);
            ItemsData.Save(FilePath);
        }

        public void Delete(int id)
        {
            ItemsData.Root.Elements("item").Where(i => (int)i.Element("id") == id).Remove();

            ItemsData.Save(FilePath);
        }
    }
}
