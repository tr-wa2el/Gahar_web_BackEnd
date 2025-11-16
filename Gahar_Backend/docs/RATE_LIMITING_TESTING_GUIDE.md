# 🧪 Rate Limiting - Testing Guide

**Date:** 16 January 2025  
**Status:** ✅ **READY FOR TESTING**

---

## ✅ Pre-Testing Checklist

- ✅ Build successful
- ✅ All services registered
- ✅ Middleware configured
- ✅ Controller endpoints available
- ✅ No compilation errors

---

## 🧪 Testing Scenarios

### Test 1: Basic Rate Limit Check

#### Scenario: User makes 100 requests (should all succeed)

**Steps:**
```bash
# Make 100 sequential requests
for i in {1..100}; do
  curl -i https://localhost:5001/api/ratelimit/status \
    -H "Authorization: Bearer {token}" \
    -s | grep "HTTP/"
  echo "Request $i"
done
```

**Expected Result:**
- ✅ All 100 requests return `200 OK`
- ✅ `X-RateLimit-Remaining` decreases from 99 to 0
- ✅ No 429 responses

---

### Test 2: Rate Limit Exceeded

#### Scenario: User makes 101st request (should fail)

**Steps:**
```bash
# Make 101 requests
for i in {1..101}; do
  response=$(curl -i https://localhost:5001/api/ratelimit/status \
    -H "Authorization: Bearer {token}" \
    -s -o /dev/null -w "%{http_code}")
  echo "Request $i: $response"
done
```

**Expected Result:**
- ✅ First 100 requests: `200 OK`
- ✅ 101st request: `429 Too Many Requests`
- ✅ Response header: `Retry-After: 60`
- ✅ Response body includes retry message

---

### Test 3: Check Remaining Requests

#### Scenario: Check remaining requests mid-window

**Steps:**
```bash
# Make 50 requests, then check remaining
for i in {1..50}; do
  curl -s https://localhost:5001/api/ratelimit/status \
    -H "Authorization: Bearer {token}" > /dev/null
done

# Check remaining
curl https://localhost:5001/api/ratelimit/remaining \
  -H "Authorization: Bearer {token}" | jq
```

**Expected Result:**
```json
{
  "remaining": 50,
  "limit": 100,
  "window": "1 دقيقة"
}
```

---

### Test 4: Window Reset

#### Scenario: Wait 60 seconds and verify reset

**Steps:**
```bash
# Make 100 requests
for i in {1..100}; do
  curl -s https://localhost:5001/api/ratelimit/status \
    -H "Authorization: Bearer {token}" > /dev/null
done

# Try 101st request (should fail)
curl -i https://localhost:5001/api/ratelimit/status \
  -H "Authorization: Bearer {token}"
# Expected: 429

# Wait 60 seconds
echo "Waiting 60 seconds..."
sleep 60

# Try again (should succeed)
curl -i https://localhost:5001/api/ratelimit/status \
  -H "Authorization: Bearer {token}"
# Expected: 200 OK
```

**Expected Result:**
- ✅ 101st request before wait: `429`
- ✅ Request after 60s wait: `200 OK`
- ✅ `X-RateLimit-Remaining: 99`

---

### Test 5: IP-Based Rate Limiting

#### Scenario: Anonymous user (no auth token)

**Steps:**
```bash
# Make requests without auth
for i in {1..105}; do
  curl -i https://localhost:5001/api/ratelimit/status | grep "HTTP/"
done
```

**Expected Result:**
- ✅ First 100 requests: `200 OK`
- ✅ Requests 101-105: `429 Too Many Requests`

---

### Test 6: Multiple Users

#### Scenario: Two users making requests independently

**User 1:**
```bash
for i in {1..100}; do
  curl -s https://localhost:5001/api/ratelimit/status \
    -H "Authorization: Bearer {token1}" > /dev/null
done
```

**User 2 (simultaneously):**
```bash
for i in {1..100}; do
  curl -s https://localhost:5001/api/ratelimit/status \
    -H "Authorization: Bearer {token2}" > /dev/null
done
```

**Expected Result:**
- ✅ User 1: 100 requests succeed
- ✅ User 2: 100 requests succeed (independent limit)
- ✅ Each user has separate tracking

---

### Test 7: Admin Reset

#### Scenario: Admin resets user's rate limit

**Steps:**
```bash
# User makes 100 requests
for i in {1..100}; do
  curl -s https://localhost:5001/api/ratelimit/status \
    -H "Authorization: Bearer {user-token}" > /dev/null
done

# Try 101st request (should fail)
curl -i https://localhost:5001/api/ratelimit/status \
  -H "Authorization: Bearer {user-token}"
# Expected: 429

# Admin resets the user's limit
curl -X POST https://localhost:5001/api/ratelimit/reset \
  -H "Authorization: Bearer {admin-token}"

# Try again (should succeed)
curl -i https://localhost:5001/api/ratelimit/status \
  -H "Authorization: Bearer {user-token}"
# Expected: 200 OK
```

**Expected Result:**
- ✅ Before reset: `429 Too Many Requests`
- ✅ Reset request: `200 OK`
- ✅ After reset: `200 OK` with full limit restored

---

### Test 8: Response Headers

#### Scenario: Verify response headers

**Steps:**
```bash
curl -i https://localhost:5001/api/ratelimit/status \
  -H "Authorization: Bearer {token}" | grep -i "X-RateLimit"
```

**Expected Result:**
```
X-RateLimit-Limit: 100
X-RateLimit-Remaining: 99
X-RateLimit-Reset: 1737002460
```

---

### Test 9: Skip Excluded Endpoints

#### Scenario: Health check and Swagger shouldn't be rate limited

**Steps:**
```bash
# Make 105 requests to health endpoint
for i in {1..105}; do
  curl -i https://localhost:5001/health | grep "HTTP/"
done

# All should succeed
```

**Expected Result:**
- ✅ All 105 requests: `200 OK` (or appropriate response)
- ✅ No 429 responses

---

### Test 10: Get Rate Limit Info

#### Scenario: Get detailed rate limit information

**Steps:**
```bash
# Make 30 requests
for i in {1..30}; do
  curl -s https://localhost:5001/api/ratelimit/status \
    -H "Authorization: Bearer {token}" > /dev/null
done

# Get info
curl https://localhost:5001/api/ratelimit/status \
  -H "Authorization: Bearer {token}" | jq
```

**Expected Result:**
```json
{
  "identifier": "user_123",
  "currentRequests": 30,
  "maxRequests": 100,
  "remainingRequests": 70,
  "resetTime": "2025-01-16T10:31:30Z",
  "resetInSeconds": 45,
  "isLimited": false
}
```

---

## 📊 Performance Testing

### Load Test: 1000 Requests

```bash
# Using Apache Bench
ab -n 1000 -c 100 \
  -H "Authorization: Bearer {token}" \
  https://localhost:5001/api/ratelimit/status
```

**Expected Results:**
- ✅ First 100 requests: `200 OK`
- ✅ Requests 101-1000: `429 Too Many Requests`
- ✅ Response time < 100ms

---

## ✅ Verification Checklist

### Functionality
- [ ] Rate limit enforced at 100 requests
- [ ] Window resets after 60 seconds
- [ ] Multiple users have independent limits
- [ ] IP-based limiting for anonymous users
- [ ] Admin can reset limits
- [ ] Response headers included
- [ ] Excluded endpoints not rate limited

### Error Handling
- [ ] 429 response when limit exceeded
- [ ] Retry-After header included
- [ ] Error message in Arabic
- [ ] Proper HTTP status codes

### Performance
- [ ] Request check < 1ms
- [ ] No memory leaks
- [ ] Cleanup works properly
- [ ] Concurrent requests handled correctly

### Logging
- [ ] Rate limit violations logged
- [ ] Admin resets logged
- [ ] Cleanup operations logged

---

## 🐛 Troubleshooting

### Issue: 429 returned too early

**Solution:**
- Check if window hasn't reset
- Verify requests were actually made
- Check server time is correct

### Issue: Rate limit not enforced

**Solution:**
- Verify middleware is registered in Program.cs
- Check if endpoint is in excluded list
- Verify service is injected

### Issue: Multiple users sharing limit

**Solution:**
- Verify user ID is extracted correctly
- Check Authorization header is present
- Verify claims are properly set

### Issue: Reset not working

**Solution:**
- Verify user is authenticated
- Check admin authorization
- Verify identifier format matches

---

## 📈 Monitoring

### Monitor Rate Limit Violations

```bash
# Check logs for rate limit violations
dotnet run | grep "Rate limit exceeded"
```

### Monitor Rate Limit Resets

```bash
# Check logs for resets
dotnet run | grep "Rate limit reset"
```

### Monitor Cleanup Operations

```bash
# Check logs for cleanup
dotnet run | grep "Rate limit cleanup"
```

---

## 🎯 Success Criteria

All of the following must pass:

- [x] Build succeeds without errors
- [ ] All 10 test scenarios pass
- [ ] Response headers correct
- [ ] Error handling works
- [ ] Multiple users independent
- [ ] Admin reset works
- [ ] Window reset works
- [ ] Cleanup works
- [ ] Performance acceptable
- [ ] Logging correct

---

## 📝 Test Report Template

```
Test Date: [DATE]
Tester: [NAME]
Build: [VERSION]

Test Results:
- Test 1 (Basic Rate Limit): [PASS/FAIL]
- Test 2 (Exceeded): [PASS/FAIL]
- Test 3 (Check Remaining): [PASS/FAIL]
- Test 4 (Window Reset): [PASS/FAIL]
- Test 5 (IP-Based): [PASS/FAIL]
- Test 6 (Multiple Users): [PASS/FAIL]
- Test 7 (Admin Reset): [PASS/FAIL]
- Test 8 (Response Headers): [PASS/FAIL]
- Test 9 (Excluded Endpoints): [PASS/FAIL]
- Test 10 (Rate Limit Info): [PASS/FAIL]

Overall Result: [PASS/FAIL]
Issues Found: [LIST]
Recommendations: [LIST]
```

---

**Ready to Test!** ✅
