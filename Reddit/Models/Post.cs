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

        public Post(string title, string url, string subredditName, DateTime created, string creatorId)
        {
            Title = title;
            Link = url;
            SubredditName = subredditName;
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
                if (String.IsNullOrWhiteSpace(value)) throw new Exception();
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
                if (value == null) throw new Exception();
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
                if (String.IsNullOrWhiteSpace(value)) throw new Exception();
                _link = value;
            }
        }

        public int Score { get; set; }

        [JsonIgnore]
        public ICollection<Comment> Comments { get; set; }

        public ICollection<User_X_Post_Upvoted> UpvotedBy { get; set; }

        public ICollection<User_X_Post_Downvoted> DownvotedBy { get; set; }

        private string _subredditName;

        public string SubredditName
        {
            get
            {
                return _subredditName;
            }
            set
            {
                if (String.IsNullOrWhiteSpace(value)) throw new Exception();
                _subredditName = value;
            }
        }

        public Subreddit Subreddit { get; set; }

        public string UrlToImage { get; set; }
    }
}
