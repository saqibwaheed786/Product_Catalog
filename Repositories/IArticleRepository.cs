using ProductCatalog.Models;

namespace ProductCatalog.Repositories
{
    public interface IArticleRepository
    {
        List<Article> GetArticlesByType(string articleType);
        Article GetArticleById(int articleId);
        List<Article> GetArticlesByUser(string userName);
        void CreateArticle(Article article);
        void UpdateArticle(Article article);
    }
}
