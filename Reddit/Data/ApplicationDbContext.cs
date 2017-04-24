using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Reddit.Models;

namespace Reddit.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);

            builder.Entity<Comment>()
                    .HasOne(c => c.Parent)
                    .WithMany(c => c.Children);

            builder.Entity<User_X_Post_Upvoted>()
                    .HasKey(x => new { x.UserId, x.PostId });

            builder.Entity<User_X_Post_Upvoted>()
                    .HasOne<Post>(x => x.Post)
                    .WithMany(p => p.UpvotedBy);

            builder.Entity<User_X_Post_Upvoted>()
                    .HasOne<ApplicationUser>(x => x.User)
                    .WithMany(u => u.UpvotedPosts);

            builder.Entity<User_X_Post_Downvoted>()
                    .HasKey(x => new { x.UserId, x.PostId });

            builder.Entity<User_X_Post_Downvoted>()
                    .HasOne<Post>(x => x.Post)
                    .WithMany(p => p.DownvotedBy);

            builder.Entity<User_X_Post_Downvoted>()
                    .HasOne<ApplicationUser>(x => x.User)
                    .WithMany(u => u.DownvotedPosts);
        }

        public DbSet<Post> Posts { get; set; }

        public DbSet<Comment> Comments { get; set; }

        public DbSet<User_X_Post_Upvoted> User_X_Post_Upvoted { get; set; }

        public DbSet<User_X_Post_Downvoted> User_X_Post_Downvoted { get; set; }
    }
}
