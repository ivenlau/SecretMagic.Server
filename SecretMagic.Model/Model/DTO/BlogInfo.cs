using System;

namespace SecretMagic.Model
{
    public class BlogInfo
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid? Previous {get; set; }

        public Guid? Next {get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public bool Active { get; set; }

        public Guid UserId { get; set; }

        public string UserName { get; set; }

        public Guid CategoryId { get; set; }

        public string CategoryName { get; set; }

        public string ImgUrl { get; set; }

        public DateTime? Date { get; set; }
    }
}