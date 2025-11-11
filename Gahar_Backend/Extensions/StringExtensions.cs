namespace Gahar_Backend.Extensions
{
    public static class StringExtensions
    {
        public static string ToSlug(this string text)
        {
  if (string.IsNullOrWhiteSpace(text))
    return string.Empty;

 text = text.ToLowerInvariant();
   text = text.Replace(" ", "-");
   text = System.Text.RegularExpressions.Regex.Replace(text, @"[^a-z0-9\-]", "");
       text = System.Text.RegularExpressions.Regex.Replace(text, @"-+", "-");
text = text.Trim('-');

       return text;
 }

  public static bool IsValidEmail(this string email)
        {
            if (string.IsNullOrWhiteSpace(email))
     return false;

        try
         {
          var addr = new System.Net.Mail.MailAddress(email);
        return addr.Address == email;
     }
            catch
   {
return false;
      }
  }

public static string Truncate(this string text, int maxLength, string suffix = "...")
        {
            if (string.IsNullOrWhiteSpace(text) || text.Length <= maxLength)
  return text;

   return text.Substring(0, maxLength - suffix.Length) + suffix;
        }

  public static string ToTitleCase(this string text)
        {
if (string.IsNullOrWhiteSpace(text))
  return string.Empty;

       return System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(text.ToLower());
 }

      public static bool Contains(this string source, string value, StringComparison comparison)
  {
   return source?.IndexOf(value, comparison) >= 0;
 }

        public static string RemoveHtmlTags(this string html)
{
       if (string.IsNullOrWhiteSpace(html))
    return string.Empty;

  return System.Text.RegularExpressions.Regex.Replace(html, "<.*?>", string.Empty);
        }
    }
}
