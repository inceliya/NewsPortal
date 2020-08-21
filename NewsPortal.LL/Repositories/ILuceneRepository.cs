﻿using Lucene.Net.Documents;
using NewsPortal.BLL.Entities;
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
        void Delete(int id);
        void DeleteAll();
        T FromDocument(Document document);
        Document ToDocument(T item);
    }
}
