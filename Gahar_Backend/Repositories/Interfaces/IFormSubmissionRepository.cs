using Gahar_Backend.Models.Entities;
using Gahar_Backend.Repositories.Interfaces;

namespace Gahar_Backend.Repositories.Interfaces;

public interface IFormSubmissionRepository : IGenericRepository<FormSubmission>
{
    Task<IEnumerable<FormSubmission>> GetByFormIdAsync(int formId);
    Task<IEnumerable<FormSubmission>> GetUnreadAsync(int formId);
    Task MarkAsReadAsync(int submissionId);
    Task ArchiveAsync(int submissionId);
    IQueryable<FormSubmission> GetQueryable();
    Task AddAsync(FormSubmission entity);
    void Update(FormSubmission entity);
    Task SaveChangesAsync();
}
