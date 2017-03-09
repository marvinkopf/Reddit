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
    [Route("api/[controller]")]
    public class PostController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly UserManager<ApplicationUser> _manager;
        
        public PostController(ApplicationDbContext context, UserManager<ApplicationUser> manager)
        {
            _context = context;
            _manager = manager;
        }
        
        [HttpGet("{id:int}")]
        public Post Get(int id) =>
            _context.Posts.First(p => p.PostId == id);

        [HttpPut("{id:int}")]
        public IActionResult Put(int id, [FromBody]Post post)
        {
             _context.Entry(post).State = EntityState.Modified;
            _context.SaveChanges();
            return Ok();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Post(string link, string title)
        {
            var post = new Post() { Link = link, Title = title };
            post.Creator = await _manager.GetUserAsync(HttpContext.User);
            post.Created = DateTime.Now;
            post.Score = 0;

            // Temporary
            post.PostId = _context.Posts.Last().PostId + 1;

            _context.Posts.Add(post);
            _context.SaveChanges();
            return Ok();
        }

        [HttpGet("{id:int}/comments")]
        public IEnumerable<Comment> GetComments(int id) =>
            _context.Posts.Include(p => p.Comments)
                .First(p => p.PostId == id).Comments;
    }
}