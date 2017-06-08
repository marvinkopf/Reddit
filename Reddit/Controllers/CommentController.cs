using System;
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
        [AllowAnonymous]
        public IActionResult Get(int id) 
        {
            var comment = _context.Comments.Include(c => c.Creator)
                .FirstOrDefault(c => c.CommentId == id);
        
            return comment != null ? (IActionResult)Ok(comment) : NotFound();
        }

        [HttpPut("{id:int}")]
        public IActionResult Put(int id, [FromBody]Comment comment)
        {
            if (id != comment.CommentId)
                return BadRequest();

            _context.Entry(comment).State = EntityState.Modified;
            _context.SaveChanges();
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Post(string txt, int? postId, int? parentId)
        {
            if (String.IsNullOrWhiteSpace(txt))
            {
                this.Response.StatusCode = 409;
                return this.Content("Text empty");
            }

            if (postId == null)
            {
                this.Response.StatusCode = 409;
                return this.Content("No postid given");
            }

            if (parentId.HasValue && _context.Comments.Find(parentId.Value).PostId != postId)
            {
                this.Response.StatusCode = 409;
                return this.Content("Can't attach comment to parent comment that's on another post");
            }

            var comment = new Comment(
                            txt,
                            DateTime.Now,
                            (await _manager.GetUserAsync(HttpContext.User)).Id,
                            postId.Value) { ParentId = parentId };

            _context.Comments.Add(comment);
            _context.SaveChanges();
            
            return CreatedAtRoute("GetComment", new { id = comment.CommentId }, comment);
        }

        [HttpPost("{id:int}/[action]")]
        public async Task<IActionResult> Upvote(int id)
        {
            if (!_context.Comments.Any(c => c.CommentId == id))
                return NotFound();

            await UnDownvote(id);
            
            var user = await _manager.GetUserAsync(HttpContext.User); 
            var relation = _context.User_X_Comment_Upvoted.FirstOrDefault(uxc =>
                                uxc.UserId == user.Id && uxc.CommentId == id);
            if (relation == null)
            {
                relation = new User_X_Comment_Upvoted();
                relation.CommentId = id;
                relation.UserId = user.Id;
                _context.User_X_Comment_Upvoted.Add(relation);

                var Comment = _context.Comments.Find(id);
                Comment.Score += 1;
                _context.Entry(Comment).State = EntityState.Modified;
            }

            await _context.SaveChangesAsync();
            
            return Ok();
        }

        [HttpPost("{id:int}/[action]")]
        public async Task<IActionResult> UnUpvote(int id)
        {
            if (!_context.Comments.Any(c => c.CommentId == id))
                return NotFound();

            var user = await _manager.GetUserAsync(HttpContext.User);
            var relation = _context.User_X_Comment_Upvoted.FirstOrDefault(uxc =>
                                uxc.UserId == user.Id && uxc.CommentId == id);
            if (relation != null)
            {
                var Comment = _context.Comments.Find(id);
                Comment.Score -= 1;
                _context.Entry(Comment).State = EntityState.Modified;

                _context.Entry(relation).State = EntityState.Deleted;

                await _context.SaveChangesAsync();
            }

            return Ok();
        }

        [HttpPost("{id:int}/[action]")]
        public async Task<IActionResult> Downvote(int id)
        {
            if (!_context.Comments.Any(c => c.CommentId == id))
                return NotFound();

            await UnUpvote(id);

            var user = await _manager.GetUserAsync(HttpContext.User);
            var relation = _context.User_X_Comment_Downvoted.FirstOrDefault(uxc =>
                                uxc.UserId == user.Id && uxc.CommentId == id);
            if (relation == null)
            {
                relation = new User_X_Comment_Downvoted();
                relation.CommentId = id;
                relation.UserId = user.Id;
                _context.User_X_Comment_Downvoted.Add(relation);

                var Comment = _context.Comments.Find(id);
                Comment.Score -= 1;
                _context.Entry(Comment).State = EntityState.Modified;
            }

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("{id:int}/[action]")]
        public async Task<IActionResult> UnDownvote(int id)
        {
            if (!_context.Comments.Any(c => c.CommentId == id))
                return NotFound();

            var user = await _manager.GetUserAsync(HttpContext.User);
            var relation = _context.User_X_Comment_Downvoted.FirstOrDefault(uxc =>
                                uxc.UserId == user.Id && uxc.CommentId == id);
            if (relation != null)
            {
                var Comment = _context.Comments.Find(id);
                Comment.Score += 1;
                _context.Entry(Comment).State = EntityState.Modified;

                _context.Entry(relation).State = EntityState.Deleted;

                await _context.SaveChangesAsync();
            }

            return Ok();
        }
    }
}