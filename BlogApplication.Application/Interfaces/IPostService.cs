using BlogApplication.Core.DTOs.Post;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApplication.Application.Interfaces {
    public interface IPostService {
        Task<PostResponseDto> CreatePostAsync(CreatePostDto dto, string userId);
        Task<PostResponseDto> UpdatePostAsync(Guid id, UpdatePostDto dto, string userId);
        Task<bool> DeletePostAsync(Guid id, string userId);
        Task<PostResponseDto> GetPostByIdAsync(Guid id);
        Task<IEnumerable<PostResponseDto>> GetAllPostsAsync();
        Task<IEnumerable<PostResponseDto>> GetUserPostsAsync(string userId);
    }
}
