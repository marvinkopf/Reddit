namespace Reddit.Models
{
    public class User_X_Post_Downvoted
    {
        public string UserId { get; set; }

        public ApplicationUser User { get; set; }

        public int PostId { get; set; }

        public Post Post { get; set; }

        public bool Downvoted { get; set; }
    }
}
