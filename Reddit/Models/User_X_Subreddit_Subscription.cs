namespace Reddit.Models
{
    public class User_X_Subreddit_Subscription
    {
        public int User_X_Subreddit_SubscriptionId { get; set; }

        public string UserId { get; set; }

        public ApplicationUser User { get; set; }

        public string SubredditName { get; set; }

        public Subreddit Subreddit { get; set; }
    }
}
