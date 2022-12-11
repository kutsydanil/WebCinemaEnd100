using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CinemaCore.Models;
using WebCinema.Enum;
using WebCinema.Models.FilterViewModels;
using WebCinema.Models.IndexViewModels;
using WebCinema.Models.PageViewModels;
using WebCinema.Models.SortViewModels;
using Microsoft.AspNetCore.Authorization;
using WebCinema.Services;

namespace WebCinema.Controllers
{
    
    public class GenresController : Controller
    {
        private readonly CinemaContext _context;
        private string Name = "Genres";
        private GenericMemoryCache<Genres> _cache;

        public GenresController(CinemaContext context, GenericMemoryCache<Genres> cache)
        {
            _context = context;
            _cache = cache;
        }

        public IActionResult Index(string? genreName, int page = 1, SortState sortOrder = SortState.NameAsc)
        {
            int pageSize = 12;

            IEnumerable<Genres> genres = _cache.GetAll(Name);

            if (!string.IsNullOrEmpty(genreName))
            {
                genres = genres.Where(p => p.Name!.Contains(genreName));
            }

            switch (sortOrder)
            {
                case SortState.NameDesc:
                    genres = genres.OrderByDescending(c => c.Name);
                    break;
                default:
                    genres = genres.OrderBy(c => c.Name);
                    break;
            }

            var count = genres.Count();
            var items = genres.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            GenreViewModel viewModel = new GenreViewModel(items,
                new PageViewModel(count, page, pageSize),
                new GenreFilterViewModel(genreName),
                new GenreSortViewModelcs(sortOrder));
            return View(viewModel);

        }

        public IActionResult Details(int? id)
        {
            if (id == null || _cache.GetAll(Name) == null)
            {
                return NotFound();
            }

            var genres = _cache.GetAll(Name).FirstOrDefault(g => g.Id == id);
            if (genres == null)
            {
                return NotFound();
            }

            return View(genres);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Genres genres)
        {
            if (ModelState.IsValid)
            {
                _context.Add(genres);
                await _context.SaveChangesAsync();
                _cache.CreateCache(Name);
                return RedirectToAction(nameof(Index));
            }
            return View(genres);
        }

        public IActionResult Edit(int? id)
        {
            if (id == null || _cache.GetAll(Name) == null)
            {
                return NotFound();
            }

            var genres = _cache.GetAll(Name).FirstOrDefault(g => g.Id == id);
            if (genres == null)
            {
                return NotFound();
            }
            return View(genres);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Genres genres)
        {
            if (id != genres.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(genres);
                    await _context.SaveChangesAsync();
                    _cache.CreateCache(Name);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GenresExists(genres.Id))
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
            return View(genres);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null || _cache.GetAll(Name) == null)
            {
                return NotFound();
            }

            var genres = _cache.GetAll(Name).FirstOrDefault(g => g.Id == id);
            if (genres == null)
            {
                return NotFound();
            }

            return View(genres);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Genres == null)
            {
                return Problem("Entity set 'CinemaContext.Genres'  is null.");
            }
            var genres = _cache.GetAll(Name).FirstOrDefault(g => g.Id == id);
            if (genres != null)
            {
                _context.Genres.Remove(genres);
            }
            
            await _context.SaveChangesAsync();
            _cache.CreateCache(Name);
            return RedirectToAction(nameof(Index));
        }

        private bool GenresExists(int id)
        {
            return (_cache.CreateCache(Name)?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
