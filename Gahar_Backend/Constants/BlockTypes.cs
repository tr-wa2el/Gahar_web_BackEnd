namespace Gahar_Backend.Constants;

public static class BlockTypes
{
    public const string HeroSection = "HeroSection";
    public const string TextBlock = "TextBlock";
    public const string ImageGallery = "ImageGallery";
    public const string CtaButton = "CtaButton";
    public const string StatsCounter = "StatsCounter";
    public const string TeamGrid = "TeamGrid";
    public const string FaqAccordion = "FaqAccordion";
    public const string ContactForm = "ContactForm";
    public const string EmbeddedVideo = "EmbeddedVideo";
    public const string Timeline = "Timeline";
    public const string CustomHtml = "CustomHtml";
    public const string ContentList = "ContentList";
    public const string LatestNews = "LatestNews";
    public const string FeaturedContent = "FeaturedContent";

  public static readonly string[] AllBlockTypes = new[]
  {
        HeroSection, TextBlock, ImageGallery, CtaButton, StatsCounter,
        TeamGrid, FaqAccordion, ContactForm, EmbeddedVideo, Timeline,
        CustomHtml, ContentList, LatestNews, FeaturedContent
    };

    public static bool IsValid(string blockType)
    {
   return AllBlockTypes.Contains(blockType);
    }
}
