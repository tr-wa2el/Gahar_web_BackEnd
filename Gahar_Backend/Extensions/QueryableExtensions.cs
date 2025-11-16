using Microsoft.EntityFrameworkCore;

namespace Gahar_Backend.Extensions
{
    public static class QueryableExtensions
 {
        public static IQueryable<T> Paginate<T>(this IQueryable<T> query, int pageNumber, int pageSize)
        {
      return query.Skip((pageNumber - 1) * pageSize).Take(pageSize);
 }

      public static async Task<PaginatedResult<T>> ToPaginatedListAsync<T>(
   this IQueryable<T> query,
      int pageNumber,
     int pageSize,
   CancellationToken cancellationToken = default)
 {
     var totalCount = await query.CountAsync(cancellationToken);
   var items = await query.Paginate(pageNumber, pageSize).ToListAsync(cancellationToken);

  return new PaginatedResult<T>
      {
        Items = items,
     TotalCount = totalCount,
 PageNumber = pageNumber,
     PageSize = pageSize,
    TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
   };
 }

      public static IQueryable<T> WhereIf<T>(
   this IQueryable<T> query,
            bool condition,
          System.Linq.Expressions.Expression<Func<T, bool>> predicate)
{
   return condition ? query.Where(predicate) : query;
        }
    }

    public class PaginatedResult<T>
 {
 public List<T> Items { get; set; } = new();
   public int TotalCount { get; set; }
   public int PageNumber { get; set; }
        public int PageSize { get; set; }
public int TotalPages { get; set; }
        public bool HasPreviousPage => PageNumber > 1;
  public bool HasNextPage => PageNumber < TotalPages;
    }
}
