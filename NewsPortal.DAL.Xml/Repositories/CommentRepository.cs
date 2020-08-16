using NewsPortal.BLL.Entities;
using NewsPortal.BLL.Repositories;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Linq;

namespace NewsPortal.DAL.Xml.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private string FilePath = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["TempXmlFilePath"]);

        protected List<Comment> Comments;
        protected XDocument ItemsData;

        public CommentRepository()
        {
            Comments = new List<Comment>();
            ItemsData = XDocument.Load(FilePath);

            if (!string.IsNullOrEmpty(ItemsData.Root.Element("Comment").Value))
            {
                var comments = from t in ItemsData.Root.Element("Comment").Descendants("item")
                               select new Comment()
                               {
                                   Id = (int)t.Element("id"),
                                   NewsId = (int)t.Element("news_id"),
                                   Author = (string)t.Element("author"),
                                   Text = (string)t.Element("text"),
                                   CreationDate = (DateTime)t.Element("creation_date")
                               };

                Comments.AddRange(comments.ToList());
            }
        }

        public Comment Get(int id)
        {
            return Comments.Find(n => n.Id == id);
        }

        public IEnumerable<Comment> GetByNewsId(int id)
        {
            return Comments.Where(n => n.NewsId == id);
        }

        public IEnumerable<Comment> GetAll()
        {
            return Comments;
        }

        public void Add(Comment comment)
        {
            if (!string.IsNullOrEmpty(ItemsData.Root.Element("Comment").Value))
            {
                comment.Id = (int)(ItemsData.Root.Element("Comment").Element("max_id")) + 1;
                ItemsData.Root.Element("Comment").SetElementValue("max_id", comment.Id);
            }
            else
            {
                comment.Id = 0;
            }
            ItemsData.Root.Element("Comment").Add(new XElement("item",
                new XElement("id", comment.Id),
                new XElement("news_id", comment.NewsId),
                new XElement("author", comment.Author),
                new XElement("text", comment.Text),
                new XElement("creation_date", comment.CreationDate)));

            ItemsData.Save(FilePath);
        }

        public void Update(Comment comment)
        {
            XElement node = ItemsData.Root.Element("Comment").Elements("item").Where(i => (int)i.Element("id") == comment.Id).Single();

            node.SetElementValue("news_id", comment.NewsId);
            node.SetElementValue("author", comment.Author);
            node.SetElementValue("text", comment.Text);
            node.SetElementValue("creation_date", comment.CreationDate);
            ItemsData.Save(FilePath);
        }

        public void Delete(int id)
        {
            ItemsData.Root.Element("Comment").Elements("item").Where(i => (int)i.Element("id") == id).Remove();

            ItemsData.Save(FilePath);
        }
    }
}
