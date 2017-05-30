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
    public class HomeController : Controller
    {
        ApplicationDbContext _context;

        private readonly UserManager<ApplicationUser> _manager;

        public HomeController(ApplicationDbContext context, UserManager<ApplicationUser> manager)
        {
            _context = context;
            _manager = manager;
        }
        public async Task<IActionResult> Index()
        {
            var user = await _manager.GetUserAsync(HttpContext.User);


            if (user != null)
            {
                user = _manager.Users.Include(u => u.Subscriptions).FirstOrDefault(u => u.Id == user.Id);

                return View(new Tuple<IEnumerable<Post>, IEnumerable<Subreddit>>(_context.Posts
                                        .Include(p => p.Comments)
                                        .Include(p => p.Creator)
                                        .Include(p => p.UpvotedBy)
                                        .Include(p => p.DownvotedBy)
                                        .Where(p => user
                                            .Subscriptions.Any(
                                                x => x.SubredditName == p.SubredditName
                                                        &&
                                                        x.Subscribed))
                                        .OrderByDescending(p => p.Created)
                                        .Take(30), _context.Subreddits.Include(s => s.SubscribedUsers)));
            }
            else
            {
                return View(new Tuple<IEnumerable<Post>, IEnumerable<Subreddit>>(_context.Posts
                                        .Include(p => p.Comments)
                                        .Include(p => p.Creator)
                                        .Include(p => p.UpvotedBy)
                                        .Include(p => p.DownvotedBy)
                                        .OrderByDescending(p => p.Created)
                                        .Take(30),
                                        _context.Subreddits.Include(s => s.SubscribedUsers)));
            }
        }

        [HttpGet("r/{sub}")]
        public IActionResult Sub(string sub)
        {
            ViewData["Title"] = sub;
            ViewData["Subtitle"] = sub;

            var subreddit = _context.Subreddits
                                    .Include(s => s.SubscribedUsers).FirstOrDefault(s => s.Name == sub);
                                
            if(subreddit == null)
                return NotFound();

            return View("Subreddit", new Tuple<IEnumerable<Post>, Subreddit>(_context.Posts
                                    .Include(p => p.Comments)
                                    .Include(p => p.Creator)
                                    .Include(p => p.UpvotedBy)
                                    .Include(p => p.DownvotedBy)
                                    .Where(p => p.SubredditName == sub)
                                    .OrderByDescending(p => p.Created)
                                    .Take(30),
                                    subreddit));
        }

        [HttpGetAttribute("[action]")]
        public IActionResult CreateSub() => View();

        [HttpGet("[action]/{id:int}")]
        public IActionResult Post(int id) =>
            View(_context.Posts
                            .Include(p => p.Creator)
                            .Include(p => p.Comments).ThenInclude(c => c.Children)
                            .Include(p => p.Comments).ThenInclude(c => c.UpvotedBy)
                            .Include(p => p.Comments).ThenInclude(c => c.DownvotedBy)
                            .Include(p => p.Comments).ThenInclude(c => c.Creator)
                            .Include(p => p.Subreddit).ThenInclude(s => s.SubscribedUsers)
                            .Include(p => p.UpvotedBy)
                            .Include(p => p.DownvotedBy)
                            .First(p => p.PostId == id));

        [Authorize]
        public IActionResult Submit()
        {
            ViewData["Subtitle"] = "Submit";
            return View();
        }

        [HttpGet("user/{userName}")]
        public IActionResult UserPage(string userName)
        {
            ViewData["Subtitle"] = userName;
            return View("User",
                _context.Users
                            .Include(u => u.CreatedComments).ThenInclude(c => c.UpvotedBy)
                            .Include(u => u.CreatedComments).ThenInclude(c => c.DownvotedBy)
                            .Include(u => u.CreatedPosts).ThenInclude(p => p.UpvotedBy)
                            .Include(u => u.CreatedPosts).ThenInclude(p => p.DownvotedBy)
                            .Include(u => u.CreatedPosts).ThenInclude(p => p.Comments)
                            .First(u => u.UserName.ToUpper() == userName.ToUpper()));
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
