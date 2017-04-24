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
        
        [HttpGet("{id:int}", Name = "Get")]
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
        public async Task<IActionResult> Post(Post post)
        {
            post.Creator = await _manager.GetUserAsync(HttpContext.User);
            post.Created = DateTime.Now;
            post.Score = 0;

            // Temporary
            post.PostId = _context.Posts.Last().PostId + 1;

            _context.Posts.Add(post);
            _context.SaveChanges();
            return CreatedAtRoute("Get", new { id = post.PostId }, post);
        }

        [HttpGet("{id:int}/comments")]
        public IEnumerable<Comment> GetComments(int id) =>
            _context.Posts.Include(p => p.Comments)
                .First(p => p.PostId == id).Comments;

        [Authorize]
        [HttpPost("{id:int}/[action]")]
        public async Task<IActionResult> Upvote(int id)
        {
            await UnDownvote(id);

            var user = await _manager.GetUserAsync(HttpContext.User); 

            var oldRelation = _context.User_X_Post_Upvoted.Find(user.Id, id);
            if (oldRelation == null)
            {
                var relation = new User_X_Post_Upvoted();
                relation.PostId = id;
                relation.UserId = user.Id;
                relation.Upvoted = true;
                _context.User_X_Post_Upvoted.Add(relation);

                var post = _context.Posts.Find(id);
                post.Score += 1;
                _context.Entry(post).State = EntityState.Modified;
            }
            else if (!oldRelation.Upvoted)
            {
                var post = _context.Posts.Find(id);
                post.Score += 1;
                _context.Entry(post).State = EntityState.Modified;

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
            var oldRelation = _context.User_X_Post_Upvoted.Find(user.Id, id);
            if (oldRelation != null && oldRelation.Upvoted)
            {
                var post = _context.Posts.Find(id);
                post.Score -= 1;
                _context.Entry(post).State = EntityState.Modified;

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

            var oldRelation = _context.User_X_Post_Downvoted.Find(user.Id, id);
            if (oldRelation == null)
            {
                var relation = new User_X_Post_Downvoted();
                relation.PostId = id;
                relation.UserId = user.Id;
                relation.Downvoted = true;
                _context.User_X_Post_Downvoted.Add(relation);

                var post = _context.Posts.Find(id);
                post.Score -= 1;
                _context.Entry(post).State = EntityState.Modified;
            }
            else if (!oldRelation.Downvoted)
            {
                var post = _context.Posts.Find(id);
                post.Score -= 1;
                _context.Entry(post).State = EntityState.Modified;

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
            var oldRelation = _context.User_X_Post_Downvoted.Find(user.Id, id);
            if (oldRelation != null && oldRelation.Downvoted)
            {
                var post = _context.Posts.Find(id);
                post.Score += 1;
                _context.Entry(post).State = EntityState.Modified;

                oldRelation.Downvoted = false;
                _context.Entry(oldRelation).State = EntityState.Modified;

                await _context.SaveChangesAsync();
            }

            return Ok();
        }
    }
}