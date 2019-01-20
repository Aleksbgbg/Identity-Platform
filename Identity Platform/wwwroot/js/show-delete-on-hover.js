$(document).ready(function() {
    $("div.comment").hover(
        function(event) {
            $(event.currentTarget)
                .find("button.btn-delete-comment")
                .css("visibility", "visible");
        },
        function(event) {
            $(event.currentTarget)
                .find("button.btn-delete-comment")
                .css("visibility", "hidden");
        }
    );
});