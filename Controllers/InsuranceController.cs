// using PojistakNET.Migrations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PojistakNET.Models;
using PojistakNET.Services;

namespace PojistakNET.Controllers
{
    [Authorize(Roles = "superadmin,admin")]
    public class InsuranceController : Controller
    {
        // InsuranceController se zaměřuje na detail pojištěnce a pojištění
        // Respektive spravuje metody pro správu pojištění daného pojištěnce

        private readonly ILogService _logService;
        private readonly InsuranceContext _insuranceContext;

        // Konstruktor s oběma závislostmi
        public InsuranceController(InsuranceContext context, ILogService logService)
        {
            _insuranceContext = context;  // Injikuje kontext pro práci s databází
            _logService = logService;    // Injikuje logger pro logování
        }

        // Pojištění
        [HttpGet]
        [Route("insurance")]
        public async Task<IActionResult> Insurance()
        {
            var allInsurances = await _insuranceContext.Insurances.ToListAsync();
            return View(allInsurances);
        }


        private (string insurerName, int Id)? GetInsurerDetails(int insurerId)
        {
            var insurerDetails = _insuranceContext.Insurers
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

                _insuranceContext.Add(insurance);
                await _insuranceContext.SaveChangesAsync();
                TempData["Success"] = "Pojištění bylo úspěšně přiřazeno.";
                await _logService.LogAsync("Success", $"Pojištění s ID {insurance.Id} bylo úspěšně přiřazeno pojištěnci s ID {insurance.InsurerId}", User.Identity?.Name);
                
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
        public async Task<IActionResult> EditInsurance(int id, int insurerId)
        {
            var insurance = await _insuranceContext.Insurances.FindAsync(id);
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
        public async Task<IActionResult> EditInsurance(int id, Insurance insurance)
        {
            if (id != insurance.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Ověření, že pojištěnec existuje
                    var insurerExists = await _insuranceContext.Insurers
                        .FirstOrDefaultAsync(i => i.Id == insurance.InsurerId);

                    if (insurerExists == null)
                    {
                        ModelState.AddModelError("InsurerId", "Pojištěnec s tímto ID neexistuje.");
                        return View(insurance);
                    }

                    // Uložení změn pojištění
                    _insuranceContext.Update(insurance);
                    await _insuranceContext.SaveChangesAsync();

                    TempData["Success"] = "Pojištění bylo úspěšně zeditováno.";
                    await _logService.LogAsync("Success", $"Pojištění s ID {insurance.Id} pojištěnce s ID {insurance.InsurerId} bylo úspěšně zeditováno.", User.Identity?.Name);

                }
                catch (DbUpdateConcurrencyException)
                {
                    // Pokud pojištění neexistuje, vrať NotFound
                    if (!_insuranceContext.Insurances.Any(e => e.Id == insurance.Id))
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
        public async Task<IActionResult> DeleteInsurance(int id)
        {
            var insurance = await _insuranceContext.Insurances.FindAsync(id);
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
        public async Task<IActionResult> DeleteInsuranceConfirmed(int id)
        {
            var insurance = await _insuranceContext.Insurances.FindAsync(id);
            if (insurance != null)
            {
                // Pokud pojištění existuje, smažeme ho
                _insuranceContext.Insurances.Remove(insurance);
                await _insuranceContext.SaveChangesAsync();

                TempData["Success"] = $"Pojištění pojištěnce s ID {insurance.InsurerId} bylo úspěšně smazáno.";
                await _logService.LogAsync("Success", $"Pojištění s ID {insurance.Id} pojištěnce s ID {insurance.InsurerId} bylo úspěšně smazáno.", User.Identity?.Name);


                // Přesměrování zpět na detail pojištěnce
                return RedirectToAction("Detail", "Insurer", new { insurerId = insurance.InsurerId });
            }

            // Pokud pojištění neexistuje...
            return NotFound();
        }

        // Detail pojištění
        [HttpGet, ActionName("Detail")]
        public async Task<IActionResult> DetailInsurance(int id)
        {
            var insurance = await _insuranceContext.Insurances.FindAsync(id);
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