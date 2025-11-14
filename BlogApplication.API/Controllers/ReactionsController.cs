using BlogApplication.Application.Interfaces;
using BlogApplication.Core.DTOs.Reaction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BlogApplication.API.Controllers {
    [ApiController]
    [Route("api/[controller]")]
    public class ReactionsController : ControllerBase {

        private readonly IReactionService _reactionService;

        public ReactionsController(IReactionService reactionService) {
            _reactionService = reactionService;
        }

        [HttpGet("post/{postId}/summary")]
        public async Task<IActionResult> GetPostReactionSummary(Guid id) {
            try {

                var userId = User.FindFirst(ClaimTypes.NameIdentifier);
                var summary = await _reactionService.GetPostReactionSummaryAsync(id, userId?.Value);
                return Ok(summary);
            } catch (Exception ex) {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateOrUpdateReaction([FromBody] CreateReactionDto dto) {
            try {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId)) {
                    return Unauthorized(new { message = "User not authenticated" });
                }

                var reaction = await _reactionService.CreateOrUpdateReactionAsync(dto, userId);
                return Ok(reaction);

            } catch (Exception ex) {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize]
        [HttpDelete("post/{postId}")]
        public async Task<IActionResult> DeleteReaction(Guid postId) {
            try {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId)) {
                    return Unauthorized(new { message = "User not authenticated" });
                }

                var result = await _reactionService.DeleteReactionAsync(postId, userId);
                if (result) {
                    return Ok(new { message = "Reaction deleted successfully" });
                }

                return BadRequest(new { message = "Failed to delete reaction" });

            } catch (Exception ex) {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
