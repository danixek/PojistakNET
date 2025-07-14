using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PojistakNET.Models;
using System.Diagnostics;
using X.PagedList;
using X.PagedList.Extensions;

namespace PojistakNET.Controllers;

[Authorize(Roles = "superadmin,admin")]
public class InsurerController : Controller
{
    // InsurerController spravuje JEN samotné pojištěnce,
    // tj. tvorba, editace a mazání profilu pojištěnců
    //
    // Detail pojištěnce je v InsuranceController.

    private readonly ILogger<InsurerController> _logger;
    private readonly InsuranceContext _context;

    // Konstruktor s oběma závislostmi
    public InsurerController(InsuranceContext context, ILogger<InsurerController> logger)
    {
        _context = context;  // Injikuje kontext pro práci s databází
        _logger = logger;    // Injikuje logger pro logování
    }

    // Pojištěnci
    [HttpGet]
    [Route("insurer")]
    public IActionResult Insurer(int? page)
    {
        // Nastavíme velikost stránky (počet pojištěnců na jedné stránce)
        int pageSize = 5;
        int pageNumber = page ?? 1; // Pokud není stránka definována, vezmeme první stránku

        // Načte seznam pojištěnců z databáze, stránkování přímo na úrovni databáze
        var insurersPagedList = _context.Insurers
            .OrderBy(i => i.LastName) // Řadíme podle příjmení
            .ToPagedList(pageNumber, pageSize); // Aplikujeme stránkování

        // Přidání logování pro diagnostiku
        _logger.LogInformation("Pojištěnci načteni: " + insurersPagedList.Count);

        // Předání seznamu pojištěnců do view
        return View(insurersPagedList);
    }

    // Zobrazí formulář pro přidání nového pojištěnce
    [HttpGet, ActionName("Add")]
    public IActionResult AddInsurer() => View();



    // Zpracuje odeslaný formulář a přidá nového pojištěnce do databáze
    [HttpPost, ActionName("Add")]
    public IActionResult AddInsurer(Insurer insurer)
    {
        if (ModelState.IsValid)
        {
            // Odstraní mezery z telefonního čísla
            insurer.Phone = insurer.Phone.Replace(" ", "");

            _context.Insurers.Add(insurer);
            _context.SaveChanges();

            // Logování úspěšného přidání            
            TempData["Success"] = "Přidání pojištěnce proběhlo úspěšně.";
            _logger.LogInformation("Pojištěnec přidán: {FirstName} {LastName}", insurer.FirstName, insurer.LastName);

            return RedirectToAction("Insurer"); // Přesměrování po úspěšném přidání
        }
        else
        {
            // Logování chyb při validaci
            TempData["Error"] = "Přidání pojištěnce se nezdařilo. Zkontrolujte zadané údaje.";
            _logger.LogWarning("Přidání pojištěnce selhalo: {Errors}", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
        }

        return View("Add", insurer); // Zobrazí formulář znovu, pokud validace selže
    }


    // GET: Edit
    [HttpGet, ActionName("Edit")]
    public IActionResult EditInsurer(int insurerId)
    {
        var insurer = _context.Insurers.Find(insurerId);
        if (insurer == null)
        {
            return NotFound();
        }
        return View("Edit", insurer);
    }

    // POST: Edit
    [HttpPost, ActionName("Edit")]
    [ValidateAntiForgeryToken]
    public IActionResult EditInsurer(int Id, [Bind("Id,FirstName,LastName,Email,Phone,Street,City,Postcode")] Insurer insurer)
    {
        if (Id != insurer.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(insurer);
                _context.SaveChanges();
                return RedirectToAction("Index", "Insurer");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Insurers.Any(e => e.Id == insurer.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }
        return View("Edit", insurer);
    }

    // GET: Delete
    [HttpGet, ActionName("Delete")]
    public IActionResult DeleteInsurer(int insurerId)
    {
        var insurer = _context.Insurers
            .Find(insurerId);

        if (insurer == null)
        {
            return NotFound();
        }

        return View("Delete", insurer);
    }

    // POST: Delete
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int Id)
    {
        // Najít pojištěnce skrze id a email
        var insurer = _context.Insurers
            .Find(Id);
        if (insurer == null)
        {
            return NotFound();
        }

        try
        {
            _context.Insurers.Remove(insurer);
            _context.SaveChanges();
        }
        catch (Exception ex)
        {
            // Logování chyb
            _logger.LogError(ex, "Chyba při mazání pojištěnce s ID {Id}", Id);
            return StatusCode(500, "Chyba serveru. Záznam nebyl smazán.");
        }

        return RedirectToAction("Index", "Insurer");
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    // Detail pojištěnce
    [HttpGet, ActionName("Detail")]
    public IActionResult DetailInsurer(int insurerId)
    {
        var insurer = _context.Insurers
            .Include(i => i.Insurances)  // Načteme také pojištění (vztah 1:N)
            .FirstOrDefault(i => i.Id == insurerId);  // Získáme prvního pojištěnce s tímto ID


        // 404 not found - pokud pojištěnec neexistuje
        if (insurer == null)
        {
            return NotFound();
        }

        // Vrátí profil pojištěnce
        return View("Detail", insurer);
    }
}