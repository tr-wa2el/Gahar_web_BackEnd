using Microsoft.EntityFrameworkCore;
using Gahar_Backend.Data;
using Gahar_Backend.Models.Entities;
using Gahar_Backend.Repositories.Interfaces;

namespace Gahar_Backend.Repositories.Implementations;

public class FormSubmissionRepository : GenericRepository<FormSubmission>, IFormSubmissionRepository
{
    public FormSubmissionRepository(ApplicationDbContext context) : base(context) { }

    public async Task<IEnumerable<FormSubmission>> GetByFormIdAsync(int formId)
    {
     return await _context.FormSubmissions
          .Where(fs => fs.FormId == formId && !fs.IsArchived)
         .OrderByDescending(fs => fs.CreatedAt)
  .ToListAsync();
    }

    public async Task<IEnumerable<FormSubmission>> GetUnreadAsync(int formId)
    {
        return await _context.FormSubmissions
       .Where(fs => fs.FormId == formId && !fs.IsRead && !fs.IsArchived)
     .OrderByDescending(fs => fs.CreatedAt)
     .ToListAsync();
    }

   public async Task MarkAsReadAsync(int submissionId)
    {
      var submission = await _context.FormSubmissions.FindAsync(submissionId);
        if (submission != null)
   {
    submission.IsRead = true;
  submission.ReadAt = DateTime.UtcNow;
      await _context.SaveChangesAsync();
        }
    }

   public async Task ArchiveAsync(int submissionId)
    {
        var submission = await _context.FormSubmissions.FindAsync(submissionId);
        if (submission != null)
        {
     submission.IsArchived = true;
submission.ArchivedAt = DateTime.UtcNow;
           await _context.SaveChangesAsync();
   }
    }

 public IQueryable<FormSubmission> GetQueryable() => _context.FormSubmissions.AsQueryable();

   public async Task AddAsync(FormSubmission entity) => await _context.FormSubmissions.AddAsync(entity);

  public void Update(FormSubmission entity) => _context.FormSubmissions.Update(entity);

    public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
}
