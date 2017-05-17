using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Reddit.Data;

namespace Reddit.Controllers
{
    public class HomeController : Controller
    {
        ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View(_context.Posts
                                    .Include(p => p.Comments)
                                    .Include(p => p.Creator)
                                    .Include(p => p.UpvotedBy)
                                    .Include(p => p.DownvotedBy)
                                    .OrderByDescending(p => p.Created)
                                    .Take(30));
        }

        [HttpGet("r/{sub}")]
        public IActionResult Sub(string sub)
        {
            ViewData["Title"] = sub;
            
            return View("Subreddit", _context.Posts
                                    .Include(p => p.Comments)
                                    .Include(p => p.Creator)
                                    .Include(p => p.UpvotedBy)
                                    .Include(p => p.DownvotedBy)
                                    .Where(p => p.Subreddit == sub)
                                    .OrderByDescending(p => p.Created)
                                    .Take(30));
        }

        [HttpGet("[action]/{id:int}")]
        public IActionResult Post(int id) =>
            View(_context.Posts
                            .Include(p => p.Creator)
                            .Include(p => p.Comments).ThenInclude(c => c.Children)
                            .Include(p => p.Comments).ThenInclude(c => c.UpvotedBy)
                            .Include(p => p.Comments).ThenInclude(c => c.DownvotedBy)
                            .Include(p => p.Comments).ThenInclude(c => c.Creator)
                            .Include(p => p.UpvotedBy)
                            .Include(p => p.DownvotedBy)
                            .First(p => p.PostId == id));

        [Authorize]
        public IActionResult Submit() => View();

        [HttpGet("user/{userName}")]
        public IActionResult UserPage(string userName)
        {
            return View("User",
                _context.Users
                            .Include(u => u.CreatedComments).ThenInclude(c => c.UpvotedBy)
                            .Include(u => u.CreatedComments).ThenInclude(c => c.DownvotedBy)
                            .Include(u => u.CreatedPosts).ThenInclude(p => p.UpvotedBy)
                            .Include(u => u.CreatedPosts).ThenInclude(p => p.DownvotedBy)
                            .Include(u => u.CreatedPosts).ThenInclude(p => p.Comments)
                            .First(u => u.UserName.ToUpper() == userName.ToUpper()));
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
