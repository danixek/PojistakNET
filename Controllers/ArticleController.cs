using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PojistakNET.Models;

namespace PojistakNET.Controllers
{
    [Authorize(Roles = "superadmin,admin")]
    public class ArticleController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ArticleController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Zobraz seznam článků
        public async Task<IActionResult> ManageArticles()
        {
            var articles = await _context.Articles.OrderByDescending(a => a.CreatedAt).ToListAsync();
            return View(articles);
        }

        // Zobraz formulář pro vytvoření článku
        public IActionResult CreateArticle()
        {
            return View();
        }

        // Zpracování formuláře pro vytvoření článku
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateArticle(Article article)
        {
            if (ModelState.IsValid)
            {
                article.CreatedAt = DateTime.Now;
                _context.Add(article);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(ManageArticles));
            }
            return View(article);
        }

        // Editace článku
        public async Task<IActionResult> EditArticle(int? id)
        {
            if (id == null) return NotFound();

            var article = await _context.Articles.FindAsync(id);
            if (article == null) return NotFound();

            return View(article);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditArticle(int id, Article article)
        {
            if (id != article.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(article);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(ManageArticles));
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
        public async Task<IActionResult> DeleteArticle(int? id)
        {
            if (id == null) return NotFound();

            var article = await _context.Articles.FindAsync(id);
            if (article == null) return NotFound();

            return View(article);
        }

        [HttpPost, ActionName("DeleteArticle")]
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
            return RedirectToAction(nameof(ManageArticles));
        }
    }

}
