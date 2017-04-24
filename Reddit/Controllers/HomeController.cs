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
                                    .Include(p => p.DownvotedBy));
        }

        [HttpGet("[action]/{id:int}")]
        public IActionResult Post(int id) =>
            View(_context.Posts
                            .Include(p => p.Creator)
                            .Include(p => p.Comments).ThenInclude(c => c.Children)
                            .Include(p => p.UpvotedBy)
                            .Include(p => p.DownvotedBy)
                            .First(p => p.PostId == id));

        [Authorize]
        public IActionResult Submit() => View();

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
