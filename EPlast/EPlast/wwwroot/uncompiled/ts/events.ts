$(document).ready(function () {
    let status = 0;
    $('[data-toggle="tooltip"]').tooltip();

    $("div.single-card").mouseleave(function () {
        if ($(this).find("div.events-unsubscribe").is(":visible")) {
            $(this).find("div.events-unsubscribe").hide();
            $(this).find("div.events-part").show();
        }
    });

    $("a.delete-card").click(function () {
        $(this).parents("div.single-card").remove();
    });

    $("div.events-unsubscribe").click(function () {

        $(this).hide();
        $(this).parents("div").first().children("div.events-part").hide();
        $(this).parents("div").first().children("div.events-participants").hide();
        $(this).parents("div").first().children("div.events-pen").show();
    });

    $("div.events-pen").click(function () {
        $(this).hide();
        $(this).parents("div").first().children("div.events-participants").show();
    });

    $("div.events-part").mouseenter(function () {
        status = 1;
        $(this).hide();
        $(this).parents("div").first().children("div.events-unsubscribe").show();
    });

    $("div.events-participants").mouseenter(function () {
        status = 0;
        $(this).hide();
        $(this).parents("div").first().children("div.events-unsubscribe").show();
    });

    $("div.events-unsubscribe").mouseleave(function () {
        $(this).hide();
        if (!$(this).parents("div").first().children("div.events-pen").is(":visible")) {
            if (status === 0) {
                $(this).parents("div").first().children("div.events-participants").show();
            }
            else {
                $(this).parents("div").first().children("div.events-part").show();
            }
        }
    });
});