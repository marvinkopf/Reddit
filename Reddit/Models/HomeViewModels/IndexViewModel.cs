using System.Collections.Generic;

namespace Reddit.Models.HomeViewModels
{
    public class IndexViewModel
    {
        public IEnumerable<Post> Posts { get; set; }

        public IEnumerable<Subreddit> Subreddits { get; set; }
    }
}
