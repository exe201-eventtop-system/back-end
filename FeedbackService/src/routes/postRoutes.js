const express = require("express");
const postController = require("../controllers/postController");
const router = express.Router();

/**
 * @swagger
 * tags:
 *   name: Posts
 *   description: Các API liên quan đến bài viết
 */

/**
 * @swagger
 * /api/posts:
 *   get:
 *     summary: Lấy danh sách tất cả bài viết
 *     tags: [Posts]
 *     responses:
 *       200:
 *         description: Danh sách bài viết
 *         content:
 *           application/json:
 *             schema:
 *               type: array
 *               items:
 *                 type: object
 *                 properties:
 *                   id:
 *                     type: integer
 *                   title:
 *                     type: string
 *                   content:
 *                     type: string
 */
router.get("/", postController.getAllPosts);

module.exports = router;
