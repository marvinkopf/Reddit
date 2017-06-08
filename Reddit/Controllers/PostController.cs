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

        [HttpGet("{id:int}", Name = "GetPost")]
        public IActionResult Get(int id)
        {
            var post = _context.Posts.FirstOrDefault(p => p.PostId == id);
            return post != null ? (IActionResult)Ok(post) : NotFound();
        }

        [HttpPut("{id:int}")]
        public IActionResult Put(int id, [FromBody]Post post)
        {
            if (id != post.PostId)
                return BadRequest();

            _context.Entry(post).State = EntityState.Modified;
            _context.SaveChanges();
            return Ok();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Post(string title, string link, string subreddit,
            string urlToImage)
        {
            if (String.IsNullOrWhiteSpace(subreddit))
            {
                this.Response.StatusCode = 409;
                return this.Content("No subreddit given");
            }

            if (!_context.Subreddits.Any(s => s.Name == subreddit))
            {
                this.Response.StatusCode = 409;
                return this.Content("Subreddit doesn't exist");
            }

            if (String.IsNullOrWhiteSpace(title))
            {
                this.Response.StatusCode = 409;
                return this.Content("No title given");
            }

            // TODO Should be a valid link/domain, too
            if (String.IsNullOrWhiteSpace(link))
            {
                this.Response.StatusCode = 409;
                return this.Content("No valid link");
            }

            var post = new Post(
                            title,
                            link,
                            subreddit,
                            DateTime.Now,
                            (await _manager.GetUserAsync(HttpContext.User)).Id)
            { UrlToImage = urlToImage };

            _context.Posts.Add(post);
            _context.SaveChanges();

            return CreatedAtRoute("GetPost", new { id = post.PostId }, post);
        }

        [HttpGet("{id:int}/comments")]
        public IActionResult GetComments(int id)
        {
            var post = _context.Posts.Include(p => p.Comments)
                .FirstOrDefault(p => p.PostId == id);
            return post != null ? (IActionResult)Ok(post.Comments) : NotFound();
        }

        [Authorize]
        [HttpPost("{id:int}/[action]")]
        public async Task<IActionResult> Upvote(int id)
        {
            if (!_context.Posts.Any(p => p.PostId == id))
                return NotFound();

            await UnDownvote(id);

            var user = await _manager.GetUserAsync(HttpContext.User);
            var relation = _context.User_X_Post_Upvoted.FirstOrDefault(uxp =>
                                uxp.UserId == user.Id && uxp.PostId == id);
            if (relation == null)
            {
                relation = new User_X_Post_Upvoted();
                relation.PostId = id;
                relation.UserId = user.Id;
                _context.User_X_Post_Upvoted.Add(relation);

                var post = _context.Posts.Find(id);
                post.Score += 1;
                _context.Entry(post).State = EntityState.Modified;

                await _context.SaveChangesAsync();
            }

            return Ok();
        }

        [HttpPost("{id:int}/[action]")]
        public async Task<IActionResult> UnUpvote(int id)
        {
            if (!_context.Posts.Any(p => p.PostId == id))
                return NotFound();

            var user = await _manager.GetUserAsync(HttpContext.User);
            var relation = _context.User_X_Post_Upvoted.FirstOrDefault(uxp =>
                                uxp.UserId == user.Id && uxp.PostId == id);
            if (relation != null)
            {
                var post = _context.Posts.Find(id);
                post.Score -= 1;
                _context.Entry(post).State = EntityState.Modified;

                _context.Entry(relation).State = EntityState.Deleted;

                await _context.SaveChangesAsync();
            }

            return Ok();
        }

        [HttpPost("{id:int}/[action]")]
        public async Task<IActionResult> Downvote(int id)
        {
            if (!_context.Posts.Any(p => p.PostId == id))
                return NotFound();

            await UnUpvote(id);

            var user = await _manager.GetUserAsync(HttpContext.User);

            var relation = _context.User_X_Post_Downvoted.FirstOrDefault( uxp =>
                                uxp.UserId == user.Id && uxp.PostId == id);
            if (relation == null)
            {
                relation = new User_X_Post_Downvoted();
                relation.PostId = id;
                relation.UserId = user.Id;
                _context.User_X_Post_Downvoted.Add(relation);

                var post = _context.Posts.Find(id);
                post.Score -= 1;
                _context.Entry(post).State = EntityState.Modified;

                await _context.SaveChangesAsync();
            }

            return Ok();
        }

        [HttpPost("{id:int}/[action]")]
        public async Task<IActionResult> UnDownvote(int id)
        {
            if (!_context.Posts.Any(p => p.PostId == id))
                return NotFound();

            var user = await _manager.GetUserAsync(HttpContext.User);
            var relation = _context.User_X_Post_Downvoted.FirstOrDefault(uxp =>
                                uxp.UserId == user.Id && uxp.PostId == id);
            if (relation != null)
            {
                var post = _context.Posts.Find(id);
                post.Score += 1;
                _context.Entry(post).State = EntityState.Modified;

                _context.Entry(relation).State = EntityState.Deleted;

                await _context.SaveChangesAsync();
            }

            return Ok();
        }
    }
}