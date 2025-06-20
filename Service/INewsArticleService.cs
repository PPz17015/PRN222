using BussinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public interface INewsArticleService
    {
        List<NewsArticle> GetAll();
        NewsArticle? GetById(int id);
        bool Add(NewsArticle newsArticle);
        bool Update(NewsArticle newsArticle);
        bool Delete(int id);
        List<NewsArticle> AdvancedSearch(string? headline = null, int? categoryId = null, int? authorId = null, 
            bool? status = null, DateTime? fromDate = null, DateTime? toDate = null);
        bool HeadlineExists(string headline, int? excludeId = null);
        bool CanDeleteArticle(int articleId);
        bool PublishArticle(int articleId, int updatedBy);
        bool UnpublishArticle(int articleId, int updatedBy);
        List<NewsArticle> GetPublishedArticles();
        //List<NewsArticle> GetDraftArticles();
        //List<NewsArticle> GetArticlesByCurrentUser(int accountId);
    }
} 