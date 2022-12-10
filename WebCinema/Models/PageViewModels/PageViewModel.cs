namespace WebCinema.Models.PageViewModels
{
    public class PageViewModel
    {
        //Номер текущей страницы
        public int PageNumber { get; }
        //Общее количество страниц
        public int TotalPages { get; }
        //Есть ли до строны 
        public bool HasPreviousPage => PageNumber > 1;
        //Есть ли после страницы
        public bool HasNextPage => PageNumber < TotalPages;

        public PageViewModel(int count, int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        }
    }
}
