using BussinessObject;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class NewsArticleService : INewsArticleService
    {
        private readonly INewsArticleRepository _newsArticleRepository;
        private readonly ICategoryRepository _categoryRepository;

        public NewsArticleService(INewsArticleRepository newsArticleRepository, ICategoryRepository categoryRepository)
        {
            _newsArticleRepository = newsArticleRepository;
            _categoryRepository = categoryRepository;
        }

        // Basic CRUD operations
        public List<NewsArticle> GetAll()
        {
            return _newsArticleRepository.GetAll();
        }

        public NewsArticle? GetById(int id)
        {
            return _newsArticleRepository.GetById(id);
        }

        public bool Add(NewsArticle newsArticle)
        {
            // Validation
            if (string.IsNullOrWhiteSpace(newsArticle.Headline))
                return false;

            if (HeadlineExists(newsArticle.Headline))
                return false;

            // Check if category exists
            var category = _categoryRepository.GetById(newsArticle.CategoryId);
            if (category == null)
                return false;

            // Set default values
            newsArticle.CreatedDate = DateTime.Now;
            newsArticle.NewsStatus = newsArticle.NewsStatus; // Keep the provided status

            return _newsArticleRepository.Add(newsArticle);
        }

        public bool Update(NewsArticle newsArticle)
        {
            // Validation
            if (string.IsNullOrWhiteSpace(newsArticle.Headline))
                return false;

            if (HeadlineExists(newsArticle.Headline, newsArticle.NewsArticleId))
                return false;

            // Check if category exists
            var category = _categoryRepository.GetById(newsArticle.CategoryId);
            if (category == null)
                return false;

            // Check if article exists
            var existingArticle = GetById(newsArticle.NewsArticleId);
            if (existingArticle == null)
                return false;

            return _newsArticleRepository.Update(newsArticle);
        }

        public bool Delete(int id)
        {
            if (!CanDeleteArticle(id))
                return false;

            return _newsArticleRepository.Delete(id);
        }

        // Advanced search
        public List<NewsArticle> AdvancedSearch(string? headline = null, int? categoryId = null, int? authorId = null, 
            bool? status = null, DateTime? fromDate = null, DateTime? toDate = null)
        {
            return _newsArticleRepository.AdvancedSearch(headline, categoryId, authorId, status, fromDate, toDate);
        }

        // Validation
        public bool HeadlineExists(string headline, int? excludeId = null)
        {
            return _newsArticleRepository.HeadlineExists(headline, excludeId);
        }



        // Business logic methods
        public bool CanDeleteArticle(int articleId)
        {
            var article = GetById(articleId);
            if (article == null)
                return false;

            // Can always delete articles (no dependencies like categories)
            return true;
        }

        public bool PublishArticle(int articleId, int updatedBy)
        {
            var article = GetById(articleId);
            if (article == null)
                return false;

            // Validation: Article must have content to be published
            if (string.IsNullOrWhiteSpace(article.NewsContent))
                return false;

            article.NewsStatus = true;
            article.UpdatedBy = updatedBy;
            article.UpdatedDate = DateTime.Now;
            article.ModifiedDate = DateTime.Now;

            return _newsArticleRepository.Update(article);
        }

        public bool UnpublishArticle(int articleId, int updatedBy)
        {
            var article = GetById(articleId);
            if (article == null)
                return false;

            article.NewsStatus = false;
            article.UpdatedBy = updatedBy;
            article.UpdatedDate = DateTime.Now;
            article.ModifiedDate = DateTime.Now;

            return _newsArticleRepository.Update(article);
        }

        public List<NewsArticle> GetPublishedArticles()
        {
            return AdvancedSearch(status: true);
        }

        public List<NewsArticle> GetDraftArticles()
        {
            return AdvancedSearch(status: false);
        }

        public List<NewsArticle> GetArticlesByCurrentUser(int accountId)
        {
            return AdvancedSearch(authorId: accountId);
        }
    }
} 