using Gahar_Backend.Data;
using Gahar_Backend.Models.Entities;
using Gahar_Backend.Repositories.Implementations;
using Microsoft.EntityFrameworkCore;
using FluentAssertions;
using Xunit;

namespace Gahar_Backend.Tests.Repositories
{
    public class GenericRepositoryTests : IDisposable
 {
     private readonly ApplicationDbContext _context;
   private readonly GenericRepository<Role> _repository;

        public GenericRepositoryTests()
 {
       var options = new DbContextOptionsBuilder<ApplicationDbContext>()
    .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
      .Options;

      _context = new ApplicationDbContext(options);
  _repository = new GenericRepository<Role>(_context);

 SeedData();
        }

   private void SeedData()
  {
       _context.Roles.AddRange(
      new Role { Id = 1, Name = "Admin", DisplayName = "Administrator" },
    new Role { Id = 2, Name = "Editor", DisplayName = "Content Editor" },
    new Role { Id = 3, Name = "Viewer", DisplayName = "Viewer" }
   );
     _context.SaveChanges();
        }

     [Fact]
public async Task GetByIdAsync_WithValidId_ShouldReturnEntity()
 {
  // Act
     var role = await _repository.GetByIdAsync(1);

   // Assert
  role.Should().NotBeNull();
     role!.Name.Should().Be("Admin");
   }

   [Fact]
 public async Task GetByIdAsync_WithInvalidId_ShouldReturnNull()
   {
   // Act
 var role = await _repository.GetByIdAsync(999);

// Assert
   role.Should().BeNull();
 }

[Fact]
 public async Task GetAllAsync_ShouldReturnAllEntities()
 {
       // Act
var roles = await _repository.GetAllAsync();

     // Assert
       roles.Should().HaveCount(3);
}

     [Fact]
public async Task CreateAsync_ShouldCreateEntity()
     {
 // Arrange
       var newRole = new Role
   {
Name = "Manager",
       DisplayName = "Manager Role"
  };

       // Act
     var createdRole = await _repository.CreateAsync(newRole);

       // Assert
  createdRole.Should().NotBeNull();
    createdRole.Id.Should().BeGreaterThan(0);
       createdRole.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
       
    var allRoles = await _repository.GetAllAsync();
       allRoles.Should().HaveCount(4);
}

        [Fact]
 public async Task UpdateAsync_ShouldUpdateEntity()
     {
    // Arrange
var role = await _repository.GetByIdAsync(1);
   role!.DisplayName = "Super Administrator";

     // Act
   var updatedRole = await _repository.UpdateAsync(role);

       // Assert
       updatedRole.DisplayName.Should().Be("Super Administrator");
  updatedRole.UpdatedAt.Should().NotBeNull();
  updatedRole.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        }

     [Fact]
  public async Task DeleteAsync_WithValidId_ShouldSoftDeleteEntity()
 {
       // Act
     var result = await _repository.DeleteAsync(1);

       // Assert
  result.Should().BeTrue();
       
   var role = await _context.Roles.IgnoreQueryFilters().FirstOrDefaultAsync(r => r.Id == 1);
 role.Should().NotBeNull();
     role!.IsDeleted.Should().BeTrue();
   role.DeletedAt.Should().NotBeNull();
        }

 [Fact]
  public async Task DeleteAsync_WithInvalidId_ShouldReturnFalse()
   {
   // Act
  var result = await _repository.DeleteAsync(999);

  // Assert
       result.Should().BeFalse();
        }

 [Fact]
        public async Task ExistsAsync_WithExistingId_ShouldReturnTrue()
{
// Act
       var exists = await _repository.ExistsAsync(1);

// Assert
       exists.Should().BeTrue();
  }

 [Fact]
   public async Task ExistsAsync_WithNonExistingId_ShouldReturnFalse()
     {
  // Act
     var exists = await _repository.ExistsAsync(999);

   // Assert
   exists.Should().BeFalse();
        }

        [Fact]
        public async Task CreateAsync_ShouldSetCreatedAtAutomatically()
   {
    // Arrange
  var newRole = new Role
       {
    Name = "TestRole",
  DisplayName = "Test Role"
       };

// Act
       var createdRole = await _repository.CreateAsync(newRole);

  // Assert
       createdRole.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
     createdRole.UpdatedAt.Should().BeNull();
        }

  [Fact]
   public async Task UpdateAsync_ShouldSetUpdatedAtAutomatically()
        {
 // Arrange
       var role = await _repository.GetByIdAsync(1);
   var originalCreatedAt = role!.CreatedAt;
await Task.Delay(100);
       role.DisplayName = "Modified";

       // Act
   var updatedRole = await _repository.UpdateAsync(role);

   // Assert
  updatedRole.UpdatedAt.Should().NotBeNull();
 updatedRole.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
updatedRole.CreatedAt.Should().Be(originalCreatedAt);
 }

        public void Dispose()
  {
  _context.Database.EnsureDeleted();
       _context.Dispose();
 }
    }
}
