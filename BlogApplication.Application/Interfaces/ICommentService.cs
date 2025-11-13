using BlogApplication.Core.DTOs.Comment;

namespace BlogApplication.Application.Interfaces {
    public interface ICommentService {
        Task<CommentResponseDto> CreateCommentAsync(CreateCommentDto dto, string userId);
        Task<bool> DeleteCommentAsync(Guid id, string userId);
        Task<IEnumerable<CommentResponseDto>> GetPostCommentsAsync(Guid postId);
    }
}