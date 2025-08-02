using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PojistakNET.Models;
using PojistakNET.Services;
using System.Diagnostics;
using X.PagedList.Extensions;

namespace PojistakNET.Controllers;

[Authorize(Roles = "superadmin,admin")]
public class InsurerController : Controller
{
    // InsurerController spravuje JEN samotné pojištěnce,
    // tj. tvorba, editace a mazání profilu pojištěnců
    //
    // Detail pojištěnce je v InsuranceController.

    private readonly InsuranceContext _insuranceContext;
    private readonly ILogService _logService;

    // Konstruktor s oběma závislostmi
    public InsurerController(InsuranceContext insuranceContext, ILogService logService)
    {
        _insuranceContext = insuranceContext;  // Injikuje kontext pro práci s databází
        _logService = logService;  // Injikuje kontext pro logování
    }

    // Pojištěnci
    [HttpGet]
    [ActionName("Index")]
    [Route("insurer")]
    public IActionResult Insurer(int? page)
    {
        // Nastavíme velikost stránky (počet pojištěnců na jedné stránce)
        int pageSize = 5;
        int pageNumber = page ?? 1; // Pokud není stránka definována, vezmeme první stránku

        // Načte seznam pojištěnců z databáze, stránkování přímo na úrovni databáze
        var insurersPagedList = _insuranceContext.Insurers
            .OrderBy(i => i.LastName) // Řadíme podle příjmení
            .ToPagedList(pageNumber, pageSize); // Aplikujeme stránkování

        // Předání seznamu pojištěnců do view
        return View(insurersPagedList);
    }

    // Zobrazí formulář pro přidání nového pojištěnce
    [HttpGet, ActionName("Add")]
    public IActionResult AddInsurer() => View();



    // Zpracuje odeslaný formulář a přidá nového pojištěnce do databáze
    [HttpPost, ActionName("Add")]
    public async Task<IActionResult> AddInsurer(Insurer insurer)
    {
        if (ModelState.IsValid)
        {
            // Odstraní mezery z telefonního čísla
            insurer.Phone = insurer.Phone.Replace(" ", "");

            _insuranceContext.Insurers.Add(insurer);
            _insuranceContext.SaveChanges();

            // Logování úspěšného přidání            
            TempData["Success"] = "Přidání pojištěnce proběhlo úspěšně.";
            await _logService.LogAsync("Success", "Přidání pojištěnce proběhlo úspěšně.", User.Identity?.Name);


            return RedirectToAction("Index"); // Přesměrování po úspěšném přidání
        }
        else
        {
            // Logování chyb při validaci
            TempData["Error"] = "Přidání pojištěnce se nezdařilo. Zkontrolujte zadané údaje.";
            await _logService.LogAsync("Error", "Přidání pojištěnce se nezdařilo.", User.Identity?.Name);

        }



        return View("Add", insurer); // Zobrazí formulář znovu, pokud validace selže
    }


    // GET: Edit
    [HttpGet, ActionName("Edit")]
    public IActionResult EditInsurer(int insurerId)
    {
        var insurer = _insuranceContext.Insurers.Find(insurerId);
        if (insurer == null)
        {
            return NotFound();
        }
        return View("Edit", insurer);
    }

    // POST: Edit
    [HttpPost, ActionName("Edit")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditInsurer(int id, [Bind("Id,FirstName,LastName,Email,Phone,Street,City,Postcode")] Insurer insurer)
    {
        if (id != insurer.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _insuranceContext.Update(insurer);
                _insuranceContext.SaveChanges();
                return RedirectToAction("Index", "Insurer");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_insuranceContext.Insurers.Any(e => e.Id == insurer.Id))
                {
                    return NotFound();
                }
                else
                {
                    TempData["Success"] = "Editace pojištěnce proběhla úspěšně.";
                    await _logService.LogAsync("Success", "Editace pojištěnce (ID {insurer.Id} proběhla úspěšně.", User.Identity?.Name);
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
        var insurer = _insuranceContext.Insurers
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
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        // Najít pojištěnce skrze id a email
        var insurer = _insuranceContext.Insurers
            .Find(id);
        if (insurer == null)
        {
            return NotFound();
        }

        try
        {
            _insuranceContext.Insurers.Remove(insurer);
            await _insuranceContext.SaveChangesAsync();
            TempData["Success"] = "Smazání pojištěnce proběhlo úspěšně.";
            await _logService.LogAsync("Success", "Smazání pojištěnce (ID {insurer.Id} proběhlo úspěšně.", User.Identity?.Name);
        }
        catch
        {
            await _logService.LogAsync("Error", $"Pojištěnec (ID {insurer.Id}) nebyl úspěšně smazán.", User.Identity?.Name);

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
        var insurer = _insuranceContext.Insurers
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