// Write your Javascript code.

function post_form(form, target) {
    var fields = {};
    
    $(form).find("input").each(function() {
                fields[$(this).attr("name")] = $(this).val();
        });

    $.post(target, fields);
}
