$(document).ready(function () {
    let status = 0;
    $('[data-toggle="tooltip"]').tooltip();
    $("div.single-card").mouseleave(function () {
        if ($(this).find("div.event-unsubscribe").is(":visible")) {
            $(this).find("div.event-unsubscribe").hide();
            $(this).find("div.event-part").show();
        }
    });
    $("button.delete-card").click(function () {
        $(this).parents("div.single-card").remove();
    });
    $("div.event-unsubscribe").click(function () {
        $(this).hide();
        $(this).parents("div").first().children("div.event-part").hide();
        $(this).parents("div").first().children("div.event-participants").hide();
        $(this).parents("div").first().children("div.event-pen").show();
    });
    $("div.event-pen").click(function () {
        $(this).hide();
        $(this).parents("div").first().children("div.event-participants").show();
    });
    $("div.event-part").mouseenter(function () {
        status = 1;
        $(this).hide();
        $(this).parents("div").first().children("div.event-unsubscribe").show();
    });
    $("div.event-participants").mouseenter(function () {
        status = 0;
        $(this).hide();
        $(this).parents("div").first().children("div.event-unsubscribe").show();
    });
    $("div.event-unsubscribe").mouseleave(function () {
        $(this).hide();
        if (!$(this).parents("div").first().children("div.event-pen").is(":visible")) {
            if (status === 0) {
                $(this).parents("div").first().children("div.event-participants").show();
            }
            else {
                $(this).parents("div").first().children("div.event-part").show();
            }
        }
    });
});
//# sourceMappingURL=events.js.map