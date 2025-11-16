namespace Gahar_Backend.Models.Entities
{
    /// <summary>
    /// Junction table for many-to-many relationship between Content and Tag
    /// </summary>
    public class ContentTag : BaseEntity
    {
        /// <summary>
        /// ID of the content
        /// </summary>
        public int ContentId { get; set; }
    
        /// <summary>
        /// Navigation property to content
        /// </summary>
        public Content Content { get; set; } = null!;

        /// <summary>
        /// ID of the tag
        /// </summary>
        public int TagId { get; set; }
        
        /// <summary>
        /// Navigation property to tag
        /// </summary>
        public Tag Tag { get; set; } = null!;
    }
}
