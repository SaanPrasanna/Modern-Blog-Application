using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApplication.Core.Entities {
    public class Comment : BaseEntity {
        public string Content { get; set; } = string.Empty;
        public Guid PostId { get; set; }
        public Post Post { get; set; } = null!;
        public string UserId { get; set; } = string.Empty;
        public User User { get; set; } = null!;
    }
}
