using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PojistakNET.Models;

namespace PojistakNET.Controllers
{
    public class HomeController : Controller
    {
        // GET: HomeController
        private readonly ApplicationDbContext _articleContext;

        // Konstruktor, kam injektuješ kontext
        public HomeController(ApplicationDbContext articleContext)
        {
            _articleContext = articleContext;
        }

        // Hlavní stránka
        public async Task<ActionResult> Index()
        {
            var articles = await _articleContext.Articles
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync();

            return View(articles);
        }

        // Články
        public async Task<IActionResult> Articles()
        {
            var articles = await _articleContext.Articles
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync();

            return View(articles);
        }

        // O aplikaci
        public IActionResult About()
        {
            return View();
        }

        // Kontakt - Kde je View pro kontakt?
        // public IActionResult Contact()
        // {
        //    return View();
        // }

        // Under Development
        public IActionResult UnderDevelopment()
        {
            return View();
        }

        // 404 Not Found -- neimplementováno


    }
}
