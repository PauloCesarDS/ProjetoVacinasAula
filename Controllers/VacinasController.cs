using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LaboratorioVacinas.Models;

namespace LaboratorioVacinas.Controllers
{
    public class VacinasController : Controller
    {
        private readonly LaboratorioVacinasContext _context;

        public VacinasController(LaboratorioVacinasContext context)
        {
            _context = context;
        }

        // GET: Vacinas
        public async Task<IActionResult> Index()
        {
            var laboratorioVacinasContext = _context.Vacina.Include(v => v.Virus);
            return View(await laboratorioVacinasContext.ToListAsync());
        }

        // GET: Vacinas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Vacina == null)
            {
                return NotFound();
            }

            var vacina = await _context.Vacina
                .Include(v => v.Virus)
                .FirstOrDefaultAsync(m => m.VacinaId == id);
            if (vacina == null)
            {
                return NotFound();
            }

            return View(vacina);
        }

        // GET: Vacinas/Create
        public IActionResult Create()
        {
            ViewData["FkVirus"] = new SelectList(_context.Virus, "VirusId", "Nome");
            return View();
        }

        // POST: Vacinas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("VacinaId,Nome,DataCriacao,FkVirus")] Vacina vacina)
        {
            if (ModelState.IsValid)
            {
                _context.Add(vacina);
                var virus = _context.Virus.Find(vacina.FkVirus);
                virus.TemVacina = true;
                await _context.SaveChangesAsync();
                _context.Update(virus);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["FkVirus"] = new SelectList(_context.Virus, "VirusId", "Nome", vacina.FkVirus);
            return View(vacina);
        }

        // GET: Vacinas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Vacina == null)
            {
                return NotFound();
            }

            var vacina = await _context.Vacina.FindAsync(id);
            if (vacina == null)
            {
                return NotFound();
            }
            ViewData["FkVirus"] = new SelectList(_context.Virus, "VirusId", "VirusId", vacina.FkVirus);
            return View(vacina);
        }

        // POST: Vacinas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("VacinaId,Nome,DataCriacao,FkVirus")] Vacina vacina)
        {
            if (id != vacina.VacinaId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(vacina);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VacinaExists(vacina.VacinaId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["FkVirus"] = new SelectList(_context.Virus, "VirusId", "VirusId", vacina.FkVirus);
            return View(vacina);
        }

        // GET: Vacinas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Vacina == null)
            {
                return NotFound();
            }

            var vacina = await _context.Vacina
                .Include(v => v.Virus)
                .FirstOrDefaultAsync(m => m.VacinaId == id);
            if (vacina == null)
            {
                return NotFound();
            }

            return View(vacina);
        }

        // POST: Vacinas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Vacina == null)
            {
                return Problem("Entity set 'LaboratorioVacinasContext.Vacina'  is null.");
            }
            var vacina = await _context.Vacina.FindAsync(id);
            if (vacina != null)
            {
                _context.Vacina.Remove(vacina);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VacinaExists(int id)
        {
          return (_context.Vacina?.Any(e => e.VacinaId == id)).GetValueOrDefault();
        }
    }
}
