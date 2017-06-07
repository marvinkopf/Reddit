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
    public class SubredditController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly UserManager<ApplicationUser> _manager;
        
        public SubredditController(ApplicationDbContext context, UserManager<ApplicationUser> manager)
        {
            _context = context;
            _manager = manager;
        }

        [HttpPost]
        [Authorize]
        public IActionResult Post(string name)
        {
            var subreddit = new Subreddit(name);

            _context.Subreddits.Add(subreddit);
            _context.SaveChanges();
            
            return Ok();
        }

        [HttpPost("{name}/[action]")]
        public async Task<IActionResult> Subscribe(string name)
        {
            var user = await _manager.GetUserAsync(HttpContext.User); 

            var oldRelation = _context.User_X_Subreddit_Subscription.Find(user.Id, name);
            if (oldRelation == null)
            {
                var relation = new User_X_Subreddit_Subscription();
                relation.SubredditName = name;
                relation.UserId = user.Id;
                relation.Subscribed = true;
                _context.User_X_Subreddit_Subscription.Add(relation);
            }
            else if (!oldRelation.Subscribed)
            {
                oldRelation.Subscribed = true;
                _context.Entry(oldRelation).State = EntityState.Modified;
            }

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("{name}/[action]")]
        public async Task<IActionResult> Unsubscribe(string name)
        {
            var user = await _manager.GetUserAsync(HttpContext.User); 

            var oldRelation = _context.User_X_Subreddit_Subscription.Find(user.Id, name);
            if (oldRelation == null)
            {
                var relation = new User_X_Subreddit_Subscription();
                relation.SubredditName = name;
                relation.UserId = user.Id;
                relation.Subscribed = false;
                _context.User_X_Subreddit_Subscription.Add(relation);
            }
            else if (oldRelation.Subscribed)
            {
                oldRelation.Subscribed = false;
                _context.Entry(oldRelation).State = EntityState.Modified;
            }

            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}