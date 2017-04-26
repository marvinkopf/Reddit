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
        public async Task<IActionResult> Post(Comment comment)
        {
            comment.Creator = await _manager.GetUserAsync(HttpContext.User);
            comment.Created = DateTime.Now;

            // Temporary
            comment.CommentId = _context.Comments.Last().CommentId + 1;

            _context.Comments.Add(comment);
            _context.SaveChanges();
            
            return CreatedAtRoute("Get", new { id = comment.CommentId }, comment);
        }

        [Authorize]
        [HttpPost("{id:int}/[action]")]
        public async Task<IActionResult> Upvote(int id)
        {
            await UnDownvote(id);

            var user = await _manager.GetUserAsync(HttpContext.User); 

            var oldRelation = _context.User_X_Comment_Upvoted.Find(user.Id, id);
            if (oldRelation == null)
            {
                var relation = new User_X_Comment_Upvoted();
                relation.CommentId = id;
                relation.UserId = user.Id;
                relation.Upvoted = true;
                _context.User_X_Comment_Upvoted.Add(relation);

                var Comment = _context.Comments.Find(id);
                Comment.Score += 1;
                _context.Entry(Comment).State = EntityState.Modified;
            }
            else if (!oldRelation.Upvoted)
            {
                var Comment = _context.Comments.Find(id);
                Comment.Score += 1;
                _context.Entry(Comment).State = EntityState.Modified;

                oldRelation.Upvoted = true;
                _context.Entry(oldRelation).State = EntityState.Modified;
            }

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("{id:int}/[action]")]
        public async Task<IActionResult> UnUpvote(int id)
        {
            var user = await _manager.GetUserAsync(HttpContext.User); 
            var oldRelation = _context.User_X_Comment_Upvoted.Find(user.Id, id);
            if (oldRelation != null && oldRelation.Upvoted)
            {
                var Comment = _context.Comments.Find(id);
                Comment.Score -= 1;
                _context.Entry(Comment).State = EntityState.Modified;

                oldRelation.Upvoted = false;
                _context.Entry(oldRelation).State = EntityState.Modified;

                await _context.SaveChangesAsync();
            }

            return Ok();
        }

        [HttpPost("{id:int}/[action]")]
        public async Task<IActionResult> Downvote(int id)
        {
            await UnUpvote(id);

            var user = await _manager.GetUserAsync(HttpContext.User); 

            var oldRelation = _context.User_X_Comment_Downvoted.Find(user.Id, id);
            if (oldRelation == null)
            {
                var relation = new User_X_Comment_Downvoted();
                relation.CommentId = id;
                relation.UserId = user.Id;
                relation.Downvoted = true;
                _context.User_X_Comment_Downvoted.Add(relation);

                var Comment = _context.Comments.Find(id);
                Comment.Score -= 1;
                _context.Entry(Comment).State = EntityState.Modified;
            }
            else if (!oldRelation.Downvoted)
            {
                var Comment = _context.Comments.Find(id);
                Comment.Score -= 1;
                _context.Entry(Comment).State = EntityState.Modified;

                oldRelation.Downvoted = true;
                _context.Entry(oldRelation).State = EntityState.Modified;
            }

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("{id:int}/[action]")]
        public async Task<IActionResult> UnDownvote(int id)
        {
            var user = await _manager.GetUserAsync(HttpContext.User); 
            var oldRelation = _context.User_X_Comment_Downvoted.Find(user.Id, id);
            if (oldRelation != null && oldRelation.Downvoted)
            {
                var Comment = _context.Comments.Find(id);
                Comment.Score += 1;
                _context.Entry(Comment).State = EntityState.Modified;

                oldRelation.Downvoted = false;
                _context.Entry(oldRelation).State = EntityState.Modified;

                await _context.SaveChangesAsync();
            }

            return Ok();
        }
    }
}