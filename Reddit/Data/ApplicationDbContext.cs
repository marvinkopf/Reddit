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

                builder.Entity<ApplicationUser>()
                        .HasMany<Comment>(u => u.CreatedComments)
                        .WithOne(c => c.Creator);

                builder.Entity<ApplicationUser>()
                        .HasMany<Post>(u => u.CreatedPosts)
                        .WithOne(p => p.Creator);

                builder.Entity<Post>()
                        .Property(p => p.SubredditName).HasColumnName("subreddit");
        }

        public DbSet<Post> Posts { get; set; }

        public DbSet<Comment> Comments { get; set; }

        public DbSet<User_X_Post_Upvoted> User_X_Post_Upvoted { get; set; }

        public DbSet<User_X_Post_Downvoted> User_X_Post_Downvoted { get; set; }

        public DbSet<User_X_Comment_Upvoted> User_X_Comment_Upvoted { get; set; }

        public DbSet<User_X_Comment_Downvoted> User_X_Comment_Downvoted { get; set; }

        public DbSet<User_X_Subreddit_Subscription> User_X_Subreddit_Subscription { get; set; }

        public DbSet<User_X_Subreddit_Moderator> User_X_Subreddit_Moderator { get; set; }

        public DbSet<Subreddit> Subreddits { get; set; }
    }
}
