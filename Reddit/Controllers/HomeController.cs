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
using Reddit.Models.HomeViewModels;

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
        public async Task<IActionResult> Index(int? after)
        {
            var user = await _manager.GetUserAsync(HttpContext.User);

            if (user != null)
            {
                user = _manager.Users.Include(u => u.Subscriptions).FirstOrDefault(u => u.Id == user.Id);

                return View(new IndexViewModel()
                {
                    Posts = _context.Posts
                                        .Include(p => p.Comments)
                                        .Include(p => p.Creator)
                                        .Include(p => p.UpvotedBy)
                                        .Include(p => p.DownvotedBy)
                                        .Where(p => after == null || p.PostId <
                                            _context
                                            .Posts
                                            .Where(pp => pp.PostId == after)
                                            .Select(pp => pp.PostId)
                                            .SingleOrDefault())
                                        .Where(p => user
                                            .Subscriptions.Any(
                                                x => x.SubredditName == p.SubredditName
                                                        &&
                                                        x.Subscribed))
                                        .OrderByDescending(p => p.Created)
                                        .Take(30),
                    Subreddits = _context.Subreddits.Include(s => s.SubscribedUsers)
                });
            }
            else
            {
                return View(new IndexViewModel()
                {
                    Posts = _context.Posts
                                        .Include(p => p.Comments)
                                        .Include(p => p.Creator)
                                        .Include(p => p.UpvotedBy)
                                        .Include(p => p.DownvotedBy)
                                        .OrderByDescending(p => p.Created)
                                        .Take(30),
                    Subreddits = _context.Subreddits.Include(s => s.SubscribedUsers)
                });
            }
        }

        [HttpGet("r/{sub}")]
        public IActionResult Sub(string sub)
        {
            ViewData["Title"] = sub;
            ViewData["Subtitle"] = sub;

            var subreddit = _context.Subreddits
                                    .Include(s => s.SubscribedUsers).FirstOrDefault(s => s.Name == sub);

            if (subreddit == null)
                return NotFound();

            return View("Subreddit", new SubredditViewModel()
            {
                Posts = _context.Posts
                                    .Include(p => p.Comments)
                                    .Include(p => p.Creator)
                                    .Include(p => p.UpvotedBy)
                                    .Include(p => p.DownvotedBy)
                                    .Where(p => p.SubredditName == sub)
                                    .OrderByDescending(p => p.Created)
                                    .Take(30),
                Subreddit = subreddit
            });
        }

        [HttpGetAttribute("[action]")]
        public IActionResult CreateSub() => View();

        [HttpGet("[action]/{id:int}")]
        public IActionResult Post(int id)
        {
            var post = _context.Posts
                            .Include(p => p.Creator)
                            .Include(p => p.Comments).ThenInclude(c => c.Children)
                            .Include(p => p.Comments).ThenInclude(c => c.UpvotedBy)
                            .Include(p => p.Comments).ThenInclude(c => c.DownvotedBy)
                            .Include(p => p.Comments).ThenInclude(c => c.Creator)
                            .Include(p => p.Subreddit).ThenInclude(s => s.SubscribedUsers)
                            .Include(p => p.UpvotedBy)
                            .Include(p => p.DownvotedBy)
                            .FirstOrDefault(p => p.PostId == id);

            if (post == null)
                return NotFound();

            return View(post);
        }

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
            var user = _context.Users
                            .Include(u => u.CreatedComments).ThenInclude(c => c.UpvotedBy)
                            .Include(u => u.CreatedComments).ThenInclude(c => c.DownvotedBy)
                            .Include(u => u.CreatedComments).ThenInclude(c => c.Post)
                                                                .ThenInclude(p => p.Creator)
                            .Include(u => u.CreatedPosts).ThenInclude(p => p.UpvotedBy)
                            .Include(u => u.CreatedPosts).ThenInclude(p => p.DownvotedBy)
                            .Include(u => u.CreatedPosts).ThenInclude(p => p.Comments)
                            .FirstOrDefault(u => u.UserName.ToUpper() == userName.ToUpper());

            if (user == null)
                return NotFound();

            return View("User", user);
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
