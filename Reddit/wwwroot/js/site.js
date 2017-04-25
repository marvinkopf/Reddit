// Write your Javascript code.

function post_form(form, target) {
    var fields = {};

    $(form).find("input,textarea").each(function () {
        fields[$(this).attr("name")] = $(this).val();
    });

    $.post(target, fields);
}

function upvote_post(postId, oldScore) {
    $.post("/api/post/" + postId + "/upvote");

    $("#" + postId + "-score").text(oldScore + 1);
    $("#" + postId + "-score").attr({ "style" : "color:orange;" });
    $("#" + postId + "-downlayer").attr({ "style" : ""});
    $('#' + postId + "-downbtn").attr({ "onclick" : "downvote_post(" + postId + "," + oldScore + ")" });
    $('#' + postId + "-upbtn").attr({ "onclick" : "un_upvote_post(" + postId + "," + oldScore + ")" });
    $("#" + postId + "-uplayer").attr({ 
        "style" : "background-color: rgba(255, 165, 0, 0.7);width:85%;height:10px;margin-top:5px"});
}

function un_upvote_post(postId, oldScore) {
    $.post("/api/post/" + postId + "/unupvote");

    $("#" + postId + "-score").text(oldScore);
    $("#" + postId + "-score").attr({ "style" : "" });
    $('#' + postId + "-upbtn").attr({ "onclick" : "upvote_post(" + postId + "," + oldScore + ")" });
    $("#" + postId + "-uplayer").attr({ "style" : "" });
}

function downvote_post(postId, oldScore) {
    $.post("/api/post/" + postId + "/downvote");

    $("#" + postId + "-score").text(oldScore - 1);
    $("#" + postId + "-score").attr({ "style" : "color:blue;" });
    $("#" + postId + "-uplayer").attr({ "style" : "" });
    $('#' + postId + "-downbtn").attr({ "onclick" : "un_downvote_post(" + postId + "," + oldScore + ")" });
    $('#' + postId + "-upbtn").attr({ "onclick" : "upvote_post(" + postId + "," + oldScore + ")" });
    $("#" + postId + "-downlayer").attr({ 
        "style" : "background-color: rgba(0, 0, 255, 0.7);width:85%;height:10px;margin-top:5px" });
}

function un_downvote_post(postId, oldScore) {
    $.post("/api/post/" + postId + "/undownvote");

    $("#" + postId + "-score").text(oldScore);
    $("#" + postId + "-score").attr({ "style" : "" });
    $('#' + postId + "-downbtn").attr({ "onclick" : "downvote_post(" + postId + "," + oldScore + ")" });
    $("#" + postId + "-downlayer").attr({ "style" : "" });
}

function upvote_comment(commentId, oldScore) {
    $.post("/api/comment/" + commentId + "/upvote");

    $("#comment-" + commentId + "-score").text(oldScore + 1 + " Points");
    $("#comment-" + commentId + "-downlayer").attr({ "style" : ""});
    $("#comment-" + commentId + "-downbtn").attr({ "onclick" : "downvote_comment(" + commentId + "," + oldScore + ")" });
    $("#comment-" + commentId + "-upbtn").attr({ "onclick" : "un_upvote_comment(" + commentId + "," + oldScore + ")" });
    $("#comment-" + commentId + "-uplayer").attr({ 
        "style" : "background-color: rgba(255, 165, 0, 0.7);width:85%;height:10px;margin-top:5px"});
}

function un_upvote_comment(commentId, oldScore) {
    $.post("/api/comment/" + commentId + "/unupvote");

    $("#comment-" + commentId + "-score").text(oldScore + " Points");
    $("#comment-" + commentId + "-upbtn").attr({ "onclick" : "upvote_comment(" + commentId + "," + oldScore + ")" });
    $("#comment-" + commentId + "-uplayer").attr({ "style" : "" });
}

function downvote_comment(commentId, oldScore) {
    $.post("/api/comment/" + commentId + "/downvote");

    $("#comment-" + commentId + "-score").text(oldScore - 1 + " Points");
    $("#comment-" + commentId + "-uplayer").attr({ "style" : "" });
    $("#comment-" + commentId + "-downbtn").attr({ "onclick" : "un_downvote_comment(" + commentId + "," + oldScore + ")" });
    $("#comment-" + commentId + "-upbtn").attr({ "onclick" : "upvote_comment(" + commentId + "," + oldScore + ")" });
    $("#comment-" + commentId + "-downlayer").attr({ 
        "style" : "background-color: rgba(0, 0, 255, 0.7);width:85%;height:10px;margin-top:5px" });
}

function un_downvote_comment(commentId, oldScore) {
    $.post("/api/comment/" + commentId + "/undownvote");

    $("#comment-" + commentId + "-score").text(oldScore + " Points");
    $("#comment-" + commentId + "-downbtn").attr({ "onclick" : "downvote_comment(" + commentId + "," + oldScore + ")" });
    $("#comment-" + commentId + "-downlayer").attr({ "style" : "" });
}

function show_comment_reply_input(commentId) {
    $("#comment-" + commentId + "-textarea").attr({ "style" : ""});
}

function hide_comment_reply_input(commentId) {
    $("#comment-" + commentId + "-textarea").attr({ "style" : "display:none"});
}

function toggle_comment(commentId) {
    var comment = $("#comment-" + commentId);
    var isCollapsed = comment.hasClass("collapsed");
    
    if (isCollapsed)
    {
        comment.addClass("uncollapsed");
        comment.removeClass("collapsed");
        comment.find(".expander").text("+");
    }
    else
    {
        comment.addClass("collapsed");
        comment.removeClass("uncollapsed");
        comment.find(".expander").text("-");
    }
} 

function load_remaining_comments(commentId) {
    var comment = $("#comment-" + commentId);

    comment.find(".hidden").removeClass("hidden");
}

function hide_child_comments(commentId, element) {
    var comment = $("#comment-" + commentId);

    if ($(element).text() === "hide child comments")
    {
        comment.find(".comment").addClass("hidden");
        comment.find(".remaining-comments").addClass("hidden");
        $(element).text("show child comments");
    }
    else
    {
        comment.find(".comment").removeClass("hidden");
        comment.find(".remaining-comments").removeClass("hidden");
        $(element).text("hide child comments");
    }
}
