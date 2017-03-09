using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Reddit.Models
{
    public class SubmitViewModel
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Link { get; set; }
    }
}
    