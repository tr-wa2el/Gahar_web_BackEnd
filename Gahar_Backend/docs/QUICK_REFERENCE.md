# ? ???? ???? - ???? ????

**?????:** ???? ???? ???????? ?????? ??? ?????? ?????

---

## ?? ???? ?????????

1. [??????? ????????](#???????-????????)
2. [Project Structure](#project-structure)
3. [Database Commands](#database-commands)
4. [Git Workflow](#git-workflow)
5. [API Endpoints Summary](#api-endpoints-summary)
6. [Common Code Snippets](#common-code-snippets)
7. [Troubleshooting](#troubleshooting)
8. [Useful Links](#useful-links)

---

## 1. ??????? ????????

### .NET Commands

```bash
# Build Solution
dotnet build

# Run API
cd GAHAR.API
dotnet run

# Run with watch (auto-reload)
dotnet watch run

# Run Tests
dotnet test

# Run Tests with Coverage
dotnet test --collect:"XPlat Code Coverage"

# Restore Packages
dotnet restore

# Clean Build
dotnet clean

# Publish for Production
dotnet publish -c Release -o ./publish

# Check for outdated packages
dotnet list package --outdated

# Update package
dotnet add package PackageName --version X.X.X
```

### Docker Commands

```bash
# Start all services
docker-compose up -d

# Stop all services
docker-compose down

# View logs
docker-compose logs -f

# Rebuild and start
docker-compose up -d --build

# Stop specific service
docker-compose stop sqlserver

# Remove all containers and volumes
docker-compose down -v

# Check running containers
docker ps

# Execute command in container
docker exec -it gahar-api bash
```

---

## 2. Project Structure

```
GAHAR.CMS/
??? GAHAR.API/      # Web API Layer
?   ??? Controllers/      # API Controllers
?   ??? Middleware/             # Custom Middleware
?   ??? Filters/             # Action Filters
?   ??? Program.cs    # App Configuration
?
??? GAHAR.Core/           # Domain Layer
?   ??? Entities/     # Domain Models
?   ??? Interfaces/        # Repository Interfaces
?   ??? Services/    # Business Logic
?   ??? Specifications/         # Query Specifications
?   ??? Exceptions/    # Custom Exceptions
?
??? GAHAR.Infrastructure/       # Data Access Layer
?   ??? Data/
?   ?   ??? AppDbContext.cs     # EF DbContext
?   ?   ??? Configurations/     # EF Configurations
?   ?   ??? Migrations/         # Database Migrations
?   ??? Repositories/           # Repository Implementations
?   ??? Identity/   # Authentication
?   ??? Messaging/              # Event Bus (RabbitMQ)
?   ??? BackgroundJobs/         # Hangfire Jobs
?   ??? Storage/     # File Storage
?   ??? Caching/    # Redis Cache
?   ??? Search/             # Search Engine
?
??? GAHAR.Shared/        # Shared Contracts
    ??? DTOs/          # Data Transfer Objects
    ??? Events/       # Event Contracts
    ??? Constants/         # Application Constants
    ??? Helpers/            # Utility Classes
```

---

## 3. Database Commands

### Entity Framework Migrations

```bash
# Create Migration
dotnet ef migrations add MigrationName --startup-project GAHAR.API --project GAHAR.Infrastructure

# Update Database
dotnet ef database update --startup-project GAHAR.API --project GAHAR.Infrastructure

# Remove Last Migration (if not applied)
dotnet ef migrations remove --startup-project GAHAR.API --project GAHAR.Infrastructure

# Rollback to specific migration
dotnet ef database update PreviousMigrationName --startup-project GAHAR.API --project GAHAR.Infrastructure

# Generate SQL Script
dotnet ef migrations script --startup-project GAHAR.API --project GAHAR.Infrastructure --output migration.sql

# Drop Database
dotnet ef database drop --startup-project GAHAR.API --project GAHAR.Infrastructure

# List Migrations
dotnet ef migrations list --startup-project GAHAR.API --project GAHAR.Infrastructure
```

### SQL Server Commands

```bash
# Connect to SQL Server (Docker)
docker exec -it gahar-sqlserver /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P 'YourPassword'

# Backup Database
docker exec -it gahar-sqlserver /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P 'YourPassword' -Q "BACKUP DATABASE GAHAR_CMS TO DISK = '/var/opt/mssql/backup/gahar_backup.bak'"

# Restore Database
docker exec -it gahar-sqlserver /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P 'YourPassword' -Q "RESTORE DATABASE GAHAR_CMS FROM DISK = '/var/opt/mssql/backup/gahar_backup.bak' WITH REPLACE"

# List Databases
docker exec -it gahar-sqlserver /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P 'YourPassword' -Q "SELECT name FROM sys.databases"
```

---

## 4. Git Workflow

### Daily Workflow

```bash
# 1. Start of day - Pull latest
git checkout develop
git pull origin develop

# 2. Create feature branch
git checkout -b feature/my-feature

# 3. Work and commit frequently
git add .
git commit -m "feat(content): add new feature"

# 4. Push to remote
git push origin feature/my-feature

# 5. Create Pull Request on GitHub
# (via GitHub UI)

# 6. After PR approval, merge to develop
# (via GitHub UI)

# 7. Delete local branch
git branch -d feature/my-feature

# 8. Delete remote branch
git push origin --delete feature/my-feature
```

### Commit Message Format

```bash
# Format: <type>(<scope>): <subject>

feat(content): add multi-layout support
fix(auth): resolve token expiration issue
docs(api): update content API documentation
refactor(repository): optimize content query
test(content): add unit tests for ContentService
chore(deps): update EF Core to 9.0.0
```

### Common Git Commands

```bash
# Status
git status

# View Changes
git diff

# View Commit History
git log --oneline

# Undo Last Commit (keep changes)
git reset --soft HEAD~1

# Undo Last Commit (discard changes)
git reset --hard HEAD~1

# Stash Changes
git stash
git stash pop

# Merge develop into current branch
git merge develop

# Rebase current branch on develop
git rebase develop

# Resolve Merge Conflicts
git mergetool

# View Remote Branches
git branch -r

# Delete Local Branch
git branch -d branch-name

# Delete Remote Branch
git push origin --delete branch-name
```

---

## 5. API Endpoints Summary

### Authentication
```
POST   /api/auth/register    - Register new user
POST   /api/auth/login     - Login
POST   /api/auth/refresh-token     - Refresh access token
POST   /api/auth/logout  - Logout
```

### Content Management
```
POST   /api/content/{type}    - Create content
GET    /api/content/{type}?lang=ar           - List content
GET    /api/content/{type}/{slug}?lang=ar    - Get by slug
PUT    /api/content/{type}/{id}?lang=ar      - Update translation
DELETE /api/content/{type}/{id}   - Delete
PUT    /api/content/{type}/{id}/publish      - Publish
PUT    /api/content/{type}/{id}/move         - Move to different type
```

### Media Management
```
POST   /api/media/upload        - Upload file
GET    /api/media      - List files
GET /api/media/{id}  - Get file
DELETE /api/media/{id}    - Delete file
```

### Layouts
```
POST   /api/layouts            - Create layout
GET    /api/layouts?contentType=news       - List layouts
GET    /api/layouts/{id}     - Get layout
PUT    /api/layouts/{id}  - Update layout
DELETE /api/layouts/{id}        - Delete layout
PUT    /api/layouts/{id}/set-default         - Set as default
POST   /api/layouts/{id}/duplicate           - Duplicate layout
```

### Pages
```
POST   /api/pages           - Create page
GET    /api/pages?lang=ar          - List pages
GET    /api/pages/{slug}?lang=ar   - Get page
PUT    /api/pages/{id}?lang=ar   - Update page
DELETE /api/pages/{id}     - Delete page
```

### Forms
```
POST   /api/forms        - Create form
GET    /api/forms    - List forms
GET    /api/forms/{id}       - Get form
POST   /api/forms/{id}/fields             - Add field
PUT    /api/forms/{id}/fields/{fieldId}        - Update field
DELETE /api/forms/{id}/fields/{fieldId}     - Delete field
PUT    /api/forms/{id}/reorder     - Reorder fields
POST   /api/forms/{id}/submit   - Submit form
GET    /api/forms/{id}/submissions  - Get submissions
GET    /api/forms/{id}/export  - Export to Excel
```

### Albums
```
POST   /api/albums              - Create album
GET    /api/albums?lang=ar  - List albums
GET    /api/albums/{slug}?lang=ar - Get album
POST   /api/albums/{id}/images     - Upload image
POST   /api/albums/{id}/images/bulk-upload  - Bulk upload
GET /api/albums/{id}/images/upload-progress/{jobId} - Upload progress
PUT    /api/albums/{id}/images/reorder   - Reorder images
PUT    /api/albums/{id}/cover-image/{imageId}      - Set cover
DELETE /api/albums/{id}/images/{imageId}          - Delete image
```

### Facilities
```
POST   /api/facilities/upload               - Upload Excel
GET    /api/facilities/upload-status/{jobId}     - Upload status
GET    /api/facilities?lang=ar&city=??????     - List facilities
GET    /api/facilities/{slug}?lang=ar            - Get facility
GET    /api/facilities/geojson?lang=ar           - GeoJSON for map
```

### Certificates
```
GET    /api/certificates/validate/{number}?lang=ar   - Validate (public)
POST/api/certificates        - Create (admin)
PUT    /api/certificates/{id}       - Update
GET    /api/certificates?status=valid    - List
```

### Menus
```
POST   /api/menus     - Create menu
GET    /api/menus?location=header           - List menus
GET    /api/menus/{id}/items?lang=ar           - Get menu with items
POST   /api/menus/{id}/items          - Add menu item
PUT    /api/menus/{id}/items/{itemId}          - Update item
DELETE /api/menus/{id}/items/{itemId}    - Delete item
PUT    /api/menus/{id}/items/reorder         - Reorder items
GET    /api/icons?library=lucide&search=home - Search icons
```

### Search
```
GET    /api/search?q=keyword&lang=ar&type=news&page=1
```

---

## 6. Common Code Snippets

### 6.1 Create New Entity

```csharp
// Entity
public class MyEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

// Configuration
public class MyEntityConfiguration : IEntityTypeConfiguration<MyEntity>
{
    public void Configure(EntityTypeBuilder<MyEntity> builder)
    {
        builder.ToTable("MyEntities");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Name).HasMaxLength(200).IsRequired();
        builder.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
        builder.HasIndex(e => e.IsActive);
    }
}

// Add to DbContext
public DbSet<MyEntity> MyEntities { get; set; }

// Create Migration
dotnet ef migrations add AddMyEntity --startup-project GAHAR.API
```

### 6.2 Create New Repository

```csharp
// Interface
public interface IMyEntityRepository : IRepository<MyEntity>
{
    Task<MyEntity?> GetByNameAsync(string name);
    Task<IEnumerable<MyEntity>> GetActiveAsync();
}

// Implementation
public class MyEntityRepository : GenericRepository<MyEntity>, IMyEntityRepository
{
    public MyEntityRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<MyEntity?> GetByNameAsync(string name)
    {
        return await _context.MyEntities
 .FirstOrDefaultAsync(e => e.Name == name);
    }

    public async Task<IEnumerable<MyEntity>> GetActiveAsync()
    {
   return await _context.MyEntities
            .Where(e => e.IsActive)
.ToListAsync();
    }
}

// Add to UnitOfWork
private IMyEntityRepository? _myEntityRepository;
public IMyEntityRepository MyEntityRepository =>
    _myEntityRepository ??= new MyEntityRepository(_context);
```

### 6.3 Create New Service

```csharp
// Interface
public interface IMyEntityService
{
    Task<MyEntityDto> CreateAsync(CreateMyEntityRequest request);
    Task<MyEntityDto> GetByIdAsync(Guid id);
    Task<List<MyEntityDto>> GetAllAsync();
    Task UpdateAsync(Guid id, UpdateMyEntityRequest request);
    Task DeleteAsync(Guid id);
}

// Implementation
public class MyEntityService : IMyEntityService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<MyEntityService> _logger;

    public MyEntityService(
        IUnitOfWork unitOfWork,
      IMapper mapper,
     ILogger<MyEntityService> logger)
  {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
  }

    public async Task<MyEntityDto> CreateAsync(CreateMyEntityRequest request)
    {
 var entity = new MyEntity
{
            Id = Guid.NewGuid(),
            Name = request.Name,
          IsActive = true,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow
        };

    await _unitOfWork.MyEntityRepository.AddAsync(entity);
   await _unitOfWork.SaveChangesAsync();

   _logger.LogInformation("MyEntity created: {Id}", entity.Id);

        return _mapper.Map<MyEntityDto>(entity);
    }

 // ... other methods
}

// Register in Program.cs
builder.Services.AddScoped<IMyEntityService, MyEntityService>();
```

### 6.4 Create New Controller

```csharp
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class MyEntityController : ControllerBase
{
    private readonly IMyEntityService _service;
    private readonly ILogger<MyEntityController> _logger;

    public MyEntityController(
      IMyEntityService service,
 ILogger<MyEntityController> logger)
    {
     _service = service;
        _logger = logger;
    }

    [HttpPost]
[ProducesResponseType(typeof(MyEntityDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
  public async Task<IActionResult> Create([FromBody] CreateMyEntityRequest request)
    {
        var entity = await _service.CreateAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = entity.Id }, entity);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(MyEntityDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id)
  {
        var entity = await _service.GetByIdAsync(id);
    return Ok(entity);
    }

  [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var entities = await _service.GetAllAsync();
  return Ok(entities);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateMyEntityRequest request)
    {
        await _service.UpdateAsync(id, request);
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _service.DeleteAsync(id);
        return NoContent();
    }
}
```

### 6.5 Create Unit Test

```csharp
public class MyEntityServiceTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<ILogger<MyEntityService>> _loggerMock;
    private readonly MyEntityService _sut;

    public MyEntityServiceTests()
    {
      _unitOfWorkMock = new Mock<IUnitOfWork>();
        _mapperMock = new Mock<IMapper>();
        _loggerMock = new Mock<ILogger<MyEntityService>>();

        _sut = new MyEntityService(
   _unitOfWorkMock.Object,
    _mapperMock.Object,
 _loggerMock.Object);
    }

    [Fact]
    public async Task CreateAsync_WithValidData_ShouldReturnCreatedEntity()
    {
     // Arrange
      var request = new CreateMyEntityRequest { Name = "Test" };
     var entity = new MyEntity { Id = Guid.NewGuid(), Name = "Test" };
        var dto = new MyEntityDto { Id = entity.Id, Name = "Test" };

        _unitOfWorkMock.Setup(u => u.MyEntityRepository.AddAsync(It.IsAny<MyEntity>()))
            .ReturnsAsync(entity);
        _mapperMock.Setup(m => m.Map<MyEntityDto>(It.IsAny<MyEntity>()))
     .Returns(dto);

        // Act
        var result = await _sut.CreateAsync(request);

        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be("Test");
        _unitOfWorkMock.Verify(u => u.MyEntityRepository.AddAsync(It.IsAny<MyEntity>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
    }
}
```

---

## 7. Troubleshooting

### Problem: Migration fails
```bash
# Solution 1: Drop database and recreate
dotnet ef database drop --startup-project GAHAR.API
dotnet ef database update --startup-project GAHAR.API

# Solution 2: Remove last migration and recreate
dotnet ef migrations remove --startup-project GAHAR.API
dotnet ef migrations add MigrationName --startup-project GAHAR.API
dotnet ef database update --startup-project GAHAR.API
```

### Problem: Port already in use
```bash
# Windows - Kill process on port 5000
netstat -ano | findstr :5000
taskkill /PID <PID> /F

# Linux/Mac - Kill process on port 5000
lsof -i :5000
kill -9 <PID>
```

### Problem: Docker container won't start
```bash
# View logs
docker-compose logs gahar-api

# Rebuild container
docker-compose up -d --build --force-recreate gahar-api

# Remove all and start fresh
docker-compose down -v
docker-compose up -d
```

### Problem: Tests failing
```bash
# Clean and rebuild
dotnet clean
dotnet build
dotnet test

# Run specific test
dotnet test --filter "FullyQualifiedName~MyTest"

# Run with detailed output
dotnet test --logger "console;verbosity=detailed"
```

### Problem: Connection string not working
```bash
# Check appsettings.json
# Check environment variables
# Check Docker networking

# Test SQL Server connection
docker exec -it gahar-sqlserver /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P 'YourPassword'
```

---

## 8. Useful Links

### Documentation
- [.NET 8 Documentation](https://learn.microsoft.com/en-us/dotnet/core/whats-new/dotnet-8)
- [EF Core Documentation](https://learn.microsoft.com/en-us/ef/core/)
- [ASP.NET Core Documentation](https://learn.microsoft.com/en-us/aspnet/core/)

### Tools
- [Swagger UI](http://localhost:5000/swagger) - API Documentation
- [Hangfire Dashboard](http://localhost:5000/hangfire) - Background Jobs
- [RabbitMQ Management](http://localhost:15672) - Message Queue

### Project Links
- [GitHub Repository](https://github.com/gahar/cms-backend)
- [Jira Board](https://gahar.atlassian.net/jira/software/projects/GAHAR)
- [Confluence Wiki](https://gahar.atlassian.net/wiki)

---

## ?? Quick Start Checklist

### First Time Setup
- [ ] Clone repository
- [ ] Install .NET 8 SDK
- [ ] Install Docker Desktop
- [ ] Run `docker-compose up -d`
- [ ] Run `dotnet restore`
- [ ] Run `dotnet ef database update --startup-project GAHAR.API`
- [ ] Run `dotnet run --project GAHAR.API`
- [ ] Open http://localhost:5000/swagger

### Daily Start
- [ ] Pull latest code: `git pull origin develop`
- [ ] Start Docker: `docker-compose up -d`
- [ ] Run API: `cd GAHAR.API && dotnet run`
- [ ] Open Swagger: http://localhost:5000/swagger

---

## ?? ?????

**??? ????? ?????:**
1. ???? ?? [Troubleshooting](#troubleshooting)
2. ???? ?? GitHub Issues
3. ???? ?? Slack #dev-backend
4. ???? GitHub Issue ????

---

**?? ????? ??? ?????? ??????:** ???? ???????  
**???????:** 10 ?????? 2025  
**???????:** 1.0

**???? ??? ????? ?? Favorites! ??**
