using Lucene.Net.Documents;
using NewsPortal.BLL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NewsPortal.BLL.Repositories
{
    public interface ILuceneRepository<T>
    {
        string Directory { get; }
        IEnumerable<T> Search(Expression<Func<NewsItem, bool>> filter, Expression<Func<NewsItem, object>> sort, string search, bool reverse);
        void Save(T item);
        void Delete(int id);
        void DeleteAll();
        T FromDocument(Document document);
        Document ToDocument(T item);
    }
}
