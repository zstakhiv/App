﻿$("#datepickerBirthday").datepicker({
    dateFormat: 'yy/mm/dd',
    changeMonth: true,
    changeYear: true,
    yearRange: '-100y:c+nn',
    maxDate: '-1d'
});

$(function () {

    $.contextMenu({
        className: "admin-edit",
        selector: '.context-menu-one',

        callback: function (key) {
            $.get("/Admin/" + key + "?userid=" + $(this).data("id"), function (data) {

                $('#dialogContent').html(data);
                $('#modDialog').modal('show');
            });
        },
        items: loadMe()
    });

    function loadMe() {
        if ($("#role").val() == "Admin") {
            return {
                "Edit": { name: "Змінити", icon: "fas fa-align-justify" },
                "Delete": { name: "Видалити", icon: "fas fa-trash-alt delete" }
            }
        }
        return {
            "Edit": { name: "Права доступу", icon: "fas fa-align-justify" },
        }

    }

});

$("input#regionsAndAdmins").each(function () {
    $(this).change(function () {
        $.get("/Admin/GetAdmins/" + "?cityId=" + $('option[value="' + $(this).val() + '"]').data('value'), function (data) {
            $('#getAdmins').empty();
            $('#getAdmins').append(data);
        });
    });
});
