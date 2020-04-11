using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SecretMagic.Model;

namespace SecretMagic.API.Services
{
    public interface IBlogService
    {
        IEnumerable<BlogInfo> GetAllBlogs();
        BlogInfo GetBlogById(Guid id);
        IEnumerable<Blog> GetBlogsByCategoryId(Guid id);
        Task UpdateBlog(BlogInfo blog);
        Task<Blog> AddBlog(BlogInfo blog);
        Task<int> DeleteBlog(Guid id);
        IEnumerable<Category> GetAllCategories();
        Task<Category> AddCategory(Category category);
        Task<int> DeleteCategory(Guid id);
        IEnumerable<BlogInfo> GetBlogs(int pageIndex = 1, int pageSize = 10, string query = null, Guid? categoryId = null, bool isSummary = true);
        Task<int> GetCount();
    }
}