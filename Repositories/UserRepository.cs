using Microsoft.AspNetCore.Http;
using ProductCatalog.Data;
using RMS.Services.UserService;
using System.Text;

namespace ProductCatalog.Models
{
    public class UserRepository : IUserRepository
    {
        private readonly ProductCatalogContext _context;

        public UserRepository(ProductCatalogContext context)
        {
            _context = context;
        }

        public User AuthenticateUser(string login, string password)
        {
            return _context.users.SingleOrDefault(u => u.Login == login && u.Password == password);
        }

        public User GetUserById(int userId)
        {
            return _context.users.Find(userId);
        }

        public List<Article> GetArticlesByUser(string username)
        {
            return _context.articles.Where(a => a.Author == username)
                                    .OrderByDescending(a => a.ViewCount)
                                    .ToList();
        }

    }
}
