using BlogApplication.Application.Interfaces;
using BlogApplication.Core.DTOs.Reaction;
using BlogApplication.Core.Entities;
using BlogApplication.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApplication.Application.Services {
    public class ReactionService: IReactionService {

        private readonly ApplicationDbContext _context;

        public ReactionService(ApplicationDbContext context) {
            _context = context;
        }


        public async Task<ReactionResponseDto> CreateOrUpdateReactionAsync(CreateReactionDto dto, string userId) {
            var post = await _context.Posts.FindAsync(dto.PostId);
            if (post == null) {
                throw new Exception("Post not found");
            }

            var existingReactoin = await _context.Reactions.FirstOrDefaultAsync(r => r.PostId == dto.PostId && r.UserId == userId);
            if (existingReactoin != null) {
                // Update existing reaction
                existingReactoin.Type = dto.Type;
                existingReactoin.UpdatedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();

                var user = await _context.Users.FindAsync(userId);
                return new ReactionResponseDto {
                    Id = existingReactoin.Id,
                    Type = existingReactoin.Type,
                    UserName = user?.UserName ?? "Unknown",
                    CreatedAt = existingReactoin.CreatedAt
                };
            } else {
                // Create new reaction
                var reaction = new Reaction {
                    PostId = dto.PostId,
                    UserId = userId,
                    Type = dto.Type,
                    CreatedAt = DateTime.UtcNow
                };

                _context.Reactions.Add(reaction);
                await _context.SaveChangesAsync();

                var user = await _context.Users.FindAsync(userId);
                return new ReactionResponseDto {
                    Id = reaction.Id,
                    Type = reaction.Type,
                    UserName = user?.UserName ?? "Unknown",
                    CreatedAt = reaction.CreatedAt
                };
            }
        }

        public async Task<bool> DeleteReactionAsync(Guid id, string userId) {
            var reaction = await _context.Reactions.FindAsync(id);

            if (reaction == null) {
                throw new Exception("Reaction not found");
            }

            _context.Reactions.Remove(reaction);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<ReactionSummaryDto> GetPostReactionSummaryAsync(Guid postId, string? userId = null) {
            var reactions = await _context.Reactions.Where(r => r.PostId == postId).ToListAsync();

            var summary = new ReactionSummaryDto {
                LikeCount = reactions.Count(r => r.Type == ReactionType.Like),
                LoveCount = reactions.Count(r => r.Type == ReactionType.Love),
                HahaCount = reactions.Count(r => r.Type == ReactionType.Haha),
                SadCount = reactions.Count(r => r.Type == ReactionType.Sad),
                WowCount = reactions.Count(r => r.Type == ReactionType.Wow),
                AngryCount = reactions.Count(r => r.Type == ReactionType.Angry),
                TotalCount = reactions.Count
            };

            if (!string.IsNullOrEmpty(userId)) {
                var userReaction = reactions.FirstOrDefault(r => r.UserId == userId);
                summary.UserReaction = userReaction?.Type;
            }

            return summary;
        }
    }
}
