using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CinemaCore.Models;
using WebCinema.Models.IndexViewModels;
using WebCinema.Models.PageViewModels;
using WebCinema.Models.SortViewModels;
using WebCinema.Models.FilterViewModels;
using WebCinema.Enum;
using WebCinema.Services;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;

namespace WebCinema.Controllers
{
    [Authorize()]
    public class FilmsController : Controller
    {
        private readonly CinemaContext _context;
        private string Name = "Films";
        private string countryName = "CountryProductions";
        private string productionName = "FilmProductions";
        private string genreName = "Genres";

        private GenericMemoryCache<Films> _cache;
        private GenericMemoryCache<FilmProductions> _productionscache;
        private GenericMemoryCache<Genres> _genrecache;
        private GenericMemoryCache<CountryProductions> _countrycache;


        public FilmsController(CinemaContext context, GenericMemoryCache<Films> cache, GenericMemoryCache<FilmProductions> productionscache, GenericMemoryCache<Genres> genrecache,
            GenericMemoryCache<CountryProductions> countrycache)
        {
            _context = context;
            _cache = cache;
            _productionscache = productionscache;
            _countrycache = countrycache;
            _genrecache = genrecache;
        }
        
        // GET: Films
        public async Task<IActionResult> Index(string? filmName, int? filmAgeLimit, int? filmDuration, int page = 1, SortState sortOrder = SortState.NameAsc, int selectedGenre = 0, int selectedFilmProduction = 0)
        {
            int pageSize = 10;

            IEnumerable<Films> films = _cache.GetAll(Name);

            if (selectedGenre != 0)
            {
                films = films.Where(f => f.GenreId == selectedGenre);
                Response.Cookies.Append("SelectedGenre", selectedGenre.ToString());
            }
            else
            {
                if (Request.Cookies.ContainsKey("SelectedGenre"))
                {
                    selectedGenre = Convert.ToInt32(Request.Cookies["SelectedGenre"]);
                    Response.Cookies.Delete("SelectedGenre");
                }
            }

            if (selectedFilmProduction != 0)
            {
                films = films.Where(f => f.FilmProductionId == selectedFilmProduction);
                Response.Cookies.Append("SelectedFilmProduction", selectedFilmProduction.ToString());
            }
            else
            {
                if (Request.Cookies.ContainsKey("SelectedFilmProduction"))
                {
                    selectedFilmProduction = Convert.ToInt32(Request.Cookies["SelectedFilmProduction"]);
                    Response.Cookies.Delete("SelectedFilmProduction");
                }
            }

            if (!string.IsNullOrEmpty(filmName))
            {
                films = films.Where(f => f.Name!.Contains(filmName));
                Response.Cookies.Append(Name, filmName);
            }
            else
            {
                if (Request.Cookies.ContainsKey(Name))
                {
                    filmName = Request.Cookies[Name];
                    Response.Cookies.Delete(Name);
                }
            }

            if (filmAgeLimit != null && filmAgeLimit > 0)
            {
                films = films.Where(f => f.AgeLimit == filmAgeLimit);
                Response.Cookies.Append("SeletedAgeLimit", filmAgeLimit.ToString());
            }
            else
            {
                if (Request.Cookies.ContainsKey("SeletedAgeLimit"))
                {
                    filmAgeLimit = Convert.ToInt32(Request.Cookies["SeletedAgeLimit"]);
                    Response.Cookies.Delete("SeletedAgeLimit");
                }
            }

            if (filmDuration != null && filmDuration > 0)
            {
                films = films.Where(f => f.Duration == filmDuration);
                Response.Cookies.Append("SeletedDuration", filmDuration.ToString());
            }
            else
            {
                if (Request.Cookies.ContainsKey("SeletedDuration"))
                {
                    filmDuration = Convert.ToInt32(Request.Cookies["SeletedDuration"]);
                    Response.Cookies.Delete("SeletedDuration");
                }
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

            var count = films.Count();
            var items = films.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            FilmsViewModel viewModel = new FilmsViewModel(
                items, 
                new PageViewModel(count, page, pageSize),
                new FilmSortViewModel(sortOrder),
                new FilmFilterViewModel(_context.Genres.ToList(), _context.FilmProductions.ToList(), filmName, filmAgeLimit, filmDuration, selectedGenre, selectedFilmProduction)
                );

            return View(viewModel);
        }

        
        // GET: Films/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null || _cache.GetAll(Name) == null)
            {
                return NotFound();
            }

            var films = _cache.GetAll(Name).FirstOrDefault(f => f.Id == id);
            if (films == null)
            {
                return NotFound();
            }

            return View(films);
        }

        
        // GET: Films/Create
        public IActionResult Create()
        {
            ViewData["CountryProductionId"] = new SelectList(_countrycache.GetAll(countryName), "Id", "Name");
            ViewData["FilmProductionId"] = new SelectList(_productionscache.GetAll(productionName), "Id", "Name");
            ViewData["GenreId"] = new SelectList(_genrecache.GetAll(genreName), "Id", "Name");
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
                _cache.CreateCache(Name);
                return RedirectToAction(nameof(Index));
            }
            ViewData["CountryProductionId"] = new SelectList(_countrycache.GetAll(countryName), "Id", "Name", films.CountryProductionId);
            ViewData["FilmProductionId"] = new SelectList(_productionscache.GetAll(productionName), "Id", "Name", films.FilmProductionId);
            ViewData["GenreId"] = new SelectList(_genrecache.GetAll(genreName), "Id", "Name", films.GenreId);
            return View(films);
        }

        
        // GET: Films/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null || _cache.GetAll(Name) == null)
            {
                return NotFound();
            }

            var films = _cache.GetAll(Name).FirstOrDefault(f => f.Id == id);
            if (films == null)
            {
                return NotFound();
            }
            ViewData["CountryProductionId"] = new SelectList(_countrycache.GetAll(countryName), "Id", "Name", films.CountryProductionId);
            ViewData["FilmProductionId"] = new SelectList(_productionscache.GetAll(productionName), "Id", "Name", films.FilmProductionId);
            ViewData["GenreId"] = new SelectList(_genrecache.GetAll(genreName), "Id", "Name", films.GenreId);
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
                    _cache.CreateCache(Name);
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
            ViewData["CountryProductionId"] = new SelectList(_countrycache.GetAll(countryName), "Id", "Name", films.CountryProductionId);
            ViewData["FilmProductionId"] = new SelectList(_productionscache.GetAll(productionName), "Id", "Name", films.FilmProductionId);
            ViewData["GenreId"] = new SelectList(_genrecache.GetAll(genreName), "Id", "Name", films.GenreId);
            return View(films);
        }

        
        // GET: Films/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null || _cache.GetAll(Name) == null)
            {
                return NotFound();
            }

            var films = _cache.GetAll(Name).FirstOrDefault(f => f.Id == id);

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
            if (_cache.GetAll(Name) == null)
            {
                return Problem("Entity set 'CinemaContext.Films'  is null.");
            }
            var films = _cache.GetAll(Name).FirstOrDefault(f => f.Id == id);
            if (films != null)
            {
                _context.Films.Remove(films);
            }
            await _context.SaveChangesAsync();
            _cache.CreateCache(Name);

            return RedirectToAction(nameof(Index));
        }

        private bool FilmsExists(int id)
        {
            return (_cache.CreateCache(Name)?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
