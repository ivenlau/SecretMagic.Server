using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SecretMagic.API.Authorization;
using SecretMagic.API.Commom;
using SecretMagic.API.Services;
using SecretMagic.Model;

namespace SecretMagic.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BlogController : ControllerBase
    {
        private readonly ILogger<BlogController> logger;
        private readonly IBlogService blogService;

        public BlogController(ILogger<BlogController> logger, IBlogService blogService)
        {
            this.logger = logger;
            this.blogService = blogService;
        }

        [HttpGet("AllBlogs")]
        public ActionResult<IEnumerable<Blog>> AllBlogs()
        {
            try
            {
                return Ok(blogService.GetAllBlogs());
            }
            catch (InternalException ie)
            {
                logger.LogError(500, ie, ie.Message, null);
                return NotFound(ie.Message);
            }
            catch (BadRequestException be)
            {
                logger.LogError(404, be, be.Message, null);
                return BadRequest(be.Message);
            }
        }

        [HttpGet("GetCount")]
        public async Task<ActionResult<int>> GetCount()
        {
            try
            {
                var count = await blogService.GetCount();
                return Ok(count);
            }
            catch (InternalException ie)
            {
                logger.LogError(500, ie, ie.Message, null);
                return NotFound(ie.Message);
            }
            catch (BadRequestException be)
            {
                logger.LogError(404, be, be.Message, null);
                return BadRequest(be.Message);
            }
        }

        [HttpGet("GetBlogs")]
        public ActionResult<IEnumerable<Blog>> GetBlogs(
            [FromQuery]int index,
            [FromQuery]int size,
            [FromQuery]string query,
            [FromQuery]string category)
        {
            try
            {
                var pageIndex = index > 0 ? index : 1;
                var pageSize = index > 0 ? size : 10;
                var title = string.IsNullOrWhiteSpace(query) ? null : query;
                Guid? categoryId = null;
                if (!string.IsNullOrWhiteSpace(category))
                {
                    categoryId = new Guid(category);
                }
                var result = blogService.GetBlogs(pageIndex, pageSize, title, categoryId);
                return Ok(result);
            }
            catch (InternalException ie)
            {
                logger.LogError(500, ie, ie.Message, null);
                return NotFound(ie.Message);
            }
            catch (BadRequestException be)
            {
                logger.LogError(404, be, be.Message, null);
                return BadRequest(be.Message);
            }
        }

        [HttpGet("GetDetailBlogs")]
        [Protected("Blog Management")]
        public ActionResult<IEnumerable<Blog>> GetDetailBlogs(
            [FromQuery]int index,
            [FromQuery]int size,
            [FromQuery]string query,
            [FromQuery]string category)
        {
            try
            {
                var pageIndex = index > 0 ? index : 1;
                var pageSize = index > 0 ? size : 10;
                var title = string.IsNullOrWhiteSpace(query) ? null : query;
                Guid? categoryId = null;
                if (!string.IsNullOrWhiteSpace(category))
                {
                    categoryId = new Guid(category);
                }
                var result = blogService.GetBlogs(pageIndex, pageSize, title, categoryId, false);
                return Ok(result);
            }
            catch (InternalException ie)
            {
                logger.LogError(500, ie, ie.Message, null);
                return NotFound(ie.Message);
            }
            catch (BadRequestException be)
            {
                logger.LogError(404, be, be.Message, null);
                return BadRequest(be.Message);
            }
        }

        [HttpGet("CategoryBlogs/{id}")]
        public ActionResult<IEnumerable<Blog>> CategoryBlogs(Guid id)
        {
            try
            {
                return Ok(blogService.GetBlogsByCategoryId(id));
            }
            catch (InternalException ie)
            {
                logger.LogError(500, ie, ie.Message, null);
                return NotFound(ie.Message);
            }
            catch (BadRequestException be)
            {
                logger.LogError(404, be, be.Message, null);
                return BadRequest(be.Message);
            }
        }

        [HttpGet("BlogDetail/{id}")]
        public ActionResult<BlogInfo> BlogDetail(Guid id)
        {
            try
            {
                return Ok(blogService.GetBlogById(id));
            }
            catch (InternalException ie)
            {
                logger.LogError(500, ie, ie.Message, null);
                return NotFound(ie.Message);
            }
            catch (BadRequestException be)
            {
                logger.LogError(404, be, be.Message, null);
                return BadRequest(be.Message);
            }
        }

        [HttpPost("NewBlog")]
        [Protected("Blog Management")]
        public async Task<IActionResult> NewBlog(BlogInfo blog)
        {
            try
            {
                var result = await blogService.AddBlog(blog);
                var blogInfo = new BlogInfo { Id = result.Id };
                return Ok(blogInfo);
            }
            catch (InternalException ie)
            {
                logger.LogError(500, ie, ie.Message, null);
                return NotFound(ie.Message);
            }
            catch (BadRequestException be)
            {
                logger.LogError(404, be, be.Message, null);
                return BadRequest(be.Message);
            }
        }

        [HttpPut("UpdateBlog")]
        [Protected("Blog Management")]
        public async Task<IActionResult> UpdateBlog(BlogInfo blog)
        {
            if (blog == null)
            {
                return BadRequest();
            }
            try
            {
                await blogService.UpdateBlog(blog);
                return Ok();
            }
            catch (InternalException ie)
            {
                logger.LogError(500, ie, ie.Message, null);
                return NotFound(ie.Message);
            }
            catch (BadRequestException be)
            {
                logger.LogError(404, be, be.Message, null);
                return BadRequest(be.Message);
            }
        }

        [HttpDelete("RemoveBlog/{id}")]
        [Protected("Blog Management")]
        public async Task<IActionResult> RemoveBlog(Guid id)
        {
            try
            {
                await blogService.DeleteBlog(id);
                return Ok();
            }
            catch (InternalException ie)
            {
                logger.LogError(500, ie, ie.Message, null);
                return NotFound(ie.Message);
            }
            catch (BadRequestException be)
            {
                logger.LogError(404, be, be.Message, null);
                return BadRequest(be.Message);
            }
        }

        [HttpGet("AllCategories")]
        public ActionResult<IEnumerable<Category>> AllCategories()
        {
            try
            {
                return Ok(blogService.GetAllCategories());
            }
            catch (InternalException ie)
            {
                logger.LogError(500, ie, ie.Message, null);
                return NotFound(ie.Message);
            }
            catch (BadRequestException be)
            {
                logger.LogError(404, be, be.Message, null);
                return BadRequest(be.Message);
            }
        }

        [HttpPost("NewCategory")]
        [Protected("Blog Category")]
        public async Task<IActionResult> NewCategory(Category category)
        {
            try
            {
                var result = await blogService.AddCategory(category);
                var blogInfo = new BlogInfo { Id = result.Id };
                return Ok(blogInfo);
            }
            catch (InternalException ie)
            {
                logger.LogError(500, ie, ie.Message, null);
                return NotFound(ie.Message);
            }
            catch (BadRequestException be)
            {
                logger.LogError(404, be, be.Message, null);
                return BadRequest(be.Message);
            }
        }

        [HttpDelete("RemoveCategory/{id}")]
        [Protected("Blog Category")]
        public async Task<IActionResult> RemoveCategory(Guid id)
        {
            try
            {
                await blogService.DeleteCategory(id);
                return Ok();
            }
            catch (InternalException ie)
            {
                logger.LogError(500, ie, ie.Message, null);
                return NotFound(ie.Message);
            }
            catch (BadRequestException be)
            {
                logger.LogError(404, be, be.Message, null);
                return BadRequest(be.Message);
            }
        }
    }
}