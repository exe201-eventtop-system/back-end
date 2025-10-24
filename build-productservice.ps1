# Build and Push ProductService to Docker Hub
Write-Host "Starting build and push process for ProductService..." -ForegroundColor Green

# Product Service
Write-Host "Building Product Service..." -ForegroundColor Yellow
docker build -t trilm189/productservice:latest -f ProductsService/API/Dockerfile .
if ($LASTEXITCODE -eq 0) {
    Write-Host "Pushing Product Service..." -ForegroundColor Yellow
    docker push trilm189/productservice:latest
    Write-Host "Product Service build and push completed successfully!" -ForegroundColor Green
}
else {
    Write-Host "Product Service build failed!" -ForegroundColor Red
    exit 1
}

Write-Host "ProductService build and push process completed!" -ForegroundColor Green
