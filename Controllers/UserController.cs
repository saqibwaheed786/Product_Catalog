using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductCatalog.Data;
using ProductCatalog.Models;
using RMS.Services.UserService;
using System.Security.Claims;
using System.Text;

namespace ProductCatalog.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly ProductCatalogContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserController(IHttpContextAccessor httpContextAccessor, ProductCatalogContext dbContext ,IUserRepository userRepository)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
            _userRepository = userRepository;
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

        [HttpGet]
        public IActionResult Login()
        {
            // Display the login form
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string login, string password)
        {
            // Handle user authentication
            var user = AuthenticateUser(login, password);

            if (user != null)
            {
                var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString())
                // Add other claims as needed
            };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                
                // Store the user ID in the session
                byte[] userIdBytes = Encoding.ASCII.GetBytes(user?.UserId.ToString() ?? string.Empty);
                HttpContext.Session.Set("UserId", userIdBytes);
                
                TempData["Greetings"] = "Welcome, " + user.Name;

                return RedirectToAction("Index", "Article");
            }

            ModelState.AddModelError("", "Invalid login attempt");
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            // Log out the current user
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "User");
        }

        public IActionResult MyArticles()
        {
            // Display a linked list of articles for the logged-in user
            var user = GetCurrentUser();
            if(user == null)
            {
                RedirectToAction("Login", "User");
            }

            var articles = _userRepository.GetArticlesByUser(user.Name);
            return View(articles);
        }

        public User AuthenticateUser(string login, string password)
        {
            // Retrieve user by login and password (username)
            var user = _dbContext.users.FirstOrDefault(u => u.Login == login && u.Password == password);

            if (user != null)
            {
                // Password is valid
                return user;
            }

            // Invalid login attempt
            return null;
        }
    }
}
