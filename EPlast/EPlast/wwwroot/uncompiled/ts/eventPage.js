$(document).ready(function () {
    $('img').on("error", function () {
        $(this).attr('src', "https://images.pexels.com/photos/459225/pexels-photo-459225.jpeg");
    });
    $('[data-toggle="tooltip"]').tooltip();
    $("div.event-unsubscribe").click(function () {
        $(this).hide();
        $(this).parents("div").first().children("div.event-part").hide();
        $(this).parents("div").first().children("div.event-participant-status").hide();
        $(this).parents("div").first().children("div.event-pen").show();
    });
    $("div.event-pen").click(function () {
        $(this).hide();
        $(this).parents("div").first().children("div.event-participant-status").show();
        $(this).parents("div").first().children("div.event-unsubscribe").show();
    });
    $("div.event-participant-status").click(function () {
        $(this).hide();
        $(this).parents("div").first().children("div.event-part").show();
    });
});
//# sourceMappingURL=eventPage.js.map