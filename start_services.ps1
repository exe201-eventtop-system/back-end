# Tạo hàm để khởi chạy các service
function Start-Services {
    Write-Host "Starting services..."

    # Chạy UserService
    Start-Process powershell -ArgumentList "cd UserService; dotnet run"

    # Chạy ProductService
    # Start-Process powershell -ArgumentList "cd ProductService; dotnet run"

    # Chạy BookingService
    # Start-Process powershell -ArgumentList "cd BookingService; dotnet run"
}

# Chạy API Gateway (Ocelot)
function Start-ApiGateway {
    Write-Host "Starting API Gateway..."
    # Đợi để các service con có thời gian khởi động (bạn có thể điều chỉnh thời gian nếu cần)
    Start-Sleep -Seconds 5

    # Chạy API Gateway
    Start-Process powershell -ArgumentList "cd API-Gateway\API; dotnet run"
}

# Gọi hàm để khởi chạy các service trước khi chạy API Gateway
Start-Services

# Sau khi các service đã được chạy, khởi động API Gateway
Start-ApiGateway
