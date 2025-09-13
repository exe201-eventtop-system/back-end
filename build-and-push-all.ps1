# Build and Push All Services to Docker Hub
Write-Host "Starting build and push process for all services..." -ForegroundColor Green

# API Gateway
Write-Host "Building API Gateway..." -ForegroundColor Yellow
docker build -t trilm189/apigateway:latest -f API-Gateway/API/Dockerfile ./API-Gateway
if ($LASTEXITCODE -eq 0) {
    Write-Host "Pushing API Gateway..." -ForegroundColor Yellow
    docker push trilm189/apigateway:latest
    Write-Host "API Gateway completed!" -ForegroundColor Green
}
else {
    Write-Host "API Gateway build failed!" -ForegroundColor Red
}

# Auth Service
Write-Host "Building Auth Service..." -ForegroundColor Yellow
docker build -t trilm189/authservice:latest -f AuthService/AuthService/Dockerfile ./AuthService
if ($LASTEXITCODE -eq 0) {
    Write-Host "Pushing Auth Service..." -ForegroundColor Yellow
    docker push trilm189/authservice:latest
    Write-Host "Auth Service completed!" -ForegroundColor Green
}
else {
    Write-Host "Auth Service build failed!" -ForegroundColor Red
}

# Planning Service
Write-Host "Building Planning Service..." -ForegroundColor Yellow
docker build -t trilm189/planningservice:latest -f PlanningService/API/Dockerfile ./PlanningService
if ($LASTEXITCODE -eq 0) {
    Write-Host "Pushing Planning Service..." -ForegroundColor Yellow
    docker push trilm189/planningservice:latest
    Write-Host "Planning Service completed!" -ForegroundColor Green
}
else {
    Write-Host "Planning Service build failed!" -ForegroundColor Red
}

# Product Service
Write-Host "Building Product Service..." -ForegroundColor Yellow
docker build -t trilm189/productservice:latest -f ProductsService/API/Dockerfile ./ProductsService
if ($LASTEXITCODE -eq 0) {
    Write-Host "Pushing Product Service..." -ForegroundColor Yellow
    docker push trilm189/productservice:latest
    Write-Host "Product Service completed!" -ForegroundColor Green
}
else {
    Write-Host "Product Service build failed!" -ForegroundColor Red
}

# Event Service
Write-Host "Building Event Service..." -ForegroundColor Yellow
docker build -t trilm189/eventservice:latest -f EventService/API/Dockerfile ./EventService
if ($LASTEXITCODE -eq 0) {
    Write-Host "Pushing Event Service..." -ForegroundColor Yellow
    docker push trilm189/eventservice:latest
    Write-Host "Event Service completed!" -ForegroundColor Green
}
else {
    Write-Host "Event Service build failed!" -ForegroundColor Red
}

# Cart Service
Write-Host "Building Cart Service..." -ForegroundColor Yellow
docker build -t trilm189/cartservice:latest -f CartService/API/Dockerfile ./CartService
if ($LASTEXITCODE -eq 0) {
    Write-Host "Pushing Cart Service..." -ForegroundColor Yellow
    docker push trilm189/cartservice:latest
    Write-Host "Cart Service completed!" -ForegroundColor Green
}
else {
    Write-Host "Cart Service build failed!" -ForegroundColor Red
}

# Blog Service
Write-Host "Building Blog Service..." -ForegroundColor Yellow
docker build -t trilm189/blogservice:latest -f BlogService/API/Dockerfile ./BlogService
if ($LASTEXITCODE -eq 0) {
    Write-Host "Pushing Blog Service..." -ForegroundColor Yellow
    docker push trilm189/blogservice:latest
    Write-Host "Blog Service completed!" -ForegroundColor Green
}
else {
    Write-Host "Blog Service build failed!" -ForegroundColor Red
}

Write-Host "All services build and push process completed!" -ForegroundColor Green 