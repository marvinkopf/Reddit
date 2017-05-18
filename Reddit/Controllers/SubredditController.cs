using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Reddit.Data;
using Reddit.Models;

namespace Reddit.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class SubredditController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly UserManager<ApplicationUser> _manager;
        
        public SubredditController(ApplicationDbContext context, UserManager<ApplicationUser> manager)
        {
            _context = context;
            _manager = manager;
        }

        [HttpPost]
        public IActionResult Post(string name)
        {
            var subreddit = new Subreddit(name);

            _context.Subreddits.Add(subreddit);
            _context.SaveChanges();
            
            return Ok();
        }
    }
}