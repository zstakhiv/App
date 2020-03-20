$(document).ready(function () {
    $('.dataTables_length').addClass('bs-select');
});
/*js from LoginAndRegister*/
function registerClick() {
    $(".switcher-text").addClass("register-active").removeClass("login-active");
}
function loginClick() {
    $(".switcher-text").addClass("login-active").removeClass("register-active");
}
$(".register-text").click(registerClick);
$(".login-text").click(loginClick);
$("input#autocomplete_input").each(function (index) {
    $(this).change(function () {
        $("#autocomplete_input_id_" + index).val($('option[value="' + $(this).val() + '"]').data('value'));
    });
});
$("#datepickerBirthday").datepicker({
    dateFormat: 'yy/mm/dd',
    changeMonth: true,
    changeYear: true,
    yearRange: '-100y:c+nn',
    maxDate: '-1d'
});
//# sourceMappingURL=site.js.map