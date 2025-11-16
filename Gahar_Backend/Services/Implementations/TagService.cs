using Gahar_Backend.Models.DTOs.Content;
using Gahar_Backend.Models.Entities;
using Gahar_Backend.Repositories.Interfaces;
using Gahar_Backend.Services.Interfaces;
using Gahar_Backend.Utilities.Exceptions;
using Gahar_Backend.Extensions;

namespace Gahar_Backend.Services.Implementations
{
    public class TagService : ITagService
 {
  private readonly ITagRepository _repository;
   private readonly IAuditLogService _auditLogService;

      public TagService(
    ITagRepository repository,
  IAuditLogService auditLogService)
{
       _repository = repository;
     _auditLogService = auditLogService;
     }

        public async Task<IEnumerable<TagDto>> GetAllAsync()
 {
var tags = await _repository.GetAllAsync();
   return tags.Select(t => MapToDto(t));
   }

   public async Task<TagDto> GetByIdAsync(int id)
 {
     var tag = await _repository.GetByIdAsync(id);
   
     if (tag == null)
  throw new NotFoundException($"Tag with id {id} not found");

       return MapToDto(tag);
        }

    public async Task<TagDto> GetBySlugAsync(string slug)
   {
      var tag = await _repository.GetBySlugAsync(slug);
        
  if (tag == null)
   throw new NotFoundException($"Tag with slug '{slug}' not found");

   return MapToDto(tag);
   }

public async Task<TagDto> CreateAsync(CreateTagDto dto)
   {
    // Validate slug uniqueness
   if (await _repository.SlugExistsAsync(dto.Slug))
   throw new BadRequestException($"Tag with slug '{dto.Slug}' already exists");

var tag = new Tag
  {
      Name = dto.Name,
   Slug = dto.Slug.ToSlug(),
    Description = dto.Description,
      Color = dto.Color
   };

    await _repository.CreateAsync(tag);

  await _auditLogService.LogAsync(
      userId: null,
       action: "Create",
   entityType: "Tag",
    entityId: tag.Id,
     description: $"Created tag: {tag.Name}"
    );

         return MapToDto(tag);
   }

public async Task<TagDto> UpdateAsync(int id, UpdateTagDto dto)
   {
    var tag = await _repository.GetByIdAsync(id);
      
     if (tag == null)
    throw new NotFoundException($"Tag with id {id} not found");

   // Validate slug uniqueness
      if (await _repository.SlugExistsAsync(dto.Slug, id))
throw new BadRequestException($"Tag with slug '{dto.Slug}' already exists");

    tag.Name = dto.Name;
   tag.Slug = dto.Slug.ToSlug();
tag.Description = dto.Description;
   tag.Color = dto.Color;

 await _repository.UpdateAsync(tag);

await _auditLogService.LogAsync(
    userId: null,
       action: "Update",
    entityType: "Tag",
      entityId: tag.Id,
      description: $"Updated tag: {tag.Name}"
   );

       return MapToDto(tag);
 }

    public async Task DeleteAsync(int id)
   {
 var tag = await _repository.GetByIdAsync(id);
   
  if (tag == null)
    throw new NotFoundException($"Tag with id {id} not found");

   await _repository.DeleteAsync(id);

    await _auditLogService.LogAsync(
     userId: null,
       action: "Delete",
 entityType: "Tag",
   entityId: id,
   description: $"Deleted tag: {tag.Name}"
    );
  }

        public async Task<IEnumerable<TagDto>> GetPopularAsync(int count)
     {
var tags = await _repository.GetPopularTagsAsync(count);
  return tags.Select(t => MapToDto(t));
   }

   public async Task<IEnumerable<TagDto>> SearchAsync(string searchTerm)
   {
     var tags = await _repository.SearchAsync(searchTerm);
   return tags.Select(t => MapToDto(t));
        }

     private TagDto MapToDto(Tag tag)
{
   return new TagDto
   {
  Id = tag.Id,
      Name = tag.Name,
       Slug = tag.Slug,
  Description = tag.Description,
 Color = tag.Color,
       UsageCount = tag.UsageCount
     };
}
    }
}
