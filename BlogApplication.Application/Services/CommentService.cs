using BlogApplication.Application.Interfaces;
using BlogApplication.Core.DTOs.Comment;
using BlogApplication.Core.Entities;
using BlogApplication.Core.Interfaces;
using BlogApplication.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BlogApplication.Application.Services {
    public class CommentService : ICommentService {
        private readonly IRepository<Comment> _commentRepository;
        private readonly ApplicationDbContext _context;

        public CommentService(IRepository<Comment> commentRepository, ApplicationDbContext context) {
            _commentRepository = commentRepository;
            _context = context;
        }

        public async Task<CommentResponseDto> CreateCommentAsync(CreateCommentDto dto, string userId) {
            var post = await _context.Posts.FindAsync(dto.PostId);
            if (post == null) {
                throw new Exception("Post not found");
            }

            var comment = new Comment {
                Content = dto.Content,
                PostId = dto.PostId,
                UserId = userId
            };

            var createdComment = await _commentRepository.AddAsync(comment);
            var user = await _context.Users.FindAsync(userId);

            return new CommentResponseDto {
                Id = createdComment.Id,
                Content = createdComment.Content,
                UserName = user?.FullName ?? "Unknown",
                CreatedAt = createdComment.CreatedAt
            };
        }

        public async Task<bool> DeleteCommentAsync(Guid id, string userId) {
            var comment = await _commentRepository.GetByIdAsync(id);

            if (comment == null) {
                throw new Exception("Comment not found");
            }

            if (comment.UserId != userId) {
                throw new Exception("Unauthorized to delete this comment");
            }

            await _commentRepository.DeleteAsync(comment);
            return true;
        }

        public async Task<IEnumerable<CommentResponseDto>> GetPostCommentsAsync(Guid postId) {
            var comments = await _context.Comments
                .Include(c => c.User)
                .Where(c => c.PostId == postId)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();

            return comments.Select(c => new CommentResponseDto {
                Id = c.Id,
                Content = c.Content,
                UserName = c.User.FullName ?? "Unknown",
                CreatedAt = c.CreatedAt
            });
        }
    }
}