// src/routes/postRoutes.js

const express = require("express");
const postController = require("../controllers/postController");
const router = express.Router();

// Route để lấy danh sách bài viết
router.get("/", postController.getAllPosts);

module.exports = router;
