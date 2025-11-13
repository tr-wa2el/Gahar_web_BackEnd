using Gahar_Backend.Models.Entities;

namespace Gahar_Backend.Repositories.Interfaces;

public interface IMediaRepository : IGenericRepository<Media>
{
    Task<IEnumerable<Media>> GetByMediaTypeAsync(string mediaType);
    Task<IEnumerable<Media>> GetByUploaderAsync(int uploaderId);
    Task<IEnumerable<Media>> SearchAsync(string searchTerm);
    IQueryable<Media> GetQueryable();
    Task AddAsync(Media entity);
    void Update(Media entity);
    Task SaveChangesAsync();
}

public interface IAlbumRepository : IGenericRepository<Album>
{
    Task<Album?> GetBySlugAsync(string slug);
   Task<bool> SlugExistsAsync(string slug, int? excludeId = null);
    Task<IEnumerable<Album>> GetPublishedAsync();
    Task<IEnumerable<Album>> GetByCreatorAsync(int creatorId);
    IQueryable<Album> GetQueryable();
    Task AddAsync(Album entity);
    void Update(Album entity);
    Task SaveChangesAsync();
}

public interface IAlbumMediaRepository : IGenericRepository<AlbumMedia>
{
    Task<IEnumerable<AlbumMedia>> GetByAlbumIdAsync(int albumId);
    Task<IEnumerable<AlbumMedia>> GetFeaturedByAlbumAsync(int albumId, int count = 5);
    IQueryable<AlbumMedia> GetQueryable();
    Task AddAsync(AlbumMedia entity);
    void Update(AlbumMedia entity);
    Task SaveChangesAsync();
}
