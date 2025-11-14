using BlogApplication.Application.Interfaces;
using BlogApplication.Core.DTOs.Post;
using BlogApplication.Core.Entities;
using BlogApplication.Core.Interfaces;
using BlogApplication.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BlogApplication.Application.Services {
    public class PostService : IPostService {
        private readonly IRepository<Post> _postRepository;
        private readonly ApplicationDbContext _context;

        public PostService(IRepository<Post> postRepository, ApplicationDbContext context) {
            _postRepository = postRepository;
            _context = context;
        }

        public async Task<PostResponseDto> CreatePostAsync(CreatePostDto dto, string userId) {

            var post = new Post {
                Title = dto.Title,
                Content = dto.Content,
                ImageUrl = dto.ImageUrl,
                AuthorId = userId
            };

            var createPost = await _postRepository.AddAsync(post);

            var user = await _context.Users.FindAsync(userId);

            return new PostResponseDto {
                Title = dto.Title,
                Content = dto.Content,
                ImageUrl = dto.ImageUrl,
                AuthorId = userId,
                AuthorName = user?.FullName ?? "Unknown",
                CreatedAt = DateTime.UtcNow,
                CommentsCount = 0,
                ReactionsCount = 0
            };
        }

        public async Task<PostResponseDto> UpdatePostAsync(Guid id, UpdatePostDto dto, string userId) {
            var post = await _postRepository.GetByIdAsync(id);

            if (post == null) {
                throw new Exception("Post not found");
            }

            if (post.AuthorId != userId) {
                throw new Exception("Unuthorized to update this post");
            }

            post.Title = dto.Title;
            post.Content = dto.Content;
            post.ImageUrl = dto.ImageUrl;

            await _postRepository.UpdateAsync(post);

            var user = await _context.Users.FindAsync(userId);

            return new PostResponseDto {
                Id = post.Id,
                Title = post.Title,
                Content = post.Content,
                ImageUrl = post.ImageUrl,
                AuthorId = userId,
                AuthorName = user?.FullName ?? "Unknown",
                CreatedAt = post.CreatedAt,
                CommentsCount = post.Comments.Count,
                ReactionsCount = post.Reactions.Count
            };

        }

        public async Task<bool> DeletePostAsync(Guid id, string userId) {
            var post = await _postRepository.GetByIdAsync(id);

            if (post == null) {
                throw new Exception("Post not found");
            }

            if (post.AuthorId != userId) {
                throw new Exception("Unuthorized to delete this post");
            }

            await _postRepository.DeleteAsync(post);
            return true;
        }

        public async Task<PostResponseDto> GetPostByIdAsync(Guid id) {
            //var post = await _postRepository.GetByIdAsync(id);
            var post = await _context.Posts
                .Include(p => p.Author)
                .Include(p => p.Comments)
                .Include(p => p.Reactions)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (post == null) {
                throw new Exception("Post not found");
            }

            return new PostResponseDto {
                Id = post.Id,
                Title = post.Title,
                Content = post.Content,
                ImageUrl = post.ImageUrl,
                AuthorId = post.AuthorId,
                AuthorName = post.Author?.FullName ?? "Unknown",
                CreatedAt = post.CreatedAt,
                CommentsCount = post.Comments.Count,
                ReactionsCount = post.Reactions.Count
            };

        }

        public async Task<IEnumerable<PostResponseDto>> GetAllPostsAsync() {
            var post = await _context.Posts
                .Include(p => p.Author)
                .Include(p => p.Comments)
                .Include(p => p.Reactions)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();

            return post.Select(post => new PostResponseDto {
                Id = post.Id,
                Title = post.Title,
                Content = post.Content,
                ImageUrl = post.ImageUrl,
                AuthorId = post.AuthorId,
                AuthorName = post.Author?.FullName ?? "Unknown",
                CreatedAt = post.CreatedAt,
                CommentsCount = post.Comments.Count,
                ReactionsCount = post.Reactions.Count
            });
        }

        public async Task<IEnumerable<PostResponseDto>> GetUserPostsAsync(string userId) {
            var post = await _context.Posts
                .Include(p => p.Author)
                .Include(p => p.Comments)
                .Include(p => p.Reactions)
                .Where(p => p.AuthorId == userId)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();

            return post.Select(post => new PostResponseDto {
                Id = post.Id,
                Title = post.Title,
                Content = post.Content,
                ImageUrl = post.ImageUrl,
                AuthorId = post.AuthorId,
                AuthorName = post.Author?.FullName ?? "Unknown",
                CreatedAt = post.CreatedAt,
                CommentsCount = post.Comments.Count,
                ReactionsCount = post.Reactions.Count
            });
        }

    }
}
