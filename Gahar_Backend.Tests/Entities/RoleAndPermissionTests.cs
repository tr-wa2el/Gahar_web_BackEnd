using Gahar_Backend.Models.Entities;
using FluentAssertions;
using Xunit;

namespace Gahar_Backend.Tests.Entities
{
    public class RoleTests
  {
        [Fact]
      public void Role_ShouldInitializeWithDefaultValues()
        {
            // Arrange & Act
      var role = new Role();

       // Assert
         role.Name.Should().NotBeNull();
            role.DisplayName.Should().NotBeNull();
            role.IsSystemRole.Should().BeFalse();
         role.UserRoles.Should().NotBeNull().And.BeEmpty();
            role.RolePermissions.Should().NotBeNull().And.BeEmpty();
     role.IsDeleted.Should().BeFalse();
        }

        [Fact]
      public void Role_ShouldSetProperties_Correctly()
        {
     // Arrange
   var role = new Role
            {
           Name = "Admin",
           DisplayName = "Administrator",
         Description = "System Administrator",
        IsSystemRole = true
            };

   // Assert
     role.Name.Should().Be("Admin");
  role.DisplayName.Should().Be("Administrator");
  role.Description.Should().Be("System Administrator");
   role.IsSystemRole.Should().BeTrue();
}
    }

public class PermissionTests
    {
        [Fact]
        public void Permission_ShouldInitializeWithDefaultValues()
      {
     // Arrange & Act
            var permission = new Permission();

   // Assert
            permission.Name.Should().NotBeNull();
            permission.Module.Should().NotBeNull();
    permission.RolePermissions.Should().NotBeNull().And.BeEmpty();
        }

     [Fact]
        public void Permission_ShouldSetProperties_Correctly()
        {
            // Arrange
   var permission = new Permission
    {
                Name = "Users.Create",
 Module = "Users",
         Description = "Create new users"
      };

     // Assert
       permission.Name.Should().Be("Users.Create");
            permission.Module.Should().Be("Users");
         permission.Description.Should().Be("Create new users");
        }
    }
}
