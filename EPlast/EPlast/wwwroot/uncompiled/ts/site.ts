$(document).ready(function () {
    $("#sbmt").click(function () {
        alert("Рапорт додано!");
    });
    $(function () {
        $("#datepicker").datepicker({ dateFormat: 'yy/mm/dd' }).datepicker("setDate", "0");
    })
    $('#dtBasicExample').DataTable();
    $('.dataTables_length').addClass('bs-select');
});
/*js from LoginAndRegister*/

function registerClick() {
    $(".switcher-text").addClass("register-active").removeClass("login-active");
    $(".register-form").removeClass("d-none");
    $(".login-form").addClass("d-none");
    // $(".login-form").hide(500);
}

function loginClick() {
    $(".switcher-text").addClass("login-active").removeClass("register-active");
    $(".register-form").addClass("d-none");
    $(".login-form").removeClass("d-none");
}

$(".register-text").click(registerClick);
$(".login-text").click(loginClick);

$("input#autocomplete_input").each(function (index) {
    $(this).change(function () {
        $("#autocomplete_input_id_" + index).val($('option[value="' + $(this).val() + '"]').data('value'));
    });
});


$("tr.read_row").dblclick(function () {
    var content = $(this).find('td').map(function () {
        return $(this).text()
    })[0];

    window.open("/Report/CreatePDFAsync?objId=" + content, '_blank')
});

$("#datepickerBirthday").datepicker({
    dateFormat: 'yy/mm/dd',
    changeMonth: true,
    changeYear: true,
    yearRange: '-100y:c+nn',
    maxDate: '-1d'

});