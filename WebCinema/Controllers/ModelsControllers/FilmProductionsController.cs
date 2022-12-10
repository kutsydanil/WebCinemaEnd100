using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CinemaCore.Models;
using WebCinema.Models.PageViewModels;
using WebCinema.Models.FilterViewModels;
using WebCinema.Models.SortViewModels;
using WebCinema.Models.IndexViewModels;
using WebCinema.Enum;


namespace WebCinema.Controllers
{
    public class FilmProductionsController : Controller
    {
        private readonly CinemaContext _context;

        public FilmProductionsController(CinemaContext context)
        {
            _context = context;
        }

        
        // GET: FilmProductions
        public async Task<IActionResult> Index(string? filmProductionName, int page = 1, SortState sortOrder = SortState.NameAsc)
        {
            int pageSize = 12;

            IQueryable<FilmProductions> filmProductions = _context.FilmProductions;

            if (!string.IsNullOrEmpty(filmProductionName))
            {
                filmProductions = filmProductions.Where(f => f.Name!.Contains(filmProductionName));
            }

            switch (sortOrder)
            {
                case SortState.NameDesc:
                    filmProductions = filmProductions.OrderByDescending(f => f.Name);
                    break;
                default:
                    filmProductions = filmProductions.OrderBy(f => f.Name);
                    break;
            }

            var count = await filmProductions.CountAsync();
            var items = await filmProductions.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            FilmProductionsViewModel viewModel = new FilmProductionsViewModel(items,
                new PageViewModel(count, page, pageSize),
                new FilmProductionFilterViewModel(filmProductionName),
                new FilmProductionSortViewModel(sortOrder));
            return View(viewModel);
        }

       
        // GET: FilmProductions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.FilmProductions == null)
            {
                return NotFound();
            }

            var filmProductions = await _context.FilmProductions
                .FirstOrDefaultAsync(m => m.Id == id);
            if (filmProductions == null)
            {
                return NotFound();
            }

            return View(filmProductions);
        }

        
        // GET: FilmProductions/Create
        public IActionResult Create()
        {
            return View();
        }

        
        // POST: FilmProductions/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] FilmProductions filmProductions)
        {
            if (ModelState.IsValid)
            {
                _context.Add(filmProductions);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(filmProductions);
        }

        // GET: FilmProductions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.FilmProductions == null)
            {
                return NotFound();
            }

            var filmProductions = await _context.FilmProductions.FindAsync(id);
            if (filmProductions == null)
            {
                return NotFound();
            }
            return View(filmProductions);
        }

        
        // POST: FilmProductions/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] FilmProductions filmProductions)
        {
            if (id != filmProductions.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(filmProductions);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FilmProductionsExists(filmProductions.Id))
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
            return View(filmProductions);
        }

        
        // GET: FilmProductions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.FilmProductions == null)
            {
                return NotFound();
            }

            var filmProductions = await _context.FilmProductions
                .FirstOrDefaultAsync(m => m.Id == id);
            if (filmProductions == null)
            {
                return NotFound();
            }

            return View(filmProductions);
        }

        
        // POST: FilmProductions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.FilmProductions == null)
            {
                return Problem("Entity set 'CinemaContext.FilmProductions'  is null.");
            }
            var filmProductions = await _context.FilmProductions.FindAsync(id);
            if (filmProductions != null)
            {
                _context.FilmProductions.Remove(filmProductions);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FilmProductionsExists(int id)
        {
          return (_context.FilmProductions?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
