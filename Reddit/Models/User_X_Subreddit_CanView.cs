namespace Reddit.Models
{
    public class User_X_Subreddit_CanView
    {
        public string UserId { get; set; }

        public ApplicationUser User { get; set; }

        public string SubredditName { get; set; }

        public Subreddit Subreddit { get; set; }

        public bool CanView { get; set; }
    }
}
