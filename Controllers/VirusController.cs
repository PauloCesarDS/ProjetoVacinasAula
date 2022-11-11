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
    public class VirusController : Controller
    {
        private readonly LaboratorioVacinasContext _context;

        public VirusController(LaboratorioVacinasContext context)
        {
            _context = context;
        }

        // GET: Virus
        public async Task<IActionResult> Index()
        {
            List virus = new List<Virus>();

              foreach (var v in _context.Virus)
              {    
                    var vacinas = _context.Vacina.where(i => i.FkVirus == v.VirusId);
                    v.Vacinas = await vacinas.ToArrayAsync();
                    v.atualizarTemVacina();
                    virus.Add(v);
              }  
              return _context.Virus != null ? 
                          View(virus) :
                          Problem("Entity set 'LaboratorioVacinasContext.Virus'  is null.");
        }

        // GET: Virus/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Virus == null)
            {
                return NotFound();
            }

            var virus = await _context.Virus
                .FirstOrDefaultAsync(m => m.VirusId == id);
            if (virus == null)
            {
                return NotFound();
            }

            return View(virus);
        }

        // GET: Virus/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Virus/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("VirusId,Nome,TemVacina")] Virus virus)
        {
            if (ModelState.IsValid)
            {
                virus.TemVacina = false;
                _context.Add(virus);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(virus);
        }

        // GET: Virus/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Virus == null)
            {
                return NotFound();
            }

            var virus = await _context.Virus.FindAsync(id);
            if (virus == null)
            {
                return NotFound();
            }
            return View(virus);
        }

        // POST: Virus/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("VirusId,Nome,TemVacina")] Virus virus)
        {
            if (id != virus.VirusId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(virus);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VirusExists(virus.VirusId))
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
            return View(virus);
        }

        // GET: Virus/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Virus == null)
            {
                return NotFound();
            }

            var virus = await _context.Virus
                .FirstOrDefaultAsync(m => m.VirusId == id);
            if (virus == null)
            {
                return NotFound();
            }

            return View(virus);
        }

        // POST: Virus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Virus == null)
            {
                return Problem("Entity set 'LaboratorioVacinasContext.Virus'  is null.");
            }
            var virus = await _context.Virus.FindAsync(id);
            if (virus != null)
            {
                _context.Virus.Remove(virus);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VirusExists(int id)
        {
          return (_context.Virus?.Any(e => e.VirusId == id)).GetValueOrDefault();
        }
    }
}
