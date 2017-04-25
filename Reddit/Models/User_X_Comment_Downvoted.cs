using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Reddit.Models
{
    public class User_X_Comment_Downvoted
    {
        public string UserId { get; set; }

        public ApplicationUser User { get; set; }

        public int CommentId { get; set; }

        public Comment Comment { get; set; }

        public bool Downvoted { get; set; }
    }
}
