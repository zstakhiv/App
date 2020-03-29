$(document).ready(function () {
    $('#dtParticipants').DataTable();
    $('.dataTables_length').addClass('bs-select');
    $('img').on("error", function () {
        $(this).attr('src', "https://images.pexels.com/photos/459225/pexels-photo-459225.jpeg");
    });
    let participantID;
    let eventID;
    let activeElement;
    const ParticipantStatus = {
        Approved: 'Учасник',
        Undetermined: 'Розглядається',
        Rejected: 'Відмовлено'
    };
    if ($('#subscribeBlock').is((':visible'))) {
        $('#unsubscribeBlock').hide();
    }
    $('#deleteIcon').click(function () {
        eventID = $('#eventId').val();
        activeElement = this;
        $("#myModal").modal('show');
    });
    $('#subscribeIcon').click(function () {
        eventID = $('#eventId').val();
        activeElement = this;
        $("#modalSubscribe").modal('show');
    });
    $('#unsubscribeIcon').click(function () {
        eventID = $('#eventId').val();
        activeElement = this;
        $("#modalUnSubscribe").modal('show');
    });
    $("#delete").click(function () {
        if ($(this).parents('div.container').hasClass('event-page-wrapper')) {
            $.ajax({
                type: "POST",
                url: "/Action/DeleteEvent",
                data: { ID: eventID },
                success: function () {
                    $("#myModal").modal('hide');
                    window.location.replace("/Action/GetAction");
                },
                error: function () {
                    $("#myModal").modal('hide');
                    $("#success").hide();
                    $("#fail").show();
                    $("#deleteResult").modal('show');
                },
            });
        }
    });
    $("#unsubscribe").click(function () {
        if ($(this).parents('div.container').hasClass('event-page-wrapper')) {
            $.ajax({
                type: "POST",
                url: "/Action/UnSubscribeOnEvent",
                data: { ID: eventID },
                success: function () {
                    $("#modalUnSubscribe").modal('hide');
                    $('#participantBlock').hide();
                    $('#userClockBlock').hide();
                    $('#unsubscribeBlock').hide();
                    $('#subscribeBlock').show();
                    $('#myTable').load(document.URL + ' #myTable');
                    $("#modalUnSubscribeSuccess").modal('show');
                },
                error: function () {
                    $("#modalUnSubscribe").modal('hide');
                    $("#FAIL").modal('show');
                },
            });
        }
    });
    $("#subscribe").click(function () {
        if ($(this).parents('div.container').hasClass('event-page-wrapper')) {
            $.ajax({
                type: "POST",
                url: "/Action/SubscribeOnEvent",
                data: { ID: eventID },
                success: function () {
                    $("#modalSubscribe").modal('hide');
                    $('#participantBlock').hide();
                    $('#userClockBlock').show();
                    $('#unsubscribeBlock').show();
                    $('#subscribeBlock').hide();
                    $('#myTable').load(document.URL + ' #myTable');
                    $("#modalSubscribeSuccess").modal('show');
                },
                error: function () {
                    $("#modalSubscribe").modal('hide');
                    $("#FAIL").modal('show');
                },
            });
        }
    });
    $(".approved").click(function () {
        participantID = $(this).parents("tr").children("td.event-invisible").children("input[type=hidden]").val();
        activeElement = this;
        $.ajax({
            type: "GET",
            url: "/Action/ApproveParticipant",
            data: { ID: participantID },
            cache: false,
            success: function () {
                $(activeElement).parents("tr").children("td:nth-child(3)").html(ParticipantStatus.Approved);
            },
            error: function () {
                $("#resultOfStatusChanging").html(`Не вдалося змінити статус даного користувача на <b>'${ParticipantStatus.Approved}'</b>.`);
                $("#statusModal").modal('show');
            },
        });
    });
    $(".undetermined").click(function () {
        participantID = $(this).parents("tr").children("td.event-invisible").children("input[type=hidden]").val();
        activeElement = this;
        $.ajax({
            type: "GET",
            url: "/Action/UndetermineParticipant",
            data: { ID: participantID },
            cache: false,
            success: function () {
                $(activeElement).parents("tr").children("td:nth-child(3)").html(ParticipantStatus.Undetermined);
            },
            error: function () {
                $("#resultOfStatusChanging").html(`Не вдалося змінити статус даного користувача на <b>'${ParticipantStatus.Undetermined}'</b>.`);
                $("#statusModal").modal('show');
            },
        });
    });
    $(".rejected").click(function () {
        participantID = $(this).parents("tr").children("td.event-invisible").children("input[type=hidden]").val();
        activeElement = this;
        $.ajax({
            type: "GET",
            url: "/Action/RejectParticipant",
            data: { ID: participantID },
            cache: false,
            success: function () {
                $(activeElement).parents("tr").children("td:nth-child(3)").html(ParticipantStatus.Rejected);
            },
            error: function () {
                $("#resultOfStatusChanging").html(`Не вдалося змінити статус даного користувача на <b>'${ParticipantStatus.Rejected}'</b>.`);
                $("#statusModal").modal('show');
            },
        });
    });
    $('[data-toggle="tooltip"]').tooltip();
    //$("div.event-unsubscribe").click(function () {
    //    $(this).hide();
    //    $(this).parents("div").first().children("div.event-part").hide();
    //    $(this).parents("div").first().children("div.event-participant-status").hide();
    //    $(this).parents("div").first().children("div.event-pen").show();
    //});
    //$("div.event-pen").click(function () {
    //    $(this).hide();
    //    $(this).parents("div").first().children("div.event-participant-status").show();
    //    $(this).parents("div").first().children("div.event-unsubscribe").show();
    //});
    //$("div.event-participant-status").click(function () {
    //    $(this).hide();
    //    $(this).parents("div").first().children("div.event-part").show();
    //});
});
//# sourceMappingURL=eventPage.js.map