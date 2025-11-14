using BlogApplication.Application.Interfaces;
using BlogApplication.Core.DTOs.Comment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BlogApplication.API.Controllers {
    [ApiController]
    [Route("api/[controller]")]
    public class CommentsController : ControllerBase {

        private readonly ICommentService _commentService;

        public CommentsController(ICommentService commentService) {
            _commentService = commentService;
        }

        [HttpGet("post/{postId}")]
        public async Task<IActionResult> GetPostComments(Guid id) {
            try {
                var comments = await _commentService.GetPostCommentsAsync(id);
                return Ok(comments);
            } catch (Exception ex) {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateComment([FromBody] CreateCommentDto dto) {
            try {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (string.IsNullOrEmpty(userId)) {
                    return Unauthorized(new { message = "User not authenticated"});
                }

                var comment = await _commentService.CreateCommentAsync(dto, userId);
                return Ok(comment);
            } catch (Exception ex) {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComment(Guid id) {
            try {

                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (string.IsNullOrEmpty(userId)) {
                    return Unauthorized(new { message = "User not authenticated" });
                }

                var result = await _commentService.DeleteCommentAsync(id, userId);

                if (result) {
                    return Ok(new { message = "Comment deleted successfully" });
                }

                return BadRequest(new { message = "Failed to delete comment" });

            } catch (Exception ex) {
                return BadRequest(new { message = ex.Message });
            }
        }

    }
}
