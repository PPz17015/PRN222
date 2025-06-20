using BussinessObject;
using DataAccessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class NewsArticleRepository : INewsArticleRepository
    {
        public List<NewsArticle> GetAll()
            => new NewsArticleDAO().GetAll();

        public NewsArticle? GetById(int id)
            => new NewsArticleDAO().GetById(id);

        public bool Add(NewsArticle newsArticle)
            => new NewsArticleDAO().Add(newsArticle);

        public bool Update(NewsArticle newsArticle)
            => new NewsArticleDAO().Update(newsArticle);

        public bool Delete(int id)
            => new NewsArticleDAO().Delete(id);

        public List<NewsArticle> AdvancedSearch(string? headline = null, int? categoryId = null, int? authorId = null, 
            bool? status = null, DateTime? fromDate = null, DateTime? toDate = null)
            => new NewsArticleDAO().AdvancedSearch(headline, categoryId, authorId, status, fromDate, toDate);

        public bool HeadlineExists(string headline, int? excludeId = null)
            => new NewsArticleDAO().HeadlineExists(headline, excludeId);

        //public int GetCountByStatus(bool status)
        //    => new NewsArticleDAO().GetCountByStatus(status);

        //public int GetCountByCategory(int categoryId)
        //    => new NewsArticleDAO().GetCountByCategory(categoryId);

        //public int GetCountByAuthor(int accountId)
        //    => new NewsArticleDAO().GetCountByAuthor(accountId);

        //public Dictionary<string, int> GetStatistics()
        //    => new NewsArticleDAO().GetStatistics();
    }
} 