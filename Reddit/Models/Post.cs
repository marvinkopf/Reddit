using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Reddit.Models
{
    public class Post
    {
        public int PostId { get; set; }

        public string Title { get; set; }

        public DateTime Created { get; set; }

        public ApplicationUser Creator { get; set; }

        public string Link { get; set; }

        public int Score { get; set; }

        public ICollection<Comment> Comments { get; set; }
    }
}
