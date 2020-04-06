$(document).ready(function () {
    $('#CurrentPositionsTable tr').click(function () {
        var buttons = [$('#endPosition'), $('#deleteCurrentPosition')];
        setRowSelected($(this), buttons);
    })

    $('#PositionsTable tr').click(function () {
        var buttons = [$('#deletePosition')];
        setRowSelected($(this), buttons);
    })

    $('#endPosition').click(function (e) {
        e.preventDefault();
        e.stopPropagation();
        var tr = $('#CurrentPositionsTable tr.row-selected:first');
        var positionId = tr.find(':first').html();
        $.ajax({
            url: '/Account/EndPosition',
            type: 'GET',
            cache: false,
            data: { id: positionId },
            success: function (result) {
                if (result) {
                    tr.click();
                    tr.remove();
                    var currentDate = new Date();
                    var day = currentDate.getDate();
                    var month = currentDate.getMonth() + 1;
                    var year = currentDate.getFullYear();
                    var strDate = (day < 10 ? '0' : '') + day + '.'
                        + (month < 10 ? '0' : '') + month + '.'
                        + year;
                    tr.find('td:last').append(' - ' + strDate);
                    tr.bind("click", function () {
                        var buttons = [$('#deletePosition')];
                        setRowSelected(tr, buttons);
                    });
                    $('#PositionsTable tbody').append(tr);
                    $('#ModalSuccess .modal-body:first p:first strong:first').html('Каденцію діловодства успішно завершено!');
                    $('#ModalSuccess').modal('show');
                }
                else {
                    $('#ModalError .modal-body:first p:first strong:first').html('Не вдалося завершити каденцію діловодства!');
                    $('#ModalError').modal('show');
                }
            },
            error: function () {
                $('#ModalError .modal-body:first p:first strong:first').html('Не вдалося завершити каденцію діловодства!');
                $('#ModalError').modal('show');
            }
        });
    })

    $('#deleteCurrentPosition').click(function (e) {
        e.preventDefault();
        e.stopPropagation();
        var tr = $('#CurrentPositionsTable tr.row-selected:first');
        var positionId = tr.find(':first').html();
        $.ajax({
            url: '/Account/DeletePosition',
            type: 'GET',
            cache: false,
            data: { id: positionId },
            success: function (result) {
                if (result) {
                    tr.click();
                    tr.remove();
                    $('#ModalSuccess .modal-body:first p:first strong:first').html('Діловодство успішно видалено!');
                    $('#ModalSuccess').modal('show');
                }
                else {
                    $('#ModalError .modal-body:first p:first strong:first').html('Не вдалося видалити діловодство!');
                    $('#ModalError').modal('show');
                }
            },
            error: function () {
                $('#ModalError .modal-body:first p:first strong:first').html('Не вдалося видалити діловодство!');
                $('#ModalError').modal('show');
            }
        });
    })

    $('#deletePosition').click(function (e) {
        e.preventDefault();
        e.stopPropagation();
        var tr = $('#PositionsTable tr.row-selected:first');
        var positionId = tr.find(':first').html();
        $.ajax({
            url: '/Account/DeletePosition',
            type: 'GET',
            cache: false,
            data: { id: positionId },
            success: function (result) {
                if (result) {
                    tr.click();
                    tr.remove();
                    $('#ModalSuccess .modal-body:first p:first strong:first').html('Діловодство успішно видалено!');
                    $('#ModalSuccess').modal('show');
                }
                else {
                    $('#ModalError .modal-body:first p:first strong:first').html('Не вдалося видалити діловодство!');
                    $('#ModalError').modal('show');
                }
            },
            error: function () {
                $('#ModalError .modal-body:first p:first strong:first').html('Не вдалося видалити діловодство!');
                $('#ModalError').modal('show');
            }
        });
    })
})

function setRowSelected(tr: JQuery<HTMLElement>, disabledElements: JQuery<HTMLElement>[]): void {
    setDisabled(disabledElements, true);
    var selected = $(tr).hasClass('row-selected');
    $(tr).parent().find('tr').removeClass('row-selected');
    if (!selected) {
        $(tr).addClass('row-selected');
        setDisabled(disabledElements, false);
    }
}