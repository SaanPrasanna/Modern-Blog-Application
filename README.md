# 📝 Modern Blog Application API

A feature-rich modern blogging application built with .NET 8, featuring a clean architecture, user authentication, post management, comment and reaction functionality. Includes best practices for web development, database integration, and API design.

![Not-By-AI](https://github.com/SaanPrasanna/Modern-Blog-Application/raw/refs/heads/master/Postman-Collection/Written-By-Human-Not-By-AI-Badge-white.svg)
## 🚀 Features

- ✅ **User Authentication & Authorization**
  - User registration with email validation
  - JWT-based authentication
  - Password reset functionality
  
- ✅ **Blog Post Management**
  - Create, read, update, and delete posts
  - Rich text content support
  - Image URL support for post thumbnails
  - Author attribution
  
- ✅ **Comment System**
  - Add comments to posts
  - Delete own comments
  - View all comments on a post
  
- ✅ **Reaction System**
  - React to posts (Like, Love, Angry, Sad, Haha, Wow)
  - Update existing reactions
  - Remove reactions
  - View reaction summary per post

## 🏗️ Architecture

This project follows **Clean Architecture** with clear separation of concerns:

```
BlogApplication/
├── BlogApplication.API/            # Presentation Layer (Controllers, Middleware)
├── BlogApplication.Application/    # Application Layer (Services, Business Logic)
├── BlogApplication.Core/           # Domain Layer (Entities, DTOs, Interfaces)
└── BlogApplication.Infrastructure/ # Infrastructure Layer (Data Access, Repositories)
```

### Project Structure

```
📁 BlogApplication
│
├── 📁 BlogApplication.API
│   ├── Controllers/
│   │   ├── AuthController.cs
│   │   ├── PostsController.cs
│   │   ├── CommentsController.cs
│   │   └── ReactionsController.cs
│   ├── Program.cs
│   └── appsettings.json
│
├── 📁 BlogApplication.Core
│   ├── Entities/
│   │   ├── BaseEntity.cs
│   │   ├── User.cs
│   │   ├── Post.cs
│   │   ├── Comment.cs
│   │   └── Reaction.cs
│   ├── DTOs/
│   │   ├── Auth/
│   │   ├── Post/
│   │   ├── Comment/
│   │   └── Reaction/
│   └── Interfaces/
│       └── IRepository.cs
│
├── 📁 BlogApplication.Application
│   ├── Services/
│   │   ├── AuthService.cs
│   │   ├── PostService.cs
│   │   ├── CommentService.cs
│   │   └── ReactionService.cs
│   └── Interfaces/
│       ├── IAuthService.cs
│       ├── IPostService.cs
│       ├── ICommentService.cs
│       └── IReactionService.cs
│
└── 📁 BlogApplication.Infrastructure
    ├── Data/
    │   └── ApplicationDbContext.cs
    └── Repositories/
        └── Repository.cs

```

## 🛠️ Technologies Used

- **Framework**: ASP.NET Core 8.0
- **ORM**: Entity Framework Core 8.0
- **Database**: SQL Server (LocalDB)
- **Authentication**: JWT Bearer Tokens
- **Identity**: ASP.NET Core Identity
- **API Documentation**: Swagger/OpenAPI

## 📦 NuGet Packages

### API Project
- Microsoft.AspNetCore.Authentication.JwtBearer (8.0.11)
- Microsoft.AspNetCore.Identity.EntityFrameworkCore (8.0.11)
- Microsoft.EntityFrameworkCore.Design (8.0.11)
- Swashbuckle.AspNetCore (6.x)

### Application Project
- AutoMapper.Extensions.Microsoft.DependencyInjection (13.0.1)
- FluentValidation.AspNetCore (11.3.0)

### Infrastructure Project
- Microsoft.EntityFrameworkCore.SqlServer (8.0.11)
- Microsoft.EntityFrameworkCore.Tools (8.0.11)
- Microsoft.AspNetCore.Identity.EntityFrameworkCore (8.0.11)

## 🚀 Getting Started

### 1. Clone the Repository

```bash
git clone https://github.com/SaanPrasanna/Modern-Blog-Application.git
cd Modern-Blog-Application
```

### 2. Update Connection String

Open `BlogApplication.API/appsettings.json` and update the connection string if needed:

```json
{
  "ConnectionStrings": {
      "DefaultConnection": "Server=RGenesis\\SQLEXPRESS;Database=BlogApplicationDb;Trusted_Connection=True;TrustServerCertificate=True;Encrypt=Mandatory;MultipleActiveResultSets=true"
  }
}
```

### 3. Update JWT Settings

In `appsettings.json`, update the JWT settings:

```json
{
  "Jwt": {
    "Key": "SuperSecretKeyThatIsAtLeast32CharactersLong!",
    "Issuer": "BlogApplication",
    "Audience": "BlogApplicationUsers"
  }
}
```

### 4. Apply Database Migrations

Open **Package Manager Console** in Visual Studio:
- Go to `Tools` → `NuGet Package Manager` → `Package Manager Console`
- Set **Default Project** to `BlogApplication.Infrastructure`

Run:
```powershell
Add-Migration InitialCreate -Project BlogApplication.Infrastructure -StartupProject BlogApplication.API
Update-Database -Project BlogApplication.Infrastructure -StartupProject BlogApplication.API
```

### 5. Run the Application

Press `F5` in Visual Studio or run:

```bash
cd BlogApplication.API
dotnet run
```

The API will be available at:
- **HTTPS**: `https://localhost:7xxx`
- **HTTP**: `http://localhost:5xxx`
- **Swagger UI**: `https://localhost:7xxx/swagger`

## 📖 API Documentation

### Authentication Endpoints

#### Register
```http
POST /api/auth/register
Content-Type: application/json

{
  "email": "sandunpmapa@example.com",
  "password": "Password123!",
  "fullName": "Sandun Prasanna"
}
```

**Response:**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "email": "sandunpmapa@example.com",
  "fullName": "Sandun Prasanna"
}
```

#### Login
```http
POST /api/auth/login
Content-Type: application/json

{
  "email": "sandunpmapa@example.com",
  "password": "Password123!"
}
```

#### Forgot Password
```http
POST /api/auth/forgot-password
Content-Type: application/json

"sandunpmapa@example.com"
```

#### Reset Password
```http
POST /api/auth/reset-password
Content-Type: application/json

{
  "email": "sandunpmapa@example.com",
  "token": "reset-token-here",
  "newPassword": "NewPassword123!"
}
```

### Post Endpoints

#### Get All Posts
```http
GET /api/posts
```

#### Get Post by ID
```http
GET /api/posts/{id}
```

#### Get User's Posts
```http
GET /api/posts/user/{userId}
```

#### Create Post (Requires Authentication)
```http
POST /api/posts
Authorization: Bearer {token}
Content-Type: application/json

{
  "title": "My First Blog Post",
  "content": "This is the content of my post...",
  "imageUrl": "https://example.com/image.jpg"
}
```

#### Update Post (Requires Authentication)
```http
PUT /api/posts/{id}
Authorization: Bearer {token}
Content-Type: application/json

{
  "title": "Updated Title",
  "content": "Updated content...",
  "imageUrl": "https://example.com/new-image.jpg"
}
```

#### Delete Post (Requires Authentication)
```http
DELETE /api/posts/{id}
Authorization: Bearer {token}
```

### Comment Endpoints

#### Get Post Comments
```http
GET /api/comments/post/{postId}
```

#### Create Comment (Requires Authentication)
```http
POST /api/comments
Authorization: Bearer {token}
Content-Type: application/json

{
  "content": "Great post!",
  "postId": "post-guid-here"
}
```

#### Delete Comment (Requires Authentication)
```http
DELETE /api/comments/{id}
Authorization: Bearer {token}
```

### Reaction Endpoints

#### Get Post Reaction Summary
```http
GET /api/reactions/post/{postId}/summary
```

**Response:**
```json
{
  "likeCount": 10,
  "loveCount": 5,
  "angryCount": 2,
  "sadCount": 1,
  "wowCount": 1,
  "totalCount": 18,
  "userReaction": 0
}
```

Reaction Types: `0=Like, 1=Love, 2=Angry, 3=Sad`

#### Create/Update Reaction (Requires Authentication)
```http
POST /api/reactions
Authorization: Bearer {token}
Content-Type: application/json

{
  "postId": "post-guid-here",
  "type": 0
}
```

#### Delete Reaction (Requires Authentication)
```http
DELETE /api/reactions/post/{postId}
Authorization: Bearer {token}
```

## 🔐 Authentication Flow

1. **Register** a new user or **Login** with existing credentials
2. Copy the JWT `token` from the response
3. In Swagger UI, click the **"Authorize"** button
4. Enter: `Bearer {your-token-here}`
5. Click **"Authorize"**
6. Now you can access protected endpoints

## 🗄️ Database Schema

### Tables

- **AspNetUsers** - User accounts (ASP.NET Identity)
- **AspNetRoles** - User roles
- **Posts** - Blog posts
- **Comments** - Post comments
- **Reactions** - Post reactions

### Relationships

```
User (1) ──── (M) Posts
User (1) ──── (M) Comments
User (1) ──── (M) Reactions

Post (1) ──── (M) Comments
Post (1) ──── (M) Reactions
```

### Cascade Delete Rules

- Delete **User** → Deletes all their **Posts**
- Delete **Post** → Deletes all its **Comments** and **Reactions**
- Delete **User** → **Does NOT** delete their Comments/Reactions (NoAction)


## 🚨 Common Issues & Solutions

### Issue: "Could not load file or assembly 'System.Runtime'"

**Solution:**
```powershell
# Update all packages to same version
Update-Package -reinstall
```

### Issue: "Multiple cascade paths"

**Solution:** Change `OnDelete` behavior to `NoAction` in `ApplicationDbContext.cs`

### Issue: Migration fails

**Solution:**
```powershell
# Remove migration
Remove-Migration

# Clean solution
# Delete bin and obj folders

# Create migration again
Add-Migration InitialCreate
Update-Database
```

### Issue: JWT token not working

**Solution:**
- Ensure the token is prefixed with `Bearer ` (with space)
- Check token expiration (default: 7 days)
- Verify JWT configuration in `appsettings.json`

## 📚 Learning Resources

- [ASP.NET Core Documentation](https://docs.microsoft.com/aspnet/core)
- [Entity Framework Core](https://docs.microsoft.com/ef/core)
- [Clean Architecture](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)
- [JWT Authentication](https://jwt.io/introduction)

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 👤 Author

**Sandun Prasanna**
- GitHub: [@SaanPrasanna](https://github.com/SaanPrasanna)
- Email: sandunpmapa@gmail.com

## 🙏 Acknowledgments

- ASP.NET Core Team
- Entity Framework Core Team
- Clean Architecture principles by Robert C. Martin

---

**Built with ❤️ using ASP.NET Core 8.0**
