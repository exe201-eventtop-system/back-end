FROM node:20

# Tạo thư mục app trong container
WORKDIR /app

# Copy package.json và package-lock.json (nếu có)
COPY package*.json ./

# Cài đặt dependencies
RUN npm install

# Copy toàn bộ mã nguồn vào container
COPY . .

# Mở cổng mà ứng dụng sử dụng
EXPOSE 3000

# Lệnh chạy ứng dụng
CMD ["node", "app.js"]
