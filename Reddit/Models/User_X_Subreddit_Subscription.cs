using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Reddit.Models
{
    public class User_X_Subreddit_Subscription
    {
        public string UserId { get; set; }

        public ApplicationUser User { get; set; }

        public string SubredditName { get; set; }

        public Subreddit Subreddit { get; set; }

        public bool Subscribed { get; set; }
    }
}
