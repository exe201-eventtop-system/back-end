const posts = [
  { id: 1, title: "Post 1", content: "Content for post 1" },
  { id: 2, title: "Post 2", content: "Content for post 2" },
  { id: 3, title: "Post 3", content: "Content for post 3" },
];

const getAllPosts = () => {
  return posts;
};

const getPostById = (id) => {
  return posts.find((post) => post.id === id);
};

module.exports = { getAllPosts, getPostById };
