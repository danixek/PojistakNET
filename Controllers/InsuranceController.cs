// using PojistakNET.Migrations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using PojistakNET.Models;

namespace PojistakNET.Controllers
{
    [Authorize(Roles = "superadmin,admin")]
    public class InsuranceController : Controller
    {
        // InsuranceController se zaměřuje na detail pojištěnce a pojištění
        // Respektive spravuje metody pro správu pojištění daného pojištěnce

        private readonly ILogger<InsuranceController> _logger;
        private readonly InsuranceContext _context;

        // Konstruktor s oběma závislostmi
        public InsuranceController(InsuranceContext context, ILogger<InsuranceController> logger)
        {
            _context = context;  // Injikuje kontext pro práci s databází
            _logger = logger;    // Injikuje logger pro logování
        }

        // Pojištění
        [HttpGet]
        [Route("insurance")]
        public async Task<IActionResult> Insurance()
        {
            var allInsurances = await _context.Insurances.ToListAsync();
            return View(allInsurances);
        }


        private (string insurerName, int Id)? GetInsurerDetails(int insurerId)
        {
            var insurerDetails = _context.Insurers
                .Where(i => i.Id == insurerId)
                .Select(i => new { insurerName = i.FirstName + " " + i.LastName, i.Id })
                .FirstOrDefault();

            // Vrátíme null, pokud pojištěnec neexistuje
            if (insurerDetails == null)
            {
                return null;
            }

            return (insurerDetails.insurerName, insurerDetails.Id);
        }

        [HttpGet, ActionName("Add")]
        public IActionResult AddInsurance(int insurerId)
        {

            var insurer = GetInsurerDetails(insurerId);

            // Zkontrolujeme, jestli pojištěnec existuje
            if (insurer == null)
            {
                // Pokud neexistuje, vrátíme 404 stránku nebo přesměrujeme na jinou akci
                return NotFound("[Pojištěnec nebyl nalezen]");
            }

            ViewBag.InsurerName = insurer.Value.insurerName;
            ViewBag.InsurerId = insurer.Value.Id;
            return View();
        }

        [HttpPost, ActionName("Add")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddInsurance(int insurerId, Insurance insurance)
        {
            if (ModelState.IsValid)
            {
                insurance.InsurerId = insurerId; // přiřazení pojištění k pojištěnci

                _context.Add(insurance);
                await _context.SaveChangesAsync();
                return RedirectToAction("Detail", "Insurer", new { insurerId });

            }
            var insurer = GetInsurerDetails(insurerId);

            if (insurer.HasValue)
            {
                // Pokud validace selže, vrátí se formulář se stejnými daty
                ViewBag.InsurerName = insurer.Value.insurerName;
            }

            // Zajištění, že pojištění obsahuje správné InsurerId při neúspěšné validaci
            insurance.InsurerId = insurerId;

            return View(insurance);
        }

        // Editace pojištění
        [HttpGet, ActionName("Edit")]
        public async Task<IActionResult> EditInsurance(int Id, int insurerId)
        {
            var insurance = await _context.Insurances.FindAsync(Id);
            if (insurance == null)
            {
                return NotFound();
            }
            // Zajistí, že InsurerId bude přítomné ve formuláři
            insurance.InsurerId = insurerId;


            var insurer = GetInsurerDetails(insurerId);

            if (insurer.HasValue)
            {
                // Pokud validace selže, vrátí se formulář se stejnými daty
                ViewBag.InsurerName = insurer.Value.insurerName;
            }

            return View(insurance);
        }

        // Uložení změn pojištění
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditInsurance(int Id, Insurance insurance)
        {
            if (Id != insurance.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Ověření, že pojištěnec existuje
                    var insurerExists = await _context.Insurers
                        .FirstOrDefaultAsync(i => i.Id == insurance.InsurerId);

                    if (insurerExists == null)
                    {
                        ModelState.AddModelError("InsurerId", "Pojištěnec s tímto ID neexistuje.");
                        return View(insurance);
                    }

                    // Uložení změn pojištění
                    _context.Update(insurance);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    // Pokud pojištění neexistuje, vrať NotFound
                    if (!_context.Insurances.Any(e => e.Id == insurance.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                // Po uložení přesměruj na detail pojištěnce
                return RedirectToAction("Detail", "Insurer", new { insurerId = insurance.InsurerId });
            }

            return View(insurance);
        }

        // Odstranění pojištění
        [HttpGet, ActionName("Delete")]
        public async Task<IActionResult> DeleteInsurance(int Id)
        {
            var insurance = await _context.Insurances.FindAsync(Id);
            if (insurance == null)
            {
                return NotFound();
            }
            int insurerId = insurance.InsurerId;

            var insurer = GetInsurerDetails(insurerId);

            if (insurer.HasValue)
            {
                // Pokud validace selže, vrátí se formulář se stejnými daty
                ViewBag.InsurerName = insurer.Value.insurerName;
            }

            return View(insurance);
        }

        // Potvrzení odstranění pojištění
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteInsuranceConfirmed(int Id)
        {
            var insurance = await _context.Insurances.FindAsync(Id);
            if (insurance != null)
            {
                // Pokud pojištění existuje, smažeme ho
                _context.Insurances.Remove(insurance);
                await _context.SaveChangesAsync();

                // Přesměrování zpět na detail pojištěnce
                return RedirectToAction("Detail", "Insurer", new { insurerId = insurance.InsurerId });
            }

            // Pokud pojištění neexistuje...
            return NotFound();
        }

        // Detail pojištění
        [HttpGet, ActionName("Detail")]
        public async Task<IActionResult> DetailInsurance(int Id)
        {
            var insurance = await _context.Insurances.FindAsync(Id);
            if (insurance == null)
            {
                return NotFound();
            }

            int insurerId = insurance.InsurerId;

            var insurer = GetInsurerDetails(insurerId);

            if (insurer.HasValue)
            {
                // Pokud validace selže, vrátí se formulář se stejnými daty
                ViewBag.InsurerName = insurer.Value.insurerName;
            }

            return View(insurance);
        }

    }
}