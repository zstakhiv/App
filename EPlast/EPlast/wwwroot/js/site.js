$("#sbmt").click(function () {
    alert("Рапорт додано!");
});
$(document).ready(function () {
    $(function () {
        $("#datepicker").datepicker({ dateFormat: 'dd/mm/yy'}).datepicker("setDate", "0");
    })
    //
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