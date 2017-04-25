using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Reddit.Models
{
    public class Comment
    {
        public int CommentId { get; set; }

        public string Txt { get; set; }

        public DateTime Created { get; set; }

        public ApplicationUser Creator { get; set; }

        public Comment Parent { get; set; }

        public int? ParentId { get; set; }

        [JsonIgnore]
        public ICollection<Comment> Children { get; set; }

        public int PostId { get; set; }

        public Post Post { get; set; }

        public int Score { get; set; }

        public ICollection<User_X_Comment_Upvoted> UpvotedBy { get; set; }

        public ICollection<User_X_Comment_Downvoted> DownvotedBy { get; set; }
    }
}
