using ProductCatalog.Data;
using ProductCatalog.Models;
using ProductCatalog.Repositories;

//using ProductCatalog.Repositories; // Ensure the correct namespace is used
using System.Collections.Generic;
using System.Linq;

namespace ProductCatalog.Manager
{
    public class ProductCatalogManager
    {
        private readonly IArticleRepository _articleRepository;
        private readonly ProductCatalogContext _dbContext;

        public ProductCatalogManager(IArticleRepository articleRepository, ProductCatalogContext dbContext)
        {
            _articleRepository = articleRepository;
            _dbContext = dbContext;
        }

        public List<Article> GetDashboard()
        {
            var dashboardLinks = new List<Article>();
            foreach (var articleType in new[] { "DVD", "CD", "book", "other" })
            {
                dashboardLinks.AddRange(_articleRepository.GetArticlesByType(articleType).Take(5));
            }
            return dashboardLinks;
        }

        public List<Article> GetUserEntries(string userName)
        {
            return _articleRepository.GetArticlesByUser(userName);
        }

        public void CreateNewArticle(Article article)
        {
            _articleRepository.CreateArticle(article);
        }

        public Article GetArticleDetails(int articleId)
        {
            var article = _articleRepository.GetArticleById(articleId);

            if (article != null)
            {
                article.ViewCount = IncreaseViewCount(article.ViewCount);
                _dbContext.SaveChanges();
            }

            return article;
        }

        public int IncreaseViewCount(int view)
        {
            // Increment the view count
            view++;

            // Return the updated view count
            return view;
        }


        public void EditArticle(Article article)
        {
            _articleRepository.UpdateArticle(article);
        }

        // Add the missing method
        public List<Article> GetArticlesByType(string articleType)
        {
            return _articleRepository.GetArticlesByType(articleType);
        }
    }
}

