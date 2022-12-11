
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CinemaCore.Models;
using WebCinema.Models.PageViewModels;
using WebCinema.Models.FilterViewModels;
using WebCinema.Models.SortViewModels;
using WebCinema.Models.IndexViewModels;
using WebCinema.Enum;
using Microsoft.Extensions.Caching.Memory;
using System.Runtime.CompilerServices;
using WebCinema.Services;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace WebCinema.Controllers
{
    [Authorize()]
    public class CountryProductionsController : Controller
    {
        private readonly CinemaContext _context;
        private string Name = "CountryProductions";
        private GenericMemoryCache<CountryProductions> _cache;

        public CountryProductionsController(CinemaContext context, GenericMemoryCache<CountryProductions> cache)
        {
            _context = context;
            _cache = cache;
        }

        public IActionResult Index(string? countryProductionName, int page = 1, SortState sortOrder = SortState.NameAsc)
        {
            int pageSize = 12;
            IEnumerable<CountryProductions> countryProductions = _cache.GetAll(Name);

            if (!string.IsNullOrEmpty(countryProductionName))
            {
                countryProductions = countryProductions.Where(p => p.Name!.Contains(countryProductionName));
            }

            switch (sortOrder)
            {
                case SortState.NameDesc:
                    countryProductions = countryProductions.OrderByDescending(c => c.Name);
                    break;
                default:
                    countryProductions = countryProductions.OrderBy(c => c.Name);
                    break;
            }

            var count = countryProductions.Count();
            var items = countryProductions.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            CountryProductionsViewModel viewModel = new CountryProductionsViewModel(items,
                new PageViewModel(count, page, pageSize), 
                new CountryProductionFilterViewModel(countryProductionName),
                new CountryProductionSortViewModel(sortOrder)
                );
            return View(viewModel);
        }

        // GET: CountryProductions/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null || _cache.GetAll(Name) == null)
            {
                return NotFound();
            }

            var countryProductions = _cache.GetAll(Name).FirstOrDefault(m => m.Id == id);
            if (countryProductions == null)
            {
                return NotFound();
            }

            return View(countryProductions);
        }

        
        // GET: CountryProductions/Create
        public IActionResult Create()
        {
            return View();
        }

        
        // POST: CountryProductions/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] CountryProductions countryProductions)
        {
            if (ModelState.IsValid)
            {
                _context.Add(countryProductions);
                await _context.SaveChangesAsync();
                _cache.CreateCache(Name);
                return RedirectToAction(nameof(Index));
            }
            return View(countryProductions);
        }

        
        // GET: CountryProductions/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null || _cache.GetAll(Name) == null)
            {
                return NotFound();
            }

            var countryProductions = _cache.GetAll(Name).FirstOrDefault(c => c.Id == id);
            if (countryProductions == null)
            {
                return NotFound();
            }
            return View(countryProductions);
        }

        
        // POST: CountryProductions/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] CountryProductions countryProductions)
        {
            if (id != countryProductions.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(countryProductions);
                    await _context.SaveChangesAsync();
                    _cache.CreateCache(Name);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CountryProductionsExists(countryProductions.Id))
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
            return View(countryProductions);
        }

        
        // GET: CountryProductions/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null || _cache.GetAll(Name) == null)
            {
                return NotFound();
            }

            var countryProductions = _cache.GetAll(Name).FirstOrDefault(m => m.Id == id);
            if (countryProductions == null)
            {
                return NotFound();
            }

            return View(countryProductions);
        }

        
        // POST: CountryProductions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_cache.GetAll(Name) == null)
            {
                return Problem("Entity set 'CinemaContext.CountryProductions'  is null.");
            }
            var countryProductions = _cache.GetAll(Name).FirstOrDefault(c => c.Id == id);
            if (countryProductions != null)
            {
                _context.CountryProductions.Remove(countryProductions);
            }
            
            await _context.SaveChangesAsync();
            _cache.CreateCache(Name);

            return RedirectToAction(nameof(Index));
        }

        private bool CountryProductionsExists(int id)
        {
          return (_cache.CreateCache(Name)?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
