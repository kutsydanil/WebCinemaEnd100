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
using WebCinema.Services;
using Microsoft.AspNetCore.Authorization;

namespace WebCinema.Controllers
{
    [Authorize()]
    public class FilmProductionsController : Controller
    {
        private readonly CinemaContext _context;
        private string Name = "FilmProductions";
        private GenericMemoryCache<FilmProductions> _cache;

        public FilmProductionsController(CinemaContext context, GenericMemoryCache<FilmProductions> cache)
        {
            _context = context;
            _cache = cache;
        }

        
        // GET: FilmProductions
        public IActionResult Index(string? filmProductionName, int page = 1, SortState sortOrder = SortState.NameAsc)
        {
            int pageSize = 12;

            IEnumerable<FilmProductions> filmProductions = _cache.GetAll(Name);

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

            var count = filmProductions.Count();
            var items = filmProductions.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            FilmProductionsViewModel viewModel = new FilmProductionsViewModel(items,
                new PageViewModel(count, page, pageSize),
                new FilmProductionFilterViewModel(filmProductionName),
                new FilmProductionSortViewModel(sortOrder));
            return View(viewModel);
        }

       
        // GET: FilmProductions/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null || _cache.GetAll(Name) == null)
            {
                return NotFound();
            }
            var filmProductions = _cache.GetAll(Name).FirstOrDefault(f => f.Id == id);
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
                _cache.CreateCache(Name);
                return RedirectToAction(nameof(Index));
            }
            return View(filmProductions);
        }

        // GET: FilmProductions/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null || _cache.GetAll(Name) == null)
            {
                return NotFound();
            }

            var filmProductions = _cache.GetAll(Name).FirstOrDefault(f => f.Id == id);
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
                    _cache.CreateCache(Name);
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
        public IActionResult Delete(int? id)
        {
            if (id == null || _cache.GetAll(Name) == null)
            {
                return NotFound();
            }

            var filmProductions = _cache.GetAll(Name).FirstOrDefault(f => f.Id == id);
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
            if (_cache.GetAll(Name) == null)
            {
                return Problem("Entity set 'CinemaContext.FilmProductions'  is null.");
            }
            var filmProductions = _cache.GetAll(Name).FirstOrDefault(f => f.Id == id);
            if (filmProductions != null)
            {
                _context.FilmProductions.Remove(filmProductions);
            }
            
            await _context.SaveChangesAsync();
            _cache.CreateCache(Name);
            return RedirectToAction(nameof(Index));
        }

        private bool FilmProductionsExists(int id)
        {
          return (_cache.CreateCache(Name)?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
