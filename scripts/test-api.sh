#!/bin/bash
# Test script for Spectra API

set -e

API_URL="http://localhost:8080"
EMAIL="test@example.com"
PASSWORD="Test123!"
DISPLAY_NAME="Test User"

echo "🧪 Testing Spectra API..."

# Wait for API to be ready
echo "⏳ Waiting for API to be ready..."
for i in {1..30}; do
    if curl -f "$API_URL/health" > /dev/null 2>&1; then
        echo "✅ API is ready!"
        break
    fi
    if [ $i -eq 30 ]; then
        echo "❌ API failed to start"
        exit 1
    fi
    sleep 2
done

# Test health endpoint
echo "🔍 Testing health endpoint..."
HEALTH_RESPONSE=$(curl -s "$API_URL/health")
if [ "$HEALTH_RESPONSE" = "Healthy" ]; then
    echo "✅ Health check passed"
else
    echo "❌ Health check failed: $HEALTH_RESPONSE"
    exit 1
fi

# Test user registration
echo "👤 Testing user registration..."
REGISTER_RESPONSE=$(curl -s -X POST "$API_URL/api/auth/register" \
    -H "Content-Type: application/json" \
    -d "{
        \"email\": \"$EMAIL\",
        \"password\": \"$PASSWORD\",
        \"confirmPassword\": \"$PASSWORD\",
        \"displayName\": \"$DISPLAY_NAME\"
    }")

if echo "$REGISTER_RESPONSE" | grep -q '"success":true'; then
    echo "✅ User registration successful"
    TOKEN=$(echo "$REGISTER_RESPONSE" | grep -o '"token":"[^"]*' | cut -d'"' -f4)
else
    echo "❌ User registration failed: $REGISTER_RESPONSE"
    exit 1
fi

# Test user login
echo "🔐 Testing user login..."
LOGIN_RESPONSE=$(curl -s -X POST "$API_URL/api/auth/login" \
    -H "Content-Type: application/json" \
    -d "{
        \"email\": \"$EMAIL\",
        \"password\": \"$PASSWORD\"
    }")

if echo "$LOGIN_RESPONSE" | grep -q '"success":true'; then
    echo "✅ User login successful"
    TOKEN=$(echo "$LOGIN_RESPONSE" | grep -o '"token":"[^"]*' | cut -d'"' -f4)
else
    echo "❌ User login failed: $LOGIN_RESPONSE"
    exit 1
fi

# Test protected endpoint
echo "🛡️  Testing protected endpoint..."
USER_RESPONSE=$(curl -s -X GET "$API_URL/api/auth/me" \
    -H "Authorization: Bearer $TOKEN")

if echo "$USER_RESPONSE" | grep -q "$EMAIL"; then
    echo "✅ Protected endpoint access successful"
else
    echo "❌ Protected endpoint access failed: $USER_RESPONSE"
    exit 1
fi

echo "🎉 All tests passed! Spectra API is working correctly."
echo "📖 API Documentation: $API_URL"
echo "🔍 Health Check: $API_URL/health"
