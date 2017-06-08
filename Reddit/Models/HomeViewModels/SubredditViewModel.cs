using System.Collections.Generic;

namespace Reddit.Models.HomeViewModels
{
    public class SubredditViewModel
    {
        public IEnumerable<Post> Posts { get; set; }

        public Subreddit Subreddit { get; set; }
    }
}
