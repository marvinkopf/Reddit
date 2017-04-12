using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Reddit.Models;

namespace Reddit.TagHelpers
{
    [HtmlTargetElement("comment", Attributes = "comment")]
    public class CommentTagHelper : TagHelper
    {
        [HtmlAttributeName("comment")]
        public Comment Comment { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (Comment == null)
                return;

            output.TagName = "div";

            output.Attributes.Add("class", "comment");

            var timePassed = TimePassedAsString(Comment.Created);

            output.Content.SetHtmlContent($"<a href='user/{Comment.Creator.Id} class='author'>{Comment.Creator.Email}</a>" +
                $"<span class='score'><b>{Comment.Score} Points</b></span>" +
                $"<span class='date'>{timePassed}</span><br>" +
                $"{Comment.Txt}");
        }

        private string TimePassedAsString(DateTime time)
        {
            var difference = DateTime.Now.Subtract(time);

            if (difference.Days >= 30)
            {
                var countMonths = difference.Days / 30;
                return difference.Days / 30 + " months ago";
            }
            else if (difference.Days > 0)
                return difference.Days + " days ago";
            else if (difference.Hours > 0)
                return difference.Hours + " hours ago";
            else if (difference.Minutes > 0)
                return difference.Minutes + " minutes ago";

            return "<1min ago";
        }
    }
}