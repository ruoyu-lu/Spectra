#!/bin/bash

echo "🧪 Testing .NET Aspire Setup for Spectra"
echo "========================================"

# Check if Aspire dashboard is accessible
echo "1. Testing Aspire Dashboard..."
if curl -s -k https://localhost:17255 > /dev/null; then
    echo "   ✅ Aspire Dashboard is accessible"
else
    echo "   ❌ Aspire Dashboard is not accessible"
fi

# Wait for services to start
echo "2. Waiting for services to start (15 seconds)..."
sleep 15

# Check common ports for API service
echo "3. Testing API Service endpoints..."
API_PORTS=(5374 7502 8080 5000)
API_FOUND=false

for port in "${API_PORTS[@]}"; do
    echo "   Testing port $port..."
    if curl -s http://localhost:$port/health > /dev/null 2>&1; then
        echo "   ✅ API Service found on port $port"
        echo "   Health check: $(curl -s http://localhost:$port/health)"
        API_FOUND=true
        break
    elif curl -s http://localhost:$port > /dev/null 2>&1; then
        echo "   ✅ API Service found on port $port (no health endpoint)"
        API_FOUND=true
        break
    fi
done

if [ "$API_FOUND" = false ]; then
    echo "   ❌ API Service not found on any common port"
fi

# Check if PostgreSQL container is running
echo "4. Testing PostgreSQL container..."
if docker ps | grep -q postgres; then
    echo "   ✅ PostgreSQL container is running"
    docker ps | grep postgres
else
    echo "   ❌ PostgreSQL container not found"
fi

echo ""
echo "🎯 Summary:"
echo "- Aspire Dashboard: https://localhost:17255"
echo "- Check the dashboard for service status and endpoints"
echo "- Services may take additional time to fully start"

echo ""
echo "📊 Current Docker containers:"
docker ps --format "table {{.Names}}\t{{.Status}}\t{{.Ports}}"
