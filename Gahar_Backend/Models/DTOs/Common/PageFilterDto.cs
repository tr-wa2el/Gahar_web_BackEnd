namespace Gahar_Backend.Models.DTOs.Common;

public class PageFilterDto
{
    public string? SearchTerm { get; set; }
    public bool? IsPublished { get; set; }
    public int? AuthorId { get; set; }
    public string? Template { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? SortBy { get; set; } = "CreatedAt";
    public string? SortOrder { get; set; } = "desc";
}
