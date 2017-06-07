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
        
        [HttpGet("{id:int}", Name = "GetComment")]
        public IActionResult Get(int id) 
        {
            var comment = _context.Comments.Include(c => c.Creator)
                .FirstOrDefault(c => c.CommentId == id && c.Creator.Id == _manager.GetUserId(HttpContext.User));
        
            return comment != null ? (IActionResult)Ok(comment) : NotFound();
        }

        [HttpPut("{id:int}")]
        public IActionResult Put(int id, [FromBody]Comment comment)
        {
            if (id != comment.CommentId)
                throw new Exception();

            _context.Entry(comment).State = EntityState.Modified;
            _context.SaveChanges();
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Post(string txt, int postId, int? parentId)
        {
            if (String.IsNullOrWhiteSpace(txt))
            {
                this.Response.StatusCode = 409;
                return this.Content("Text empty");
            }

            var comment = new Comment(
                            txt,
                            DateTime.Now,
                            (await _manager.GetUserAsync(HttpContext.User)).Id,
                            postId) { ParentId = parentId };

            _context.Comments.Add(comment);
            _context.SaveChanges();
            
            return CreatedAtRoute("GetComment", new { id = comment.CommentId }, comment);
        }

        [Authorize]
        [HttpPost("{id:int}/[action]")]
        public async Task<IActionResult> Upvote(int id)
        {
            if (!_context.Comments.Any(c => c.PostId == id))
                return NotFound();

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
            if (!_context.Comments.Any(c => c.PostId == id))
                return NotFound();

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
            if (!_context.Comments.Any(c => c.PostId == id))
                return NotFound();

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
            if (!_context.Comments.Any(c => c.PostId == id))
                return NotFound();

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