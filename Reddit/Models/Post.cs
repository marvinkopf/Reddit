using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Reddit.Models
{
    public class Post
    {
        // Used by EF
        protected Post() { }

        public Post(string title, string url, string subreddit, DateTime created, string creatorId)
        {
            Title = title;
            Link = url;
            Subreddit = subreddit;
            CreatorId = creatorId;
            Created = created;
        }

        public int PostId { get; set; }

        private string _title;
        public string Title
        {
            get
            {
                return _title;
            }
            set
            {
                Contract.Requires(!String.IsNullOrWhiteSpace(value));
                _title = value;
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

        private string _link;

        public string Link
        {
            get
            {
                return _link;
            }
            set
            {
                Contract.Requires(!String.IsNullOrWhiteSpace(value));
                _link = value;
            }
        }

        public int Score { get; set; }

        [JsonIgnore]
        public ICollection<Comment> Comments { get; set; }

        public ICollection<User_X_Post_Upvoted> UpvotedBy { get; set; }

        public ICollection<User_X_Post_Downvoted> DownvotedBy { get; set; }

        private string _subreddit;

        public string Subreddit
        {
            get
            {
                return _subreddit;
            }
            set
            {
                Contract.Requires(!String.IsNullOrWhiteSpace(value));
                _subreddit = value;
            }
        }
    }
}
