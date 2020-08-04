using NewsPortal.BLL.Entities;
using NewsPortal.BLL.IRepositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace NewsPortal.DAL.Xml.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        protected List<Comment> Comments;
        protected XDocument ItemsData;

        public CommentRepository()
        {
            Comments = new List<Comment>();
            ItemsData = XDocument.Load(Path.GetFullPath(@"C:\Users\nikos\source\repos\NewsPortal\NewsPortal.DAL.Xml\Data\CommentData.xml"));
            if (!string.IsNullOrEmpty(ItemsData.Root.Value))
            {
                var comments = from t in ItemsData.Descendants("item")
                               select new Comment()
                               {
                                   Id = (int)t.Element("id"),
                                   NewsId = (int)t.Element("news_id"),
                                   Author = t.Element("author").Value,
                                   Text = t.Element("text").Value,
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
            if (!string.IsNullOrEmpty(ItemsData.Root.Value))
            {
                comment.Id = (int)(from S in ItemsData.Descendants("item") orderby (int)S.Element("id") descending select (int)S.Element("id")).FirstOrDefault() + 1;
            }
            else
            {
                comment.Id = 0;
            }
            ItemsData.Root.Add(new XElement("item",
                new XElement("id", comment.Id),
                new XElement("news_id", comment.NewsId),
                new XElement("author", comment.Author),
                new XElement("text", comment.Text),
                new XElement("creation_date", comment.CreationDate)));

            ItemsData.Save(Path.GetFullPath(@"C:\Users\nikos\source\repos\NewsPortal\NewsPortal.DAL.Xml\Data\CommentData.xml"));
        }

        public void Update(Comment comment)
        {
            try
            {
                XElement node = ItemsData.Root.Elements("item").Where(i => (int)i.Element("id") == comment.Id).FirstOrDefault();

                node.SetElementValue("news_id", comment.NewsId);
                node.SetElementValue("author", comment.Author);
                node.SetElementValue("text", comment.Text);
                node.SetElementValue("creation_date", comment.CreationDate);
                ItemsData.Save(Path.GetFullPath(@"C:\Users\nikos\source\repos\NewsPortal\NewsPortal.DAL.Xml\Data\CommentData.xml"));
            }
            catch (Exception)
            {

                throw new NotImplementedException();
            }
        }

        public void Delete(int id)
        {
            try
            {
                ItemsData.Root.Elements("item").Where(i => (int)i.Element("id") == id).Remove();

                ItemsData.Save(Path.GetFullPath(@"C:\Users\nikos\source\repos\NewsPortal\NewsPortal.DAL.Xml\Data\CommentData.xml"));

            }
            catch (Exception)
            {

                throw new NotImplementedException();
            }
        }
    }
}
