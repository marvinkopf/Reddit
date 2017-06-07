using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Reddit.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public ICollection<User_X_Post_Upvoted> UpvotedPosts { get; set; }

        public ICollection<User_X_Post_Downvoted> DownvotedPosts { get; set; }

        public ICollection<User_X_Comment_Upvoted> UpvotedComments { get; set; }

        public ICollection<User_X_Comment_Downvoted> DownvotedComments { get; set; }

        [JsonIgnore]
        public ICollection<Comment> CreatedComments { get; set; }

        [JsonIgnore]
        public ICollection<Post> CreatedPosts { get; set; }

        [JsonIgnore]
        public ICollection<User_X_Subreddit_Subscription> Subscriptions { get; set; }
    }
}
