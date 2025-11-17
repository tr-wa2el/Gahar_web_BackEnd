# Test Users Login Script for Windows / سكريبت الدخول للمستخدمين التجريبيين
# يساعدك على الدخول بسرعة واختبار الصلاحيات

$API_URL = "http://localhost:5000"

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "🔐 Gahar Backend - Test Users Login" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

# Define test users
$users = @(
    @{Name = "Super Admin"; Email = "admin@gahar.sa"; Password = "Admin@123"},
    @{Name = "Admin"; Email = "admin@example.com"; Password = "Admin@123"},
    @{Name = "Editor"; Email = "editor@example.com"; Password = "Editor@123"},
    @{Name = "Viewer"; Email = "viewer@example.com"; Password = "Viewer@123"},
    @{Name = "Regular User"; Email = "user@example.com"; Password = "User@123"}
)

# Display available users
Write-Host "📋 Available Test Users:" -ForegroundColor Green
Write-Host "========================" -ForegroundColor Green
for ($i = 0; $i -lt $users.Count; $i++) {
    $index = $i + 1
    Write-Host "$index) $($users[$i].Name) ($($users[$i].Email))" -ForegroundColor Yellow
}
Write-Host ""

# Get user input
$choice = Read-Host "Select user (1-$($users.Count))"

# Validate input
if ($choice -notmatch '^\d+$' -or [int]$choice -lt 1 -or [int]$choice -gt $users.Count) {
    Write-Host "❌ Invalid choice. Please select 1-$($users.Count)." -ForegroundColor Red
    exit
}

# Get selected user details
$selectedUser = $users[[int]$choice - 1]
$email = $selectedUser.Email
$password = $selectedUser.Password
$role = $selectedUser.Name

Write-Host ""
Write-Host "🔐 Logging in as: $role ($email)" -ForegroundColor Cyan
Write-Host "Endpoint: $API_URL/api/auth/login" -ForegroundColor Gray
Write-Host ""

# Make login request
try {
    $loginBody = @{
      email = $email
        password = $password
    } | ConvertTo-Json

    $response = Invoke-RestMethod -Uri "$API_URL/api/auth/login" `
     -Method Post `
        -Headers @{"Content-Type" = "application/json"} `
     -Body $loginBody

    # Login successful
    Write-Host "✅ Login Successful!" -ForegroundColor Green
    Write-Host ""
    
    $token = $response.accessToken
    $refreshToken = $response.refreshToken
    
    Write-Host "📝 Your Tokens:" -ForegroundColor Cyan
    Write-Host "===============" -ForegroundColor Cyan
    Write-Host ""
    
    Write-Host "Access Token:" -ForegroundColor Yellow
    Write-Host "Bearer $token" -ForegroundColor Green
    Write-Host ""
    
    Write-Host "Refresh Token:" -ForegroundColor Yellow
  Write-Host "$refreshToken" -ForegroundColor Green
    Write-Host ""
    
    # Check if user can access Swagger
    if ($role -eq "Super Admin" -or $role -eq "Admin") {
Write-Host "✅ Swagger Access: ENABLED" -ForegroundColor Green
        Write-Host "🌐 Swagger URL: $API_URL/swagger/index.html" -ForegroundColor Cyan
      Write-Host "🔑 Authorization: Bearer $token" -ForegroundColor Cyan
        Write-Host ""
   
   Write-Host "📌 Steps to access Swagger:" -ForegroundColor Yellow
        Write-Host "1. Open: $API_URL/swagger/index.html" -ForegroundColor White
     Write-Host "2. Click the 'Authorize' button (🔒)" -ForegroundColor White
        Write-Host "3. Paste: Bearer $token" -ForegroundColor White
    Write-Host "4. Click 'Authorize'" -ForegroundColor White
    } else {
    Write-Host "❌ Swagger Access: DISABLED" -ForegroundColor Red
        Write-Host "💡 Tip: Use Admin account to access Swagger" -ForegroundColor Yellow
    }
    
    Write-Host ""
    Write-Host "🧪 Test API Request:" -ForegroundColor Cyan
    Write-Host "====================" -ForegroundColor Cyan
    Write-Host "`$headers = @{'Authorization' = 'Bearer $token'}" -ForegroundColor White
    Write-Host "`$response = Invoke-RestMethod -Uri '$API_URL/api/endpoint' -Headers `$headers" -ForegroundColor White
    Write-Host ""
    
    # Option to copy token
    Write-Host "💾 Copy to Clipboard:" -ForegroundColor Yellow
    $copyToken = Read-Host "Copy access token to clipboard? (y/n)"
    if ($copyToken -eq "y") {
   $token | Set-Clipboard
    Write-Host "✅ Token copied to clipboard!" -ForegroundColor Green
    }
    
} catch {
    Write-Host "❌ Login Failed!" -ForegroundColor Red
    Write-Host "Error: $($_.Exception.Message)" -ForegroundColor Red
    exit
}

Write-Host ""
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""
Write-Host "📚 Documentation:" -ForegroundColor Cyan
Write-Host "- TEST_USERS.md: المستخدمين التجريبيين" -ForegroundColor Gray
Write-Host "- SWAGGER_QUICK_START.md: البدء السريع" -ForegroundColor Gray
Write-Host "- SWAGGER_API_EXAMPLES.md: أمثلة API" -ForegroundColor Gray
Write-Host ""
