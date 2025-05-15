const postService = require("../services/postService");

// Lấy danh sách bài viết
const getAllPosts = async (req, res) => {
  try {
    const posts = await postService.getPosts();
    res.status(200).json(posts); // Trả về mảng bài viết
  } catch (error) {
    res.status(500).json({ message: error.message });
  }
};

module.exports = { getAllPosts };
