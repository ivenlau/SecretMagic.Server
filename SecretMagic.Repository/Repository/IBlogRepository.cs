using SecretMagic.Model;
using System;
using System.Collections.Generic;

namespace SecretMagic.Repository
{
    public interface IBlogRepository : ICrudable<Blog>
    {
        IEnumerable<Blog> GetBlogByCategory(Guid categoryId);
    }
}