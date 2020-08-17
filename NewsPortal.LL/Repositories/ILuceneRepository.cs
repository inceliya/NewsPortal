using Lucene.Net.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsPortal.LL.Repositories
{
    public interface ILuceneRepository<T> 
    {
        string Directory { get; }
        IEnumerable<T> Search(string search);
        void Save(T item);
        void Update(T item);
        void Delete(int id);
        T FromDocument(Document document);
        Document ToDocument(T item);
    }
}
