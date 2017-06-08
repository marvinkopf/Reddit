using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Reddit.Models
{
    public class Subreddit
    {
        // Used by EF
        protected Subreddit() { }

        public Subreddit(string name)
        {
            Name = name;
        }

        private string _name;

        [Key]
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                if (String.IsNullOrWhiteSpace(value)) throw new Exception();
                _name = value;
            }
        }

        public string SidebarText { get; set; }

        public bool IsPrivate { get; set; }

        [JsonIgnore]
        public ICollection<User_X_Subreddit_Subscription> SubscribedUsers { get; set; }

        [JsonIgnore]
        public ICollection<User_X_Subreddit_Moderator> Moderators { get; set; }

        [JsonIgnore]
        public ICollection<User_X_Subreddit_CanView> PrivateViewers { get; set; }
    }
}
