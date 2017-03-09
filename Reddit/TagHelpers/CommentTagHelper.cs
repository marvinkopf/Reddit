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

            output.Content.SetHtmlContent(Comment.Txt);
        }
    }
}