let profileListing = [
    ['<a class="text-light nav-link p-0" href="/Account/UserProfile">Персональні дані</a>',
        'Дійсне членство',
        '<a class="text-light nav-link p-0" href="/Account/UserProfile">Діловодства</a>',
        '<a class="text-light nav-link p-0" href="/EventUser/Eventuser">Події</a>',
        'Станиця',
        'Курінь',
        'З`їзди',
        'Відзначення',
        'Бланки'],
    [],
    ['<a class="text-light nav-link p-0" href="/Admin/UsersTable">Таблиця користувачів</a>',
        '<a class="text-light nav-link p-0" href="/City/Index">Станиці</a>',
        'Округи',
        '<a class="text-light nav-link p-0" href="/Action/GetAction">Події</a>',
        '<a class="text-light nav-link p-0" href="/Club">Курені</a>',
        'Відзначення',
        'Кадра Виховників'],
    ['<a class="text-light nav-link p-0" href="/Documentation/CreateAnnualReport">Подати річний звіт станиці</a>',
        '<a class="text-light nav-link p-0" href="/Documentation/ViewAnnualReports">Річні звіти</a>',
        '<a class="text-light nav-link p-0" href="/Admin/RegionsAdmins">Осередки та адміни</a>',
        'Геостатистика',
        'Статистика по роках',
        'Статистика (періоди)',
        'Порівняти осередки',
        '<a class="text-light nav-link p-0" href="/Documentation/CreateDecesion">Додати рішення керівних органів</a>',
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