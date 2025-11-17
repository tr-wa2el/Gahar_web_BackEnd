#!/bin/bash

# Test Users Login Script / سكريبت الدخول للمستخدمين التجريبيين
# يساعدك على الدخول بسرعة واختبار الصلاحيات

API_URL="http://localhost:5000"

echo "========================================"
echo "🔐 Gahar Backend - Test Users Login"
echo "========================================"
echo ""

# Array of test users
declare -A users=(
    [1]="admin@gahar.sa|Admin@123|Super Admin"
    [2]="admin@example.com|Admin@123|Admin"
  [3]="editor@example.com|Editor@123|Editor"
    [4]="viewer@example.com|Viewer@123|Viewer"
    [5]="user@example.com|User@123|Regular User"
)

# Display available users
echo "📋 Available Test Users:"
echo "========================"
for key in "${!users[@]}"; do
    IFS='|' read -r email password role <<< "${users[$key]}"
  echo "$key) $role ($email)"
done
echo ""

# Get user input
read -p "Select user (1-5): " user_choice

# Validate input
if [[ ! $user_choice =~ ^[1-5]$ ]]; then
    echo "❌ Invalid choice. Please select 1-5."
    exit 1
fi

# Get selected user details
IFS='|' read -r email password role <<< "${users[$user_choice]}"

echo ""
echo "🔐 Logging in as: $role ($email)"
echo "Endpoint: $API_URL/api/auth/login"
echo ""

# Make login request
response=$(curl -s -X POST "$API_URL/api/auth/login" \
    -H "Content-Type: application/json" \
    -d "{\"email\":\"$email\",\"password\":\"$password\"}")

# Check if login was successful
if echo "$response" | grep -q "accessToken"; then
    echo "✅ Login Successful!"
    echo ""
    
 # Extract token
    token=$(echo "$response" | grep -o '"accessToken":"[^"]*' | cut -d'"' -f4)
  refresh_token=$(echo "$response" | grep -o '"refreshToken":"[^"]*' | cut -d'"' -f4)
    
    echo "📝 Your Tokens:"
    echo "==============="
    echo ""
    echo "Access Token:"
    echo "Bearer $token"
    echo ""
    echo "Refresh Token:"
  echo "$refresh_token"
    echo ""

    # Show Swagger access info
    if [[ "$role" == "Super Admin" ]] || [[ "$role" == "Admin" ]]; then
        echo "✅ Swagger Access: ENABLED"
    echo "🌐 Swagger URL: $API_URL/swagger/index.html"
    echo "🔑 Authorization: Bearer $token"
   echo ""
        echo "📌 Steps to access Swagger:"
        echo "1. Go to: $API_URL/swagger/index.html"
        echo "2. Click the 'Authorize' button (🔒)"
        echo "3. Paste: Bearer $token"
        echo "4. Click 'Authorize'"
    else
        echo "❌ Swagger Access: DISABLED (Admin only)"
  echo "💡 Tip: Use Admin account to access Swagger"
    fi
    
    echo ""
    echo "🧪 Test API Request:"
    echo "===================="
    echo "curl -H \"Authorization: Bearer $token\" \"$API_URL/api/endpoint\""
    echo ""
  
else
    echo "❌ Login Failed!"
    echo "Response: $response"
    exit 1
fi

echo "========================================"
