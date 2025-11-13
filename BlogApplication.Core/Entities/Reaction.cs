using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApplication.Core.Entities {
    public enum ReactionType {
        Like,
        Love,
        Haha,
        Wow,
        Sad,
        Angry
    }
    public class Reaction : BaseEntity {
        public ReactionType Type { get; set; }
        public Guid PostId { get; set; }
        public Post Post { get; set; } = null!;
        public string UserId { get; set; } = string.Empty;
        public User User { get; set; } = null!;
    }
}
