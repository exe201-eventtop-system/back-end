const postModel = require("../models/postModel");

const getPosts = async () => {
  try {
    const posts = postModel.getAllPosts();
    return posts;
  } catch (error) {
    throw new Error("Error fetching posts");
  }
};

module.exports = { getPosts };
