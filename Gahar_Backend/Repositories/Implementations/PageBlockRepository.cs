using Microsoft.EntityFrameworkCore;
using Gahar_Backend.Data;
using Gahar_Backend.Models.Entities;
using Gahar_Backend.Repositories.Interfaces;

namespace Gahar_Backend.Repositories.Implementations;

public class PageBlockRepository : GenericRepository<PageBlock>, IPageBlockRepository
{
    public PageBlockRepository(ApplicationDbContext context) : base(context)
    {
  }

    public async Task<IEnumerable<PageBlock>> GetByPageIdAsync(int pageId)
    {
        return await _context.PageBlocks
.Where(b => b.PageId == pageId)
    .OrderBy(b => b.DisplayOrder)
  .ToListAsync();
    }

    public async Task ReorderBlocksAsync(int pageId, List<int> blockIds)
    {
  var blocks = await _context.PageBlocks
 .Where(b => b.PageId == pageId && blockIds.Contains(b.Id))
    .ToListAsync();

        for (int i = 0; i < blockIds.Count; i++)
    {
       var block = blocks.FirstOrDefault(b => b.Id == blockIds[i]);
    if (block != null)
   {
     block.DisplayOrder = i + 1;
          }
        }

        await _context.SaveChangesAsync();
    }
}
