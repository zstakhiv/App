let menuOptionsBySection = [6, 0, 7, 6, 14];
let profileListing = [
    ['Персональні дані',
        'Дійсне членство',
        'Діловодства',
        'Вишколи',
        'З`їзд',
        'Бланки'],
    [],
    ['Користувачі',
        'Станиці',
        'Округи',
        'Курені',
        'Акції',
        'Відзначення',
        'Кадра Виховників'],
    ['Користувачі',
        'Станиця',
        'Курінь',
        'Кваліфікаційні Вишколи',
        'Акції/Табори',
        'Відзначення'],
    ['Подати річний звіт станиці',
        'Таблиця користувачів',
        'Зголосити вишкіл',
        'Річні звіти',
        'Осередки та адміни',
        'Зголошення вишколів',
        'Геостатистика',
        'Статистика по роках',
        'Статистика (періоди)',
        'Порівняти осередки',
        'Додати рішення КПР',
        'Додати рішення КРК',
        'Зголошені на КПЗ2018',
        'Статистичні звіти']
];
let buttonClone;
let backdrop;
let mainWrapper;
let open = false;
let currentIndex = -1;
let scrollBeforeMenu;

function init() {
    backdrop = document.getElementsByClassName('backdrop')[0];
    buttonClone = document.getElementsByClassName('menu-button')[0];
    mainWrapper = document.getElementsByClassName('main-wrapper')[0];
    document.getElementsByClassName('menu-button-holder')[0].removeChild(buttonClone);
}
function onMenuHover(index) {
    console.log(open);
    console.log(index);
    console.log(currentIndex);
    console.log(profileListing[index].length);
    if (open) {

        if (currentIndex === index || profileListing[index].length === 0) {
            closeMenu();

            var checkVisible = setInterval(function () {
                if (window.scrollY === scrollBeforeMenu) {
                    clearInterval(checkVisible);
                } else {
                    window.scrollTo(0, scrollBeforeMenu);
                    console.log('scroll');
                }
            }, 10);

            return;
        }
    } else {
        scrollBeforeMenu = +window.scrollY;
    }
    if (profileListing[index].length === 0) {
        console.log('yess');
        return;
    }
    console.log('no');
    currentIndex = index;
    mainWrapper.classList.add('non-scrollable');
    show(backdrop);
    open = true;
    let buttons = document.getElementsByClassName('menu-button');
    for (let i = buttons.length - 1; i >= 0; --i) {
        document.getElementsByClassName('menu-button-holder')[0].removeChild(buttons[i]);
    }
    for (let i = 0; i < menuOptionsBySection[index]; ++i) {
        let clone = buttonClone.cloneNode(true);
        clone.innerHTML = profileListing[index][i];
        document.getElementsByClassName('menu-button-holder')[0].appendChild(clone);
    }
}
function closeMenu() {
    if (open) {
        hide(backdrop);
        open = false;
        mainWrapper.classList.remove('non-scrollable');
    }
}
function show(element) {
    element.classList.remove('hidden');
}
function hide(element) {
    element.classList.add('hidden');
}