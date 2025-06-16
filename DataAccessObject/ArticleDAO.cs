using BussinessObject;
using Microsoft.EntityFrameworkCore;

namespace DataAccessObject
{
    public class NewsArticleDAO
    {
        // Get all news articles with related data
        public List<NewsArticle> GetAll()
        {
            using (var context = new FunewsManagementContext())
            {
                return context.NewsArticles
                    .Include(n => n.Category)
                    .Include(n => n.Account)
                    .OrderByDescending(n => n.CreatedDate)
                    .ToList();
            }
        }

        // Get news article by ID with related data
        public NewsArticle? GetById(int id)
        {
            using (var context = new FunewsManagementContext())
            {
                return context.NewsArticles
                    .Include(n => n.Category)
                    .Include(n => n.Account)
                    .FirstOrDefault(n => n.NewsArticleId == id);
            }
        }

        // Get news articles by status
        public List<NewsArticle> GetByStatus(bool status)
        {
            using (var context = new FunewsManagementContext())
            {
                return context.NewsArticles
                    .Include(n => n.Category)
                    .Include(n => n.Account)
                    .Where(n => n.NewsStatus == status)
                    .OrderByDescending(n => n.CreatedDate)
                    .ToList();
            }
        }

        // Get news articles by category
        public List<NewsArticle> GetByCategory(int categoryId)
        {
            using (var context = new FunewsManagementContext())
            {
                return context.NewsArticles
                    .Include(n => n.Category)
                    .Include(n => n.Account)
                    .Where(n => n.CategoryId == categoryId)
                    .OrderByDescending(n => n.CreatedDate)
                    .ToList();
            }
        }

        // Get news articles by author
        public List<NewsArticle> GetByAuthor(int accountId)
        {
            using (var context = new FunewsManagementContext())
            {
                return context.NewsArticles
                    .Include(n => n.Category)
                    .Include(n => n.Account)
                    .Where(n => n.AccountId == accountId)
                    .OrderByDescending(n => n.CreatedDate)
                    .ToList();
            }
        }

        // Search news articles by headline
        public List<NewsArticle> SearchByHeadline(string keyword)
        {
            using (var context = new FunewsManagementContext())
            {
                return context.NewsArticles
                    .Include(n => n.Category)
                    .Include(n => n.Account)
                    .Where(n => n.Headline.Contains(keyword))
                    .OrderByDescending(n => n.CreatedDate)
                    .ToList();
            }
        }

        // Get news articles by date range
        public List<NewsArticle> GetByDateRange(DateTime fromDate, DateTime toDate)
        {
            using (var context = new FunewsManagementContext())
            {
                return context.NewsArticles
                    .Include(n => n.Category)
                    .Include(n => n.Account)
                    .Where(n => n.CreatedDate >= fromDate && n.CreatedDate <= toDate)
                    .OrderByDescending(n => n.CreatedDate)
                    .ToList();
            }
        }

        // Get latest news articles
        public List<NewsArticle> GetLatest(int count = 10)
        {
            using (var context = new FunewsManagementContext())
            {
                return context.NewsArticles
                    .Include(n => n.Category)
                    .Include(n => n.Account)
                    .Where(n => n.NewsStatus == true)
                    .OrderByDescending(n => n.CreatedDate)
                    .Take(count)
                    .ToList();
            }
        }

        // Get news articles with pagination
        public (List<NewsArticle> articles, int totalCount) GetWithPagination(int page, int pageSize, bool? status = null, int? categoryId = null)
        {
            using (var context = new FunewsManagementContext())
            {
                var query = context.NewsArticles
                    .Include(n => n.Category)
                    .Include(n => n.Account)
                    .AsQueryable();

                // Apply filters
                if (status.HasValue)
                {
                    query = query.Where(n => n.NewsStatus == status.Value);
                }

                if (categoryId.HasValue)
                {
                    query = query.Where(n => n.CategoryId == categoryId.Value);
                }

                var totalCount = query.Count();
                var articles = query
                    .OrderByDescending(n => n.CreatedDate)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                return (articles, totalCount);
            }
        }

        // Add new news article
        public bool Add(NewsArticle newsArticle)
        {
            try
            {
                using (var context = new FunewsManagementContext())
                {
                    newsArticle.CreatedDate = DateTime.Now;
                    context.NewsArticles.Add(newsArticle);
                    context.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                // Log exception for debugging if needed
                System.Diagnostics.Debug.WriteLine($"Exception in Add(): {ex.Message}");
                return false;
            }
        }

        // Update news article
        public bool Update(NewsArticle newsArticle)
        {
            try
            {
                using (var context = new FunewsManagementContext())
                {
                    var existingArticle = context.NewsArticles.Find(newsArticle.NewsArticleId);
                    if (existingArticle == null) return false;

                    existingArticle.Headline = newsArticle.Headline;
                    existingArticle.NewsContent = newsArticle.NewsContent;
                    existingArticle.NewsStatus = newsArticle.NewsStatus;
                    existingArticle.CategoryId = newsArticle.CategoryId;
                    existingArticle.UpdatedBy = newsArticle.UpdatedBy;
                    existingArticle.UpdatedDate = DateTime.Now;
                    existingArticle.ModifiedDate = DateTime.Now;

                    context.SaveChanges();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        // Delete news article
        public bool Delete(int id)
        {
            try
            {
                using (var context = new FunewsManagementContext())
                {
                    var newsArticle = context.NewsArticles.Find(id);
                    if (newsArticle == null) return false;

                    context.NewsArticles.Remove(newsArticle);
                    context.SaveChanges();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        // Check if headline exists (for validation)
        public bool HeadlineExists(string headline, int? excludeId = null)
        {
            using (var context = new FunewsManagementContext())
            {
                var query = context.NewsArticles.Where(n => n.Headline == headline);
                if (excludeId.HasValue)
                {
                    query = query.Where(n => n.NewsArticleId != excludeId.Value);
                }
                return query.Any();
            }
        }

        // Get count by status
        public int GetCountByStatus(bool status)
        {
            using (var context = new FunewsManagementContext())
            {
                return context.NewsArticles.Count(n => n.NewsStatus == status);
            }
        }

        // Get count by category
        public int GetCountByCategory(int categoryId)
        {
            using (var context = new FunewsManagementContext())
            {
                return context.NewsArticles.Count(n => n.CategoryId == categoryId);
            }
        }

        // Get count by author
        public int GetCountByAuthor(int accountId)
        {
            using (var context = new FunewsManagementContext())
            {
                return context.NewsArticles.Count(n => n.AccountId == accountId);
            }
        }

        // Get articles for dashboard statistics
        public Dictionary<string, int> GetStatistics()
        {
            using (var context = new FunewsManagementContext())
            {
                var stats = new Dictionary<string, int>
                {
                    ["Total"] = context.NewsArticles.Count(),
                    ["Published"] = context.NewsArticles.Count(n => n.NewsStatus == true),
                    ["Draft"] = context.NewsArticles.Count(n => n.NewsStatus == false),
                    ["ThisMonth"] = context.NewsArticles.Count(n => n.CreatedDate.Month == DateTime.Now.Month && n.CreatedDate.Year == DateTime.Now.Year),
                    ["Today"] = context.NewsArticles.Count(n => n.CreatedDate.Date == DateTime.Today)
                };
                return stats;
            }
        }

        // Advanced search with multiple criteria
        public List<NewsArticle> AdvancedSearch(string? headline = null, int? categoryId = null, int? authorId = null, 
            bool? status = null, DateTime? fromDate = null, DateTime? toDate = null)
        {
            using (var context = new FunewsManagementContext())
            {
                var query = context.NewsArticles
                    .Include(n => n.Category)
                    .Include(n => n.Account)
                    .AsQueryable();

                if (!string.IsNullOrEmpty(headline))
                {
                    query = query.Where(n => n.Headline.Contains(headline) || 
                                            (n.NewsContent != null && n.NewsContent.Contains(headline)));
                }

                if (categoryId.HasValue)
                {
                    query = query.Where(n => n.CategoryId == categoryId.Value);
                }

                if (authorId.HasValue)
                {
                    query = query.Where(n => n.AccountId == authorId.Value);
                }

                if (status.HasValue)
                {
                    query = query.Where(n => n.NewsStatus == status.Value);
                }

                if (fromDate.HasValue)
                {
                    query = query.Where(n => n.CreatedDate >= fromDate.Value);
                }

                if (toDate.HasValue)
                {
                    query = query.Where(n => n.CreatedDate <= toDate.Value);
                }

                return query.OrderByDescending(n => n.CreatedDate).ToList();
            }
        }
    }
}