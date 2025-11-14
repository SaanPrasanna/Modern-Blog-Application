using BlogApplication.Core.DTOs.Reaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApplication.Application.Interfaces {
    public interface IReactionService {
        Task<ReactionResponseDto> CreateOrUpdateReactionAsync(CreateReactionDto dto, string userId);
        Task<bool> DeleteReactionAsync(Guid id, string userId);
        Task<ReactionSummaryDto> GetPostReactionSummaryAsync(Guid postId, string? userId);
    }
}
