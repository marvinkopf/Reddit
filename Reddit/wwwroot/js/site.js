// Write your Javascript code.

function post_form(form, target) {
    var fields = {};

    $(form).find("input").each(function () {
        fields[$(this).attr("name")] = $(this).val();
    });

    $.post(target, fields);
}

function upvote_post(postId, oldScore) {
    // Fake
    $.post("api/post/" + postId);

    $("#" + postId + "-score").text(oldScore + 1);
    $("#" + postId + "-score").attr({ "style" : "color:orange;" });
    $("#" + postId + "-downlayer").attr({ "style" : ""});
    $('#' + postId + "-downbtn").attr({ "onclick" : "downvote_post(" + postId + "," + oldScore + ")" });
    $('#' + postId + "-upbtn").attr({ "onclick" : "un_upvote_post(" + postId + "," + oldScore + ")" });
    $("#" + postId + "-uplayer").attr({ 
        "style" : "background-color: rgba(255, 165, 0, 0.7);width:85%;height:10px;margin-top:5px"});
}

function un_upvote_post(postId, oldScore) {
    // Fake
    $.post("api/post/" + postId);

    $("#" + postId + "-score").text(oldScore);
    $("#" + postId + "-score").attr({ "style" : "" });
    $('#' + postId + "-upbtn").attr({ "onclick" : "upvote_post(" + postId + "," + oldScore + ")" });
    $("#" + postId + "-uplayer").attr({ "style" : "" });
}

function downvote_post(postId, oldScore) {
    // Fake
    $.post("api/post/" + postId);

    $("#" + postId + "-score").text(oldScore - 1);
    $("#" + postId + "-score").attr({ "style" : "color:blue;" });
    $("#" + postId + "-uplayer").attr({ "style" : "" });
    $('#' + postId + "-downbtn").attr({ "onclick" : "un_downvote_post(" + postId + "," + oldScore + ")" });
    $('#' + postId + "-upbtn").attr({ "onclick" : "upvote_post(" + postId + "," + oldScore + ")" });
    $("#" + postId + "-downlayer").attr({ 
        "style" : "background-color: rgba(0, 0, 255, 0.7);width:85%;height:10px;margin-top:5px" });
}

function un_downvote_post(postId, oldScore) {
    // Fake
    $.post("api/post/" + postId);

    $("#" + postId + "-score").text(oldScore);
    $("#" + postId + "-score").attr({ "style" : "" });
    $('#' + postId + "-downbtn").attr({ "onclick" : "downvote_post(" + postId + "," + oldScore + ")" });
    $("#" + postId + "-downlayer").attr({ "style" : "" });
}
