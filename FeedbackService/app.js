// src/app.js

const express = require("express");
const bodyParser = require("body-parser");
const postRoutes = require("./src/routes/postRoutes");

const app = express();

// Middleware toàn cục để parse JSON
app.use(bodyParser.json());

// Đăng ký các route
app.use("/api/posts", postRoutes); // Các route của bài viết

// Khởi động server
app.listen(3000, () => {
  console.log("Server running on port 3000");
});
