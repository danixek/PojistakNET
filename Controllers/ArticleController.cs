using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PojistakNET.Models;
using PojistakNET.Services;

namespace PojistakNET.Controllers
{
    [Authorize(Roles = "superadmin,admin")]
    public class ArticleController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogService _logService;

        public ArticleController(ApplicationDbContext context, ILogService logService)
        {
            _context = context;
            _logService = logService;    // Injikuje logger pro logování
        }
        
        [AllowAnonymous]
        [Route("Home")]
        public async Task<IActionResult> Detail(int id)
        {
            var article = await _context.Articles.FirstOrDefaultAsync(a => a.Id == id);
            if (article == null) return NotFound();
            return View(article);
        }


        // Zobraz seznam článků - admin správa
        [ActionName("Index")]
        public async Task<IActionResult> ManageArticles()
        {
            var articles = await _context.Articles.OrderByDescending(a => a.CreatedAt).ToListAsync();
            return View(articles);
        }

        // Zobraz formulář pro vytvoření článku
        [ActionName("Create")]
        public IActionResult CreateArticle()
        {
            return View();
        }

        // Zpracování formuláře pro vytvoření článku
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Create")]
        public async Task<IActionResult> CreateArticle(Article article)
        {
            if (ModelState.IsValid)
            {
                article.CreatedAt = DateTime.Now;
                _context.Add(article);
                await _context.SaveChangesAsync();
                await _logService.LogAsync("Success", $"Článek byl úspěšně přidán.", User.Identity?.Name);
                TempData["Message"] = $"Článek byl úspěšně přidán.";

                return RedirectToAction("Index");
            }
            return View(article);
        }

        // Editace článku
        [ActionName("Edit")]
        public async Task<IActionResult> EditArticle(int? id)
        {
            if (id == null) return NotFound();

            var article = await _context.Articles.FindAsync(id);
            if (article == null) return NotFound();

            return View(article);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Edit")]
        public async Task<IActionResult> EditArticle(int id, Article article)
        {
            if (id != article.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(article);
                    await _context.SaveChangesAsync();
                    await _logService.LogAsync("Success", $"Článek byl úspěšně zeditován.", User.Identity?.Name);
                    TempData["Message"] = $"Článek byl úspěšně zeditován.";
                    return RedirectToAction("Index");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Articles.Any(e => e.Id == id)) return NotFound();
                    else throw;
                }
            }
            return View(article);
        }

        // Smazání článku
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteArticle(int? id)
        {
            if (id == null) return NotFound();

            var article = await _context.Articles.FindAsync(id);
            if (article == null) return NotFound();

            return View(article);
        }

        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteArticleConfirmed(int id)
        {
            var article = await _context.Articles.FindAsync(id);
            if (article == null)
            {
                // Článek neexistuje, můžeš vrátit NotFound nebo přesměrovat s hláškou
                return NotFound();
            }
            _context.Articles.Remove(article);
            await _context.SaveChangesAsync();

            await _logService.LogAsync("Warning", $"Článek úspěšně smazán.", User.Identity?.Name);
            TempData["Message"] = $"Článek byl úspěšně smazán.";
            return RedirectToAction("Index");
        }
    }

}
