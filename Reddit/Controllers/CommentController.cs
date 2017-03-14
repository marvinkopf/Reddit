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
    public class CommentController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly UserManager<ApplicationUser> _manager;
        
        public CommentController(ApplicationDbContext context, UserManager<ApplicationUser> manager)
        {
            _context = context;
            _manager = manager;
        }
        
        [HttpGet("{id:int}")]
        public Comment Get(int id) =>
            _context.Comments.Include(c => c.Creator)
                .First(c => c.CommentId == id && c.Creator.Id == _manager.GetUserId(HttpContext.User));

        [HttpPut("{id:int}")]
        public IActionResult Put(int id, [FromBody]Comment comment)
        {
             _context.Entry(comment).State = EntityState.Modified;
            _context.SaveChanges();
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Post(string txt, int postId)
        {
            var comment = new Comment() { Txt = txt };
            comment.Post = _context.Posts.First(p => p.PostId == postId);
            comment.Creator = await _manager.GetUserAsync(HttpContext.User);
            comment.Created = DateTime.Now;

            // Temporary
            comment.CommentId = _context.Comments.Last().CommentId + 1;

            _context.Comments.Add(comment);
            _context.SaveChanges();
            return Ok();
        }
    }
}