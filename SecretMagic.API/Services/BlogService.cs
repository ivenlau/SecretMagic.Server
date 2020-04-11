using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SecretMagic.API.Commom;
using SecretMagic.Model;
using SecretMagic.Repository;
using System.Linq;
using System.Text.RegularExpressions;

namespace SecretMagic.API.Services
{
    public class BlogService : IBlogService
    {
        private readonly IBlogRepository blogRepository;
        private readonly IUserRepository userRepository;
        private readonly ICategoryRepository categoryRepository;
        public BlogService(IBlogRepository blogRepository, IUserRepository userRepository, ICategoryRepository categoryRepository)
        {
            this.blogRepository = blogRepository;
            this.userRepository = userRepository;
            this.categoryRepository = categoryRepository;
        }

        public async Task<Blog> AddBlog(BlogInfo blogInfo)
        {
            if (blogInfo == null)
            {
                throw new BadRequestException("Invalid blog information!");
            }
            try
            {
                var user = userRepository.GetById(blogInfo.UserId);
                var category = new Category { Id = blogInfo.CategoryId };
                var blog = new Blog
                {
                    Title = blogInfo.Title,
                    Content = blogInfo.Content,
                    Active = blogInfo.Active,
                    Author = user,
                    Category = category,
                    ImgUrl = blogInfo.ImgUrl
                };
                return await this.blogRepository.Create(blog);
            }
            catch
            {
                throw new InternalException("DB error occurred when adding one blog.");
            }
        }

        public async Task<int> DeleteBlog(Guid id)
        {
            var blog = this.blogRepository.GetById(id);
            if (blog == null)
            {
                throw new BadRequestException("Invalid blog information!");
            }
            try
            {
                return await this.blogRepository.Delete(blog);
            }
            catch
            {
                throw new InternalException("DB error occurred when adding one blog.");
            }
        }

        public IEnumerable<BlogInfo> GetAllBlogs()
        {
            try
            {
                List<BlogInfo> result = new List<BlogInfo>();
                this.blogRepository.Read().OrderByDescending(b => b.CreateTime).ToList().ForEach(b =>
                {
                    result.Add(new BlogInfo
                    {
                        Id = b.Id,
                        Title = b.Title,
                        Content = b.Content,
                        Active = b.Active,
                        UserId = b.Author.Id,
                        UserName = b.Author.Name,
                        CategoryId = b.Category.Id,
                        CategoryName = b.Category.Name,
                        ImgUrl = b.ImgUrl,
                        Date = b.CreateTime
                    });
                });
                return result;
            }
            catch
            {
                throw new InternalException("DB error occurred when getting blogs.");
            }
        }

        public BlogInfo GetBlogById(Guid id)
        {
            try
            {
                var blog = this.blogRepository.GetById(id);
                var next = this.blogRepository.Read()
                            .Where(b => b.Category == blog.Category && b.CreateTime > blog.CreateTime)
                            .OrderBy(b => b.CreateTime)
                            .Take(1)
                            .FirstOrDefault();
                var previous = this.blogRepository.Read()
                            .Where(b => b.Category == blog.Category && b.CreateTime < blog.CreateTime)
                            .OrderByDescending(b => b.CreateTime)
                            .Take(1)
                            .FirstOrDefault();
                var result = new BlogInfo
                {
                    Id = blog.Id,
                    Title = blog.Title,
                    Content = blog.Content,
                    Active = blog.Active,
                    UserId = blog.Author.Id,
                    UserName = blog.Author.Name,
                    CategoryId = blog.Category.Id,
                    CategoryName = blog.Category.Name,
                    Date = blog.CreateTime,
                    ImgUrl = blog.ImgUrl
                };
                if (previous != null)
                {
                    result.Previous = previous.Id;
                }
                if (next != null)
                {
                    result.Next = next.Id;
                }
                return result;
            }
            catch
            {
                throw new InternalException("DB error occurred when getting blog by id.");
            }
        }

        public IEnumerable<Blog> GetBlogsByCategoryId(Guid id)
        {
            try
            {
                return this.blogRepository.GetBlogByCategory(id);
            }
            catch
            {
                throw new InternalException("DB error occurred when getting blogs by category.");
            }
        }

        public async Task UpdateBlog(BlogInfo blog)
        {
            var current = this.blogRepository.GetById(blog.Id);
            current.Title = blog.Title;
            current.Content = blog.Content;
            current.Active = blog.Active;
            current.ImgUrl = blog.ImgUrl;
            if (current == null)
            {
                throw new BadRequestException("Invalid blog information!");
            }
            try
            {
                await this.blogRepository.Update(current);
            }
            catch
            {
                throw new InternalException("DB error occurred when updating one blog.");
            }
        }

        public IEnumerable<Category> GetAllCategories()
        {
            return this.categoryRepository.Read();
        }

        public async Task<Category> AddCategory(Category category)
        {
            return await this.categoryRepository.Create(category);
        }

        public async Task<int> DeleteCategory(Guid id)
        {
            var cate = this.categoryRepository.GetById(id);
            if (cate == null)
            {
                throw new BadRequestException("Invalid categpry information!");
            }
            else
            {
                try
                {
                    return await this.categoryRepository.Delete(cate);
                }
                catch
                {
                    throw new InternalException("DB error occurred when adding one categpry.");
                }
            }
        }

        public IEnumerable<BlogInfo> GetBlogs(int pageIndex = 1, int pageSize = 10, string query = null, Guid? categoryId = null, bool isSummary = true)
        {
            try
            {
                List<BlogInfo> result = new List<BlogInfo>();
                var blogs = this.blogRepository.Read();

                if (categoryId != null)
                {
                    blogs = blogs.Where(b => b.Category.Id == categoryId);
                }
                if (query != null)
                {
                    blogs = blogs.Where(b => b.Title.Contains(query));
                }
                blogs.OrderByDescending(b => b.CreateTime).Skip((pageIndex - 1) * pageSize)
                    .Take(pageSize).ToList().ForEach(b =>
                {
                    var summary = b.Content;
                    if (isSummary && !string.IsNullOrWhiteSpace(summary))
                    {
                        summary = Regex.Replace(summary, @"<\/*[^<>]*>", "", RegexOptions.IgnoreCase);
                        if (summary.Length > 200)
                        {
                            summary = summary.Substring(0, 200);
                            summary += "...";
                        }
                    }
                    result.Add(new BlogInfo
                    {
                        Id = b.Id,
                        Title = b.Title,
                        Content = summary,
                        Active = b.Active,
                        UserId = b.Author.Id,
                        UserName = b.Author.Name,
                        CategoryId = b.Category.Id,
                        CategoryName = b.Category.Name,
                        ImgUrl = b.ImgUrl,
                        Date = b.CreateTime
                    });
                });
                return result;
            }
            catch
            {
                throw new InternalException("DB error occurred when getting blogs.");
            }
        }

        public async Task<int> GetCount()
        {
            try
            {
                return await this.blogRepository.ReadCount();
            }
            catch
            {
                throw new InternalException("DB error occurred when getting blog count.");
            }
        }
    }
}