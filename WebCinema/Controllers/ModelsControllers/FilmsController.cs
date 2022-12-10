using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CinemaCore.Models;
using WebCinema.Models.IndexViewModels;
using WebCinema.Models.PageViewModels;
using WebCinema.Models.SortViewModels;
using WebCinema.Models.FilterViewModels;
using WebCinema.Enum;


namespace WebCinema.Controllers
{
    public class FilmsController : Controller
    {
        private readonly CinemaContext _context;

        public FilmsController(CinemaContext context)
        {
            _context = context;
        }

        
        // GET: Films
        public async Task<IActionResult> Index(string? filmName, int? filmAgeLimit, int? filmDuration, int page = 1, SortState sortOrder = SortState.NameAsc, int selectedGenre = 0, int selectedFilmProduction = 0)
        {
            int pageSize = 10;
            IQueryable<Films> films = _context.Films.Include(p => p.FilmProduction).Include(g => g.Genre).Include(c => c.CountryProduction);

            if(selectedGenre != 0)
            {
                films = films.Where(f => f.GenreId == selectedGenre);
            }
            if (selectedFilmProduction != 0)
            {
                films = films.Where(f => f.FilmProductionId == selectedFilmProduction);
            }

            if (!string.IsNullOrEmpty(filmName))
            {
                films = films.Where(f => f.Name!.Contains(filmName));
            }

            if (filmAgeLimit != null && filmAgeLimit > 0)
            {
                films = films.Where(f => f.AgeLimit == filmAgeLimit);
            }

            if (filmDuration != null && filmDuration > 0)
            {
                films = films.Where(f => f.Duration == filmDuration);
            }

            switch (sortOrder)
            {
                case SortState.NameDesc:
                    films = films.OrderByDescending(f => f.Name);
                    break;
                case SortState.AgeDesc:
                    films = films.OrderByDescending(f => f.AgeLimit);
                    break;
                case SortState.AgeAsc:
                    films = films.OrderBy(f => f.AgeLimit);
                    break;
                case SortState.GenreDesc:
                    films = films.OrderByDescending(f => f.Genre!.Name);
                    break;
                case SortState.GenreAsc:
                    films = films.OrderBy(f => f.Genre!.Name);
                    break;
                case SortState.FilmProductionAsc:
                    films = films.OrderBy(f => f.FilmProduction!.Name);
                    break;
                case SortState.DurationAsc:
                    films = films.OrderBy(f => f.Duration);
                    break;
                case SortState.DurationDesc:
                    films = films.OrderByDescending(f => f.Duration);
                    break;
                case SortState.FilmProductionDesc:
                    films = films.OrderByDescending(f => f.FilmProduction!.Name);
                    break;
                default:
                    films = films.OrderBy(f => f.Name);
                    break;
            }

            var count = await films.CountAsync();
            var items = await films.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            FilmsViewModel viewModel = new FilmsViewModel(
                items, 
                new PageViewModel(count, page, pageSize),
                new FilmSortViewModel(sortOrder),
                new FilmFilterViewModel(_context.Genres.ToList(), _context.FilmProductions.ToList(), filmName, filmAgeLimit, filmDuration, selectedGenre, selectedFilmProduction)
                );

            return View(viewModel);
        }

        
        // GET: Films/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Films == null)
            {
                return NotFound();
            }

            var films = await _context.Films
                .Include(f => f.CountryProduction)
                .Include(f => f.FilmProduction)
                .Include(f => f.Genre)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (films == null)
            {
                return NotFound();
            }

            return View(films);
        }

        
        // GET: Films/Create
        public IActionResult Create()
        {
            ViewData["CountryProductionId"] = new SelectList(_context.CountryProductions, "Id", "Name");
            ViewData["FilmProductionId"] = new SelectList(_context.FilmProductions, "Id", "Name");
            ViewData["GenreId"] = new SelectList(_context.Genres, "Id", "Name");
            return View();
        }


        
        // POST: Films/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,GenreId,Duration,FilmProductionId,CountryProductionId,AgeLimit,Description")] Films films)
        {
            if (ModelState.IsValid)
            {
                _context.Add(films);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CountryProductionId"] = new SelectList(_context.CountryProductions, "Id", "Name", films.CountryProductionId);
            ViewData["FilmProductionId"] = new SelectList(_context.FilmProductions, "Id", "Name", films.FilmProductionId);
            ViewData["GenreId"] = new SelectList(_context.Genres, "Id", "Name", films.GenreId);
            return View(films);
        }

        
        // GET: Films/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Films == null)
            {
                return NotFound();
            }

            var films = await _context.Films.FindAsync(id);
            if (films == null)
            {
                return NotFound();
            }
            ViewData["CountryProductionId"] = new SelectList(_context.CountryProductions, "Id", "Name", films.CountryProductionId);
            ViewData["FilmProductionId"] = new SelectList(_context.FilmProductions, "Id", "Name", films.FilmProductionId);
            ViewData["GenreId"] = new SelectList(_context.Genres, "Id", "Name", films.GenreId);
            return View(films);
        }

        
        // POST: Films/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,GenreId,Duration,FilmProductionId,CountryProductionId,AgeLimit,Description")] Films films)
        {
            if (id != films.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(films);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FilmsExists(films.Id))
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
            ViewData["CountryProductionId"] = new SelectList(_context.CountryProductions, "Id", "Name", films.CountryProductionId);
            ViewData["FilmProductionId"] = new SelectList(_context.FilmProductions, "Id", "Name", films.FilmProductionId);
            ViewData["GenreId"] = new SelectList(_context.Genres, "Id", "Name", films.GenreId);
            return View(films);
        }

        
        // GET: Films/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Films == null)
            {
                return NotFound();
            }

            var films = await _context.Films
                .Include(f => f.CountryProduction)
                .Include(f => f.FilmProduction)
                .Include(f => f.Genre)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (films == null)
            {
                return NotFound();
            }

            return View(films);
        }

        
        // POST: Films/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Films == null)
            {
                return Problem("Entity set 'CinemaContext.Films'  is null.");
            }
            var films = await _context.Films.FindAsync(id);
            if (films != null)
            {
                _context.Films.Remove(films);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FilmsExists(int id)
        {
          return (_context.Films?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
