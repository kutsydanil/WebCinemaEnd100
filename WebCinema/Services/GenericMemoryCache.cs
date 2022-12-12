using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace WebCinema.Services
{
    public class GenericMemoryCache<T> where T: class
    {
        private IMemoryCache _cache;
        private CinemaContext _context;
        private DbSet<T> _table;

        public GenericMemoryCache(CinemaContext context, IMemoryCache memoryCache)
        {
            _cache = memoryCache;
            _context = context;
            _table = _context.Set<T>();
        }

        public IEnumerable<T> GetAll(string cacheKey)
        {
            IEnumerable <T> elements = null;
            Type type = typeof(T);
            if(type.Name == "Films")
            {
                elements = (IEnumerable<T>?)_context.Films.Include(p => p.FilmProduction).Include(g => g.Genre).Include(c => c.CountryProduction);
            }
            else
            {
                if (!_cache.TryGetValue(cacheKey, out elements))
                {
                    elements = _table.ToList();
                    if (elements != null)
                    {
                        _cache.Set(cacheKey, elements, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));
                    }
                    else
                    {
                        throw new Exception($"Проблемы в кешировании таблицы {type.Name}");
                    }
                }
            }
            
            return elements;
        }


        public IEnumerable<T> CreateCache(string cacheKey)
        {
            IEnumerable<T> elements = null;
            Type type = typeof(T);
            if (_cache.TryGetValue(cacheKey, out elements))
            {
                _cache.Remove(cacheKey);
                elements = _table.ToList();
                if(elements != null)
                {
                    _cache.Set(cacheKey, elements, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));
                }
                else
                {
                    throw new Exception($"Проблемы в кешировании таблицы {type.Name}");
                }
            }
            return elements;
        }
    }
}
