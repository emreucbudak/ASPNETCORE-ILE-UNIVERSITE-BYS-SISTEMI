using Microsoft.AspNetCore.Mvc;
using WebApplication8.Data;
using WebApplication8.Models;
using Microsoft.EntityFrameworkCore;

namespace WebApplication8.Controllers.WebController
{
    public class AccountController : Controller
    {
        private readonly RepositoryDbContext _context;

        public AccountController(RepositoryDbContext context)
        {
            _context = context;
        }

        // GET: Login
        public IActionResult LoginUser()
        {
            var userRole = HttpContext.Session.GetString("Role");

            if (!string.IsNullOrEmpty(userRole))
            {
                if (userRole == "Student")
                {
                    // Öğrenci rolü ile ana sayfaya yönlendir
                    return RedirectToAction("Index", "Student");
                }
                else if (userRole == "Advisor")
                {
                    // Danışman rolü ile ana sayfaya yönlendir
                    return RedirectToAction("Index", "Advisor");
                }
            }
            return View();
        }

        // POST: Login
        [HttpPost]
        public async Task<IActionResult> LoginUser(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Kullanıcıyı veritabanından al
                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.Username == model.Username && u.PasswordHash == model.Password && u.Role == model.Role);

                if (user != null)
                {
                    // Kullanıcı başarıyla giriş yaptı, session'a kullanıcı bilgilerini ekle
                    HttpContext.Session.SetString("UserID", user.RelatedID.ToString());
                    HttpContext.Session.SetString("Username", user.Username);
                    HttpContext.Session.SetString("Role", user.Role);

                    // Kullanıcı rolüne göre yönlendir
                    if (user.Role == "Student")
                    {
                        // Kullanıcı giriş yaptıktan sonra
                        HttpContext.Session.SetString("StudentID", user.RelatedID.ToString());

                        return RedirectToAction("Index", "Student", new { id = user.RelatedID });
                    }
                    else if (user.Role == "Advisor")
                    {
                        return RedirectToAction("Index", "Advisor", new { id = user.RelatedID });
                    }
                }
                else
                {
                    ViewBag.Message = "Invalid username, password, or role.";
                }
            }
            return View(model);
        }
        public IActionResult Logout()
        {
            // Oturumu kapat
            HttpContext.Session.Clear();

            // Kullanıcıyı giriş sayfasına yönlendir
            return RedirectToAction("LoginUser", "Account");
        }


    }
}
