using ProductCatalog.Models;

namespace RMS.Services.UserService
{
    public interface IUserRepository
    {
        //IEnumerable<User> GetAll();
        public User AuthenticateUser(string login, string password);
        public User GetUserById(int userId);
        public List<Article> GetArticlesByUser(string username);

    }
}
