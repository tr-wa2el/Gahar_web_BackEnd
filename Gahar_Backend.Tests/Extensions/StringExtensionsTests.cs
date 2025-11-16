using Gahar_Backend.Extensions;
using FluentAssertions;
using Xunit;

namespace Gahar_Backend.Tests.Extensions
{
    public class StringExtensionsTests
    {
        [Theory]
  [InlineData("Hello World", "hello-world")]
  [InlineData("Hello   World", "hello-world")]
  [InlineData("Hello-World", "hello-world")]
        [InlineData("Hello_World", "helloworld")]
  [InlineData("Hello@World!", "helloworld")]
        [InlineData("مرحبا بك", "")]
        public void ToSlug_ShouldConvertTextToSlug(string input, string expected)
   {
       // Act
   var result = input.ToSlug();

       // Assert
result.Should().Be(expected);
        }

 [Fact]
        public void ToSlug_WithNullOrEmpty_ShouldReturnEmptyString()
 {
// Act & Assert
   string.Empty.ToSlug().Should().BeEmpty();
   ((string)null!).ToSlug().Should().BeEmpty();
  }

      [Theory]
        [InlineData("test@example.com", true)]
   [InlineData("user.name@example.co.uk", true)]
  [InlineData("invalid.email", false)]
   [InlineData("@example.com", false)]
[InlineData("test@", false)]
        [InlineData("", false)]
   public void IsValidEmail_ShouldValidateEmailCorrectly(string email, bool expected)
        {
  // Act
    var result = email.IsValidEmail();

// Assert
       result.Should().Be(expected);
    }

[Theory]
        [InlineData("Hello World", 5, "...", "He...")]
   [InlineData("Hello World", 20, "...", "Hello World")]
        [InlineData("Hello World", 8, "...", "Hello...")]
        [InlineData("Test", 10, "...", "Test")]
        public void Truncate_ShouldTruncateTextCorrectly(string input, int maxLength, string suffix, string expected)
   {
      // Act
      var result = input.Truncate(maxLength, suffix);

 // Assert
          result.Should().Be(expected);
 }

        [Fact]
public void Truncate_WithNullOrEmpty_ShouldReturnOriginal()
     {
// Act & Assert
    string.Empty.Truncate(10).Should().BeEmpty();
 ((string)null!).Truncate(10).Should().BeNull();
        }

        [Theory]
  [InlineData("hello world", "Hello World")]
  [InlineData("HELLO WORLD", "Hello World")]
        [InlineData("hElLo WoRlD", "Hello World")]
        public void ToTitleCase_ShouldConvertToTitleCase(string input, string expected)
{
// Act
  var result = input.ToTitleCase();

     // Assert
   result.Should().Be(expected);
        }

[Fact]
 public void ToTitleCase_WithNullOrEmpty_ShouldReturnEmptyString()
  {
   // Act & Assert
  string.Empty.ToTitleCase().Should().BeEmpty();
   ((string)null!).ToTitleCase().Should().BeEmpty();
        }

     [Theory]
 [InlineData("Hello World", "World", StringComparison.Ordinal, true)]
     [InlineData("Hello World", "world", StringComparison.OrdinalIgnoreCase, true)]
  [InlineData("Hello World", "world", StringComparison.Ordinal, false)]
  [InlineData("Hello World", "Test", StringComparison.Ordinal, false)]
        public void Contains_WithComparison_ShouldCheckCorrectly(string source, string value, StringComparison comparison, bool expected)
   {
 // Act
   var result = source.Contains(value, comparison);

       // Assert
result.Should().Be(expected);
        }

  [Theory]
        [InlineData("<p>Hello World</p>", "Hello World")]
  [InlineData("<div><span>Test</span></div>", "Test")]
        [InlineData("<b>Bold</b> <i>Italic</i>", "Bold Italic")]
  [InlineData("No HTML tags", "No HTML tags")]
        [InlineData("", "")]
        public void RemoveHtmlTags_ShouldRemoveAllHtmlTags(string input, string expected)
  {
     // Act
  var result = input.RemoveHtmlTags();

    // Assert
    result.Should().Be(expected);
 }

 [Fact]
  public void RemoveHtmlTags_WithNullOrEmpty_ShouldReturnEmptyString()
  {
   // Act & Assert
       string.Empty.RemoveHtmlTags().Should().BeEmpty();
 ((string)null!).RemoveHtmlTags().Should().BeEmpty();
        }
    }
}
