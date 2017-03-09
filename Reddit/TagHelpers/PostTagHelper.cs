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
    [HtmlTargetElement("post", Attributes = "post")]
    public class PostTagHelper : TagHelper
    {
        [HtmlAttributeName("post")]
        public Post Post { get; set; }
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (Post == null)
                return;

            output.TagName = "div";

            output.Attributes.Add("class", "post");

            output.Content.SetHtmlContent(
                $"<a class=\"post-title\" href=//{Post.Link}>{Post.Title}</a><br>" +
                $"<a href=/user/{Post.Creator.Id}>{Post.Creator.Email}</a> | " +
                $"{Post.Created:dd / MM / yyyy} | <a href='/post/{Post.PostId}'>{Post.Comments.Count()} comments</a>");
        }
    }
}