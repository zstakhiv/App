let profileListing = [
    ['<a class="text-light" href="/Account/UserProfile">Персональні дані</a>',
        'Дійсне членство',
        'Діловодства',
        'Вишколи',
        'З`їзд',
        'Бланки'],
    [],
    ['Користувачі',
        'Станиці',
        'Округи',
        '<a class="text-light" href="/Action/GetAction">Акції</a>',
        '<a class="text-light" href="/Club">Курені</a>',
        'Відзначення',
        'Кадра Виховників'],
    ['Користувачі',
        'Станиця',
        'Курінь',
        'Кваліфікаційні Вишколи',
        'Акції/Табори',
        'Відзначення'],
    ['<a class="text-light" href="/Report/CreateAnnualReport">Подати річний звіт станиці</a>',
        'Таблиця користувачів',
        'Зголосити вишкіл',
        '<a class="text-light" href="/Report/ViewAnnualReports">Річні звіти</a>',
        'Осередки та адміни',
        'Зголошення вишколів',
        'Геостатистика',
        'Статистика по роках',
        'Статистика (періоди)',
        'Порівняти осередки',
        '<a class="text-light" href="/Report/CreateRaport">Додати рішення керівних органів</a>',
        'Зголошені на КПЗ2018',
        'Статистичні звіти']
];
let buttonClone;
let backdrop;
let mainWrapper;
let menuOpen = false;
let currentIndex = -1;
let scrollBeforeMenu;
$(document).ready(() => {
    backdrop = $('.backdrop')[0];
    buttonClone = $('.menu-button')[0];
    mainWrapper = $('.main-wrapper')[0];
    $('.menu-button-holder')[0].removeChild(buttonClone);
    $(backdrop).hide();
    $(backdrop).css('visibility', 'visible');
});
function onMenuHover(index) {
    if (menuOpen) {
        if (currentIndex === index || profileListing[index].length === 0) {
            closeMenu();
            const checkVisible = setInterval(function () {
                if (window.scrollY === scrollBeforeMenu) {
                    clearInterval(checkVisible);
                }
                else {
                    window.scrollTo(0, scrollBeforeMenu);
                }
            }, 10);
            return;
        }
    }
    else {
        scrollBeforeMenu = +window.scrollY;
    }
    if (profileListing[index].length === 0) {
        return;
    }
    currentIndex = index;
    $(mainWrapper).addClass('non-scrollable');
    $(backdrop).show();
    menuOpen = true;
    let buttons = $('.menu-button');
    for (let i = buttons.length - 1; i >= 0; --i) {
        $('.menu-button-holder')[0].removeChild(buttons[i]);
    }
    for (let i = 0; i < profileListing[index].length; ++i) {
        let clone = buttonClone.cloneNode(true);
        $(clone).html(profileListing[index][i]);
        $('.menu-button-holder')[0].appendChild(clone);
    }
}
function closeMenu() {
    if (menuOpen) {
        $(backdrop).hide();
        menuOpen = false;
        $(mainWrapper).removeClass('non-scrollable');
    }
}
//# sourceMappingURL=mainMenu.js.map
$(document).ready(function () {
    $(function () {
        $("#datepicker").datepicker({ dateFormat: 'yy/mm/dd' }).datepicker("setDate", "0");
    });
    $('#dtReadRaport').DataTable({
        "language": {
            "url": "https://cdn.datatables.net/plug-ins/1.10.20/i18n/Ukrainian.json"
        }
    });
    $("tr.raport-click-row").dblclick(function () {
        let content = $(this).children().first().text();
        window.open("/Report/CreatePDFAsync?objId=" + content, '_blank');
    });
});
//# sourceMappingURL=raport.js.map
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