using BussinessObject;
using Microsoft.EntityFrameworkCore;

namespace DataAccessObject
{
    public class NewsArticleDAO
    {
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

        public (List<NewsArticle> articles, int totalCount) GetWithPagination(int page, int pageSize, bool? status = null, int? categoryId = null)
        {
            using (var context = new FunewsManagementContext())
            {
                var query = context.NewsArticles
                    .Include(n => n.Category)
                    .Include(n => n.Account)
                    .AsQueryable();

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
            catch
            {
                return false;
            }
        }

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

        public int GetCountByStatus(bool status)
        {
            using (var context = new FunewsManagementContext())
            {
                return context.NewsArticles.Count(n => n.NewsStatus == status);
            }
        }

        public int GetCountByCategory(int categoryId)
        {
            using (var context = new FunewsManagementContext())
            {
                return context.NewsArticles.Count(n => n.CategoryId == categoryId);
            }
        }

        public int GetCountByAuthor(int accountId)
        {
            using (var context = new FunewsManagementContext())
            {
                return context.NewsArticles.Count(n => n.AccountId == accountId);
            }
        }

        public Dictionary<string, int> GetStatistics()
        {
            using (var context = new FunewsManagementContext())
            {
                return new Dictionary<string, int>
                {
                    { "TotalArticles", context.NewsArticles.Count() },
                    { "PublishedArticles", context.NewsArticles.Count(n => n.NewsStatus == true) },
                    { "DraftArticles", context.NewsArticles.Count(n => n.NewsStatus == false) },
                    { "Categories", context.Categories.Count() },
                    { "Authors", context.SystemAccounts.Count() }
                };
            }
        }

        public List<NewsArticle> AdvancedSearch(string? headline = null, int? categoryId = null, int? authorId = null, 
            bool? status = null, DateTime? fromDate = null, DateTime? toDate = null)
        {
            using (var context = new FunewsManagementContext())
            {
                var query = context.NewsArticles
                    .Include(n => n.Category)
                    .Include(n => n.Account)
                    .AsQueryable();

                if (!string.IsNullOrWhiteSpace(headline))
                {
                    query = query.Where(n => n.Headline.Contains(headline));
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