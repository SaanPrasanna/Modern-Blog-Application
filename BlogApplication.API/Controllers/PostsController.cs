using BlogApplication.Application.Interfaces;
using BlogApplication.Core.DTOs.Post;

//using BlogApplication.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BlogApplication.API.Controllers {
    [ApiController]
    [Route("api/[controller]")]
    public class PostsController : ControllerBase {

        private readonly IPostService _postService;

        public PostsController(IPostService postService) {
            _postService = postService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPosts() {
            var posts = await _postService.GetAllPostsAsync();
            return Ok(posts);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPostById(Guid id) {
            try {
                var post = await _postService.GetPostByIdAsync(id);

                if (post == null) {
                    return NotFound(new { message = "Post not found." });
                }

                return Ok(post);
            } catch (Exception ex) {
                return NotFound(new { message = ex.Message });
            }
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreatePost([FromBody] CreatePostDto dto) {
            try {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId)) {
                    return Unauthorized(new { message = "User to authorized" });
                }

                var post = await _postService.CreatePostAsync(dto, userId);
                return CreatedAtAction(nameof(GetPostById), new { id = post.Id }, post);
            } catch (Exception ex) {
                return BadRequest(new { message = ex.Message });
            }

        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePost(Guid id, [FromBody] UpdatePostDto dto) {
            try {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId)) {
                    return Unauthorized(new { message = "User not authenticated" });
                }

                var post = await _postService.UpdatePostAsync(id, dto, userId);
                return Ok(post);
            } catch (Exception ex) {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePost(Guid id) {
            try {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId)) {
                    return Unauthorized(new { message = "User not authenticated" });
                }

                var result = await _postService.DeletePostAsync(id, userId);
                if (result) {
                    return Ok(new { message = "Post deleted successfully" });
                }

                return BadRequest(new { message = "Failed to delete post"});

            } catch (Exception ex) {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
