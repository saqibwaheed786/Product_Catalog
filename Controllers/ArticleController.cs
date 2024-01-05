using ProductCatalog.Manager;
using ProductCatalog.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using Microsoft.AspNetCore.Http;
using RMS.Services.UserService;
using ProductCatalog.Data;
using Microsoft.EntityFrameworkCore;

namespace ProductCatalog.Controllers
{
    public class ArticleController : Controller
    {
        private readonly ProductCatalogManager _productCatalogManager;
        private readonly IUserRepository _userRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ProductCatalogContext _dbContext;

        public ArticleController(IHttpContextAccessor httpContextAccessor, ProductCatalogManager productCatalogManager, IUserRepository userRepository, ProductCatalogContext dbContext)
        {
            _productCatalogManager = productCatalogManager;
            _httpContextAccessor = httpContextAccessor;
            _userRepository = userRepository;
            _dbContext = dbContext;
        }
        public IActionResult Index()
        {
            return View();
        }

        public User? GetCurrentUser()
        {
            if (_httpContextAccessor.HttpContext != null)
            {
                // Get the current user's ID from the session
                byte[]? userIdBytes;
                if (!_httpContextAccessor.HttpContext.Session.TryGetValue("UserId", out userIdBytes) || userIdBytes == null)
                {
                    return null; // or throw an exception, depending on your requirements
                }

                int userId = Int32.Parse(Encoding.ASCII.GetString(userIdBytes));

                User? user = _userRepository.GetUserById(userId);

                return user;
            }
            return null;
        }

        public List<Article> ListArticles(string articleType)
        {
            var articles = _productCatalogManager.GetArticlesByType(articleType);
            return articles;
        }

        public ActionResult ShowArticle(int articleId)
        {
            var article = _productCatalogManager.GetArticleDetails(articleId);

            return View(article);
        }

        public ActionResult MyArticles()
        {
            var user = GetCurrentUser();
            if (user == null)
            {
                RedirectToAction("Login", "User");
            }

            var userEntries = _productCatalogManager.GetUserEntries(user.Name);
            return View(userEntries);
        }

        public ActionResult DVDArticles()
        {
            var user = GetCurrentUser();
            if (user == null)
            {
                RedirectToAction("Login", "User");
            }
            var articles = ListArticles("DVD");
            return View(articles);
        }

        public ActionResult CDArticles()
        {
            var user = GetCurrentUser();
            if (user == null)
            {
                RedirectToAction("Login", "User");
            }
            var articles = ListArticles("CD");
            return View(articles);
        }

        public ActionResult bookArticles()
        {
            var user = GetCurrentUser();
            if (user == null)
            {
                RedirectToAction("Login", "User");
            }
            var articles = ListArticles("book");
            return View(articles);
        }

        public ActionResult otherArticles()
        {
            var user = GetCurrentUser();
            if (user == null)
            {
                RedirectToAction("Login", "User");
            }
            var articles = ListArticles("other");
            return View(articles);
        }

        [HttpGet]
        public ActionResult CreateArticle()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateArticle(Article article)
        {
            var user = GetCurrentUser();
            if (user == null)
            {
                RedirectToAction("Login", "User");
            }

            article.Author = user.Name;
            _productCatalogManager.CreateNewArticle(article);
            return RedirectToAction("MyArticles", "User");
        }

        public ActionResult EditArticle(int articleId)
        {
            var article = _dbContext.articles.FirstOrDefault(a => a.ArticleId == articleId);
            return View(article);
        }

        [HttpPost]
        public ActionResult EditArticle(Article article)
        {
            _productCatalogManager.EditArticle(article);
            return RedirectToAction("MyArticles", "User");
        }
    }
}