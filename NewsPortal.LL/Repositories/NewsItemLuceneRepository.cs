using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Lucene.Net.Store;
using NewsPortal.BLL.Entities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace NewsPortal.LL.Repositories
{
    public class NewsItemLuceneRepository : BLL.Repositories.ILuceneRepository<NewsItem>
    {
        public string Directory { get; private set; }

        public NewsItemLuceneRepository()
        {
            Directory = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["LucenePath"]);
        }

        public void Save(NewsItem item)
        {
            using (var analyzer = new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_30))
            {
                using (var writer = new IndexWriter(FSDirectory, analyzer, IndexWriter.MaxFieldLength.UNLIMITED))
                {
                    var termQuery = new TermQuery(new Term("Id", item.Id.ToString()));
                    writer.DeleteDocuments(termQuery);
                    writer.AddDocument(ToDocument(item));
                    writer.Optimize();
                }
            }
        }

        public void Delete(int id)
        {
            using (var analyzer = new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_30))
            {
                using (var writer = new IndexWriter(FSDirectory, analyzer, IndexWriter.MaxFieldLength.UNLIMITED))
                {
                    var termQuery = new TermQuery(new Term("Id", id.ToString()));
                    writer.DeleteDocuments(termQuery);
                    writer.Optimize();
                }
            }
        }

        public IEnumerable<NewsItem> Search(string filter, string sort, string search, bool reverse)
        {
            using (var analyzer = new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_30))
            {
                using (var searcher = new IndexSearcher(FSDirectory, false))
                {
                    QueryParser parser = new MultiFieldQueryParser(Lucene.Net.Util.Version.LUCENE_30, new string[] { "Title", "Description" }, analyzer);
                    Query query = parser.Parse(search.Trim());
                    int limit = 100;
                    searcher.IndexReader.Reopen();
                    var hits = searcher.Search(query,null, limit, Sort(sort, reverse)).ScoreDocs;
                    var list = new List<NewsItem>();
                    foreach (var hit in hits)
                    {
                        var foundDoc = searcher.Doc(hit.Doc);
                        list.Add(FromDocument(foundDoc));
                        searcher.IndexReader.Reopen();
                    }
                    return list;
                }
            }
        }

        public NewsItem FromDocument(Document document)
        {
            var item = new NewsItem();
            item.Id = int.Parse(document.Get("Id"));
            item.Title = document.Get("Title");
            item.Description = document.Get("Description");
            item.Image = document.Get("Image");
            item.PublicationDate = DateTime.Parse(document.Get("PublicationDate"));
            item.Visibility = document.Get("Visibility").ToLower() == "true";
            return item;
        }

        public Document ToDocument(NewsItem item)
        {
            var document = new Document();
            document.Add(new Field("Id", item.Id.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
            document.Add(new Field("Title", item.Title, Field.Store.YES, Field.Index.ANALYZED));
            document.Add(new Field("Description", item.Description, Field.Store.YES, Field.Index.ANALYZED));
            document.Add(new Field("Image", item.Image, Field.Store.YES, Field.Index.NOT_ANALYZED));
            document.Add(new Field("PublicationDate", item.PublicationDate.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
            document.Add(new Field("Visibility", item.Visibility.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
            return document;
        }

        public void DeleteAll()
        {
            using (var analyzer = new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_30))
            {
                using (var writer = new IndexWriter(FSDirectory, analyzer, IndexWriter.MaxFieldLength.UNLIMITED))
                {
                    writer.DeleteAll();
                }
            }
        }
        private Filter Filter(string filter)
        {
            switch (filter)
            {
                case "today":
                    return FieldCacheRangeFilter.NewStringRange("PublicationDate", DateTime.Now.Date.ToString(),
                        DateTime.Now.Date.AddDays(1).ToString(), true, true);
                    
                case "yesterday":
                    return FieldCacheRangeFilter.NewStringRange("PublicationDate", DateTime.Now.Date.AddDays(-1).ToString(),
                        DateTime.Now.Date.ToString(), true, true);
                case "week":
                    return FieldCacheRangeFilter.NewStringRange("PublicationDate", DateTime.Now.Date.AddDays(-7).ToString(),
                        DateTime.Now.Date.ToString(), true, true);
                case "all":
                default:
                    return FieldCacheRangeFilter.NewStringRange("PublicationDate", DateTime.MinValue.ToString(),
                        DateTime.MaxValue.ToString(), true, true);
            }
        }

        private Sort Sort(string sort, bool reverse)
        {
           SortField sortParam;
            switch (sort)
            {
                case "title":
                    sortParam = new SortField("Title", SortField.STRING, !reverse);
                    break;
                case "description":
                    sortParam = new SortField("Description", SortField.STRING, !reverse);
                    break;
                case "date":
                default:
                    sortParam = new SortField("PublicationDate", SortField.STRING, reverse); 
                    break;
            }
            return new Sort(sortParam);
        }
        private FSDirectory fSDirectory;

        private FSDirectory FSDirectory
        {
            get
            {
                if (fSDirectory == null)
                {
                    fSDirectory = FSDirectory.Open(Directory);
                }

                if (IndexWriter.IsLocked(fSDirectory))
                {
                    IndexWriter.Unlock(fSDirectory);
                }
                return fSDirectory;
            }
        }
    }
}
