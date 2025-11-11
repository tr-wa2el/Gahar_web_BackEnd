using Gahar_Backend.Data;
using Gahar_Backend.Extensions;
using Gahar_Backend.Models.Entities;
using Microsoft.EntityFrameworkCore;
using FluentAssertions;
using Xunit;

namespace Gahar_Backend.Tests.Extensions
{
    public class QueryableExtensionsTests : IDisposable
{
  private readonly ApplicationDbContext _context;

 public QueryableExtensionsTests()
{
   var options = new DbContextOptionsBuilder<ApplicationDbContext>()
.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
  .Options;

 _context = new ApplicationDbContext(options);
SeedData();
 }

    private void SeedData()
{
var roles = new List<Role>();
for (int i = 1; i <= 50; i++)
   {
   roles.Add(new Role
 {
        Id = i,
     Name = $"Role{i}",
    DisplayName = $"Role {i}",
   Description = i % 2 == 0 ? "Even Role" : "Odd Role"
  });
            }

 _context.Roles.AddRange(roles);
     _context.SaveChanges();
        }

   [Fact]
        public void Paginate_ShouldReturnCorrectPage()
    {
  // Arrange
  var query = _context.Roles.AsQueryable();

// Act
  var result = query.Paginate(2, 10).ToList();

   // Assert
     result.Should().HaveCount(10);
    result.First().Id.Should().Be(11);
   result.Last().Id.Should().Be(20);
   }

   [Fact]
  public void Paginate_WithPageOne_ShouldReturnFirstItems()
  {
// Arrange
  var query = _context.Roles.AsQueryable();

   // Act
  var result = query.Paginate(1, 5).ToList();

 // Assert
  result.Should().HaveCount(5);
  result.First().Id.Should().Be(1);
result.Last().Id.Should().Be(5);
        }

        [Fact]
   public async Task ToPaginatedListAsync_ShouldReturnCorrectResult()
   {
   // Arrange
  var query = _context.Roles.AsQueryable();

   // Act
     var result = await query.ToPaginatedListAsync(2, 10);

// Assert
     result.Items.Should().HaveCount(10);
  result.TotalCount.Should().Be(50);
     result.PageNumber.Should().Be(2);
   result.PageSize.Should().Be(10);
       result.TotalPages.Should().Be(5);
     result.HasPreviousPage.Should().BeTrue();
    result.HasNextPage.Should().BeTrue();
        }

 [Fact]
  public async Task ToPaginatedListAsync_WithFirstPage_ShouldHaveNoreviousPage()
{
 // Arrange
var query = _context.Roles.AsQueryable();

     // Act
   var result = await query.ToPaginatedListAsync(1, 10);

   // Assert
      result.HasPreviousPage.Should().BeFalse();
   result.HasNextPage.Should().BeTrue();
 }

[Fact]
        public async Task ToPaginatedListAsync_WithLastPage_ShouldHaveNoNextPage()
        {
            // Arrange
   var query = _context.Roles.AsQueryable();

 // Act
     var result = await query.ToPaginatedListAsync(5, 10);

       // Assert
    result.HasPreviousPage.Should().BeTrue();
  result.HasNextPage.Should().BeFalse();
        }

[Fact]
    public async Task ToPaginatedListAsync_WithLargePageSize_ShouldReturnAllItems()
{
  // Arrange
      var query = _context.Roles.AsQueryable();

       // Act
   var result = await query.ToPaginatedListAsync(1, 100);

// Assert
result.Items.Should().HaveCount(50);
     result.TotalPages.Should().Be(1);
     result.HasPreviousPage.Should().BeFalse();
result.HasNextPage.Should().BeFalse();
        }

   [Fact]
        public void WhereIf_WithTrueCondition_ShouldApplyPredicate()
{
   // Arrange
  var query = _context.Roles.AsQueryable();
   var searchTerm = "Role1";

 // Act
   var result = query
  .WhereIf(searchTerm != null, r => r.Name.Contains(searchTerm))
    .ToList();

       // Assert
     result.Should().NotBeEmpty();
 result.All(r => r.Name.Contains("Role1")).Should().BeTrue();
        }

        [Fact]
 public void WhereIf_WithFalseCondition_ShouldNotApplyPredicate()
        {
// Arrange
  var query = _context.Roles.AsQueryable();

// Act
  var result = query
                .WhereIf(false, r => r.Name == "NonExistent")
    .ToList();

 // Assert
      result.Should().HaveCount(50); // All items returned
     }

 [Fact]
     public void WhereIf_ChainedMultipleTimes_ShouldApplyAllConditions()
   {
// Arrange
  var query = _context.Roles.AsQueryable();
   var condition1 = true;
  var condition2 = true;

// Act
var result = query
 .WhereIf(condition1, r => r.Description!.Contains("Even"))
     .WhereIf(condition2, r => r.Id <= 20)
  .ToList();

// Assert
  result.Should().NotBeEmpty();
          result.All(r => r.Description == "Even Role" && r.Id <= 20).Should().BeTrue();
 }

  [Fact]
public async Task ToPaginatedListAsync_WithFiltering_ShouldReturnFilteredResults()
        {
// Arrange
  var query = _context.Roles
      .Where(r => r.Description == "Even Role");

  // Act
  var result = await query.ToPaginatedListAsync(1, 10);

 // Assert
  result.TotalCount.Should().Be(25); // 25 even roles
    result.Items.Should().HaveCount(10);
  result.TotalPages.Should().Be(3);
        }

    [Theory]
  [InlineData(1, 10, 10)]
   [InlineData(2, 10, 10)]
  [InlineData(3, 10, 10)]
        [InlineData(4, 10, 10)]
[InlineData(5, 10, 10)]
        [InlineData(6, 10, 0)] // Beyond available data
        public async Task ToPaginatedListAsync_WithDifferentPages_ShouldReturnCorrectCounts(int pageNumber, int pageSize, int expectedCount)
        {
  // Arrange
var query = _context.Roles.AsQueryable();

   // Act
  var result = await query.ToPaginatedListAsync(pageNumber, pageSize);

   // Assert
 result.Items.Should().HaveCount(expectedCount);
        }

  public void Dispose()
        {
      _context.Database.EnsureDeleted();
       _context.Dispose();
        }
    }
}
