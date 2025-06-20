using BussinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public interface INewsArticleRepository
    {
        List<NewsArticle> GetAll();
        NewsArticle? GetById(int id);
        bool Add(NewsArticle newsArticle);
        bool Update(NewsArticle newsArticle);
        bool Delete(int id);

        List<NewsArticle> AdvancedSearch(string? headline = null, int? categoryId = null, int? authorId = null,
            bool? status = null, DateTime? fromDate = null, DateTime? toDate = null);

        bool HeadlineExists(string headline, int? excludeId = null);

        //int GetCountByStatus(bool status);
        //int GetCountByCategory(int categoryId);
        //int GetCountByAuthor(int accountId);
        //Dictionary<string, int> GetStatistics();
    }
}