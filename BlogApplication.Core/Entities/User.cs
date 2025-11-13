using Microsoft.AspNetCore.Identity;
using System.Xml.Linq;

namespace BlogApplication.Core.Entities {
    public class User : IdentityUser {
        public string? FullName { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public ICollection<Post> Posts { get; set; } = new List<Post>();
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public ICollection<Reaction> Reactions { get; set; } = new List<Reaction>();
    }
}