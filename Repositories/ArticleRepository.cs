using ProductCatalog.Data;
using Microsoft.EntityFrameworkCore;
using ProductCatalog.Repositories;


namespace ProductCatalog.Models
{
    public class ArticleRepository : IArticleRepository
    {
        private readonly ProductCatalogContext _dbContext;

        public ArticleRepository(ProductCatalogContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<Article> GetArticlesByType(string articleType)
        {
            return _dbContext.articles
                .Where(a => a.ArticleType == articleType)
                .OrderByDescending(a => a.ViewCount)
                .ToList();
        }
        //public List<Article> GetArticlesByType(string articleType)
        //{
        //    return _dbContext.articles.Where(a => a.ArticleType == articleType).ToList();
        //}
        public Article GetArticleById(int articleId)
        {
            var article = _dbContext.articles.Find(articleId);
            //if (article != null)
            //{
            //    article.ViewCount++;
            //    _dbContext.SaveChanges();
            //}
            return article;
        }

        public List<Article> GetArticlesByUser(string userName)
        {
            return _dbContext.articles
                .Where(a => a.Author == userName)
                .OrderByDescending(a => a.ViewCount)
                .ToList();
        }

        public void CreateArticle(Article article)
        {
            _dbContext.articles.Add(article);
            _dbContext.SaveChanges();
        }

        public void UpdateArticle(Article article)
        {
            // Find the existing article in the database
            var previousArticle = _dbContext.articles.FirstOrDefault(a => a.ArticleId == article.ArticleId);

            if (previousArticle != null)
            {
                // Update the properties of the existing article with the new values
                previousArticle.Title = article.Title;
                previousArticle.PublicationYear = article.PublicationYear;
                previousArticle.ArticleType = article.ArticleType;
                previousArticle.ContentText = article.ContentText;
                previousArticle.Genre = article.Genre;

                // Save changes to the database
                _dbContext.SaveChanges();
            }
            else
            {
                // Handle the case where the article with the specified ID is not found
                // You might throw an exception or handle it in a way that makes sense for your application
                // For example:
                throw new InvalidOperationException($"Article with ID {article.ArticleId} not found.");
            }
        }

    }

}
