using BlogApplication.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApplication.Core.DTOs.Reaction {
    public class CreateReactionDto {
        public Guid PostId { get; set; }
        public ReactionType Type { get; set; }
    }

    public class  ReactionResponseDto {
        public Guid Id { get; set; }
        public ReactionType Type { get; set; }
        public string UserName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }

    public class ReactionSummaryDto {
        public int LikeCount { get; set; }
        public int LoveCount { get; set; }
        public int HahaCount { get; set; }
        public int SadCount { get; set; }
        public int WowCount { get; set; }
        public int AngryCount { get; set; }
        public int TotalCount { get; set; }
        public ReactionType? UserReaction { get; set; }
    }
}
