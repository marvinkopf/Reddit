using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Reddit.Models
{
    public class Comment
    {
        // Used by entity framework
        protected Comment() { }

        public Comment(string txt, DateTime created, string creatorId, int postId)
        {
            Txt = txt;
            Created = created;
            CreatorId = creatorId;
            PostId = postId;
        }

        public int CommentId { get; set; }

        private string _txt;
        public string Txt
        {
            get
            {
                return _txt;
            }
            set
            {
                Contract.Requires(!String.IsNullOrWhiteSpace(value));
                _txt = value;
            }
        }

        private DateTime _created;
        public DateTime Created
        {
            get
            {
                return _created;
            }
            set
            {
                Contract.Requires(value != null);
                _created = value;
            }
        }

        public string CreatorId { get; set; }

        public ApplicationUser Creator { get; set; }

        private Comment _parent;
        public Comment Parent
        {
            get
            {
                return _parent;
            }
            set
            {
                Contract.Requires(value?.PostId != PostId);
                _parent = value;
            }
        }

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
