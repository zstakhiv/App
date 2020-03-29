﻿$(document).ready(function () {
    let participantID: string | number | string[];
    $('#dtParticipants').DataTable();
    $('.dataTables_length').addClass('bs-select');
    $('img').on("error", function () {
        $(this).attr('src', "https://images.pexels.com/photos/459225/pexels-photo-459225.jpeg");
    });

    const ParticipantStatus = {
        Approved: 'Учасник',
        Undetermined: 'Розглядається',
        Rejected: 'Відмовлено'
    }

    let flag: boolean;
    let activeElement: HTMLElement;

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
                alert("Trouble with approving");
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
                alert("Trouble with approving");
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
                alert("Trouble with approving");
            },
        });
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