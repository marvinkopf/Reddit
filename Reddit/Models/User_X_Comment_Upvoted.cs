namespace Reddit.Models
{
    public class User_X_Comment_Upvoted
    {
        public int User_X_Comment_UpvotedId { get; set; }

        public string UserId { get; set; }

        public ApplicationUser User { get; set; }

        public int CommentId { get; set; }

        public Comment Comment { get; set; }
    }
}
