using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApplication.Core.DTOs.Comment {
    public class CreateCommentDto {
        public string Content { get; set; } = string.Empty;
        public Guid PostId { get; set; }
    }

    public class CommentResponseDto {
        public Guid Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}
