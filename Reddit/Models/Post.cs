using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Reddit.Models
{
    public class Post
    {
        public int PostId { get; set; }

        public string Title { get; set; }

        public DateTime Created { get; set; }

        public ApplicationUser Creator { get; set; }

        public string Link { get; set; }

        public int Score { get; set; }

        [JsonIgnore]
        public ICollection<Comment> Comments { get; set; }

        public ICollection<User_X_Post_Upvoted> UpvotedBy { get; set; }

        public ICollection<User_X_Post_Downvoted> DownvotedBy { get; set; }

        public string Subreddit { get; set; }
    }
}
