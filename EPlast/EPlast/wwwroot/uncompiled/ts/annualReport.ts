$('#view-annual-reports-form').ready(function () {
    var indexAnnualReportId = 0;
    var indexCityId = 1;
    var indexCityName = 2;
    var indexDate = 5;
    var indexAnnualReportStatus = 6;

    $('#AnnualReportsTable').DataTable({
        'language': {
            'url': "https://cdn.datatables.net/plug-ins/1.10.20/i18n/Ukrainian.json"
        }
    });

    const AnnualReportStatus = {
        Unconfirmed: 'Непідтверджений',
        Confirmed: 'Підтверджений',
        Saved: 'Збережений'
    }

    $('#AnnualReportsTable tbody tr').click(function () {
        setDisabled([$('#reviewAnnualReport'), $('#confirmAnnualReport'), $('#cancelAnnualReport'), $('#editAnnualReport'), $('#deleteAnnualReport')], true);
        var selected = $(this).hasClass('row-selected');
        $('#AnnualReportsTable tr').removeClass('row-selected');
        if (!selected) {
            $(this).addClass('row-selected');
            switch ($(this).find('td').eq(indexAnnualReportStatus).html()) {
                case AnnualReportStatus.Unconfirmed:
                    setDisabled([$('#confirmAnnualReport'), $('#deleteAnnualReport'), $('#editAnnualReport')], false);
                    break;
                case AnnualReportStatus.Confirmed:
                    setDisabled([$('#cancelAnnualReport')], false);
                    break;
            }
            setDisabled([$('#reviewAnnualReport')], false);
        }
    })

    $('#addAnnualReport').click(function (e) {
        e.preventDefault();
        e.stopPropagation();
        $('#ModalAddAnnualReport').modal('show');
    })

    $('#reviewAnnualReport').click(function (e) {
        e.preventDefault();
        e.stopPropagation();
        var tr = $('#AnnualReportsTable tr.row-selected:first');
        var annualReportId = $(tr).find('td').eq(indexAnnualReportId).html();
        $.ajax({
            url: '/Documentation/GetAnnualReport',
            type: 'GET',
            cache: false,
            data: { id: annualReportId },
            success: function (result) {
                $('#ModalContent').html(result);
                $('#AnnualReportModal').modal('show');
            },
            error: function (response) {
                if (response.status === 404) {
                    showModalMessage($('#ModalError'), response.responseText);
                }
                else {
                    var strURL = '/Error/HandleError?code=' + response.status;
                    window.open(strURL, '_self');
                }
            }
        });
    })

    $('#confirmAnnualReport').click(function (e) {
        e.preventDefault();
        e.stopPropagation();
        var tr = $('#AnnualReportsTable tr.row-selected:first');
        var cityName = $(tr).find('td').eq(indexCityName).html();
        var date = $(tr).find('td').eq(indexDate).html();
        var year = date.split('-').pop();
        $('#Yes').bind('click', confirmAnnualReport);
        showModalMessage($('#YesNoModal'), 'Ви дійсно хочете підтвердити річний звіт станиці ' + cityName +
            ' за ' + year + ' рік?');
    })

    function confirmAnnualReport(): void {
        $('#Yes').modal('hide');
        $('#Yes').unbind();
        var tr = $('#AnnualReportsTable tr.row-selected:first');
        var annualReportId = $(tr).find('td').eq(indexAnnualReportId).html();
        $.ajax({
            url: '/Documentation/ConfirmAnnualReport',
            type: 'GET',
            cache: false,
            data: { id: annualReportId },
            success: function (message) {
                var rows = $('#AnnualReportsTable tbody:first tr');
                var cityId = tr.find('td').eq(indexCityId).html();
                $(rows).filter(function () {
                    return $(this).find('td').eq(indexCityId).html() == cityId
                        && $(this).find('td').eq(indexAnnualReportStatus).html() == AnnualReportStatus.Confirmed
                }).find('td').eq(indexAnnualReportStatus).html(AnnualReportStatus.Saved);
                $(tr).find('td').eq(indexAnnualReportStatus).html(AnnualReportStatus.Confirmed);
                setDisabled([$('#confirmAnnualReport'), $('#deleteAnnualReport'), $('#editAnnualReport')], true);
                setDisabled([$('#cancelAnnualReport')], false);
                showModalMessage($('#ModalSuccess'), message);
            },
            error: function (response) {
                if (response.status === 404) {
                    showModalMessage($('#ModalError'), response.responseText);
                }
                else {
                    var strURL = '/Error/HandleError?code=' + response.status;
                    window.open(strURL, '_self');
                }
            }
        });
    }

    $('#cancelAnnualReport').click(function (e) {
        e.preventDefault();
        e.stopPropagation();
        var tr = $('#AnnualReportsTable tr.row-selected:first');
        var cityName = $(tr).find('td').eq(indexCityName).html();
        var date = $(tr).find('td').eq(indexDate).html();
        var year = date.split('-').pop();
        $('#Yes').bind('click', cancelAnnualReport);
        showModalMessage($('#YesNoModal'), 'Ви дійсно хочете скасувати річний звіт станиці ' + cityName +
            ' за ' + year + ' рік?');
    })

    function cancelAnnualReport(): void {
        $('#Yes').modal('hide');
        $('#Yes').unbind();
        var tr = $('#AnnualReportsTable tr.row-selected:first');
        var annualReportId = $(tr).find('td').eq(indexAnnualReportId).html();
        $.ajax({
            url: '/Documentation/CancelAnnualReport',
            type: 'GET',
            cache: false,
            data: { id: annualReportId },
            success: function (message) {
                $(tr).find('td').eq(indexAnnualReportStatus).html(AnnualReportStatus.Unconfirmed);
                setDisabled([$('#cancelAnnualReport')], true);
                setDisabled([$('#confirmAnnualReport'), $('#deleteAnnualReport'), $('#editAnnualReport')], false);
                showModalMessage($('#ModalSuccess'), message);
            },
            error: function (response) {
                if (response.status === 404) {
                    showModalMessage($('#ModalError'), response.responseText);
                }
                else {
                    var strURL = '/Error/HandleError?code=' + response.status;
                    window.open(strURL, '_self');
                }
            }
        });
    }

    $('#editAnnualReport').click(function (e) {
        e.preventDefault();
        e.stopPropagation();
        var tr = $('#AnnualReportsTable tr.row-selected:first');
        var annualReportId = $(tr).find('td').eq(indexAnnualReportId).html();
        var strURL = '/Documentation/EditAnnualReport?id=' + annualReportId;
        window.open(strURL, '_self');
    })

    $('#deleteAnnualReport').click(function (e) {
        e.preventDefault();
        e.stopPropagation();
        var tr = $('#AnnualReportsTable tr.row-selected:first');
        var cityName = $(tr).find('td').eq(indexCityName).html();
        var date = $(tr).find('td').eq(indexDate).html();
        var year = date.split('-').pop();
        $('#Yes').bind('click', deleteAnnualReport);
        showModalMessage($('#YesNoModal'), 'Ви дійсно хочете видалити річний звіт станиці ' + cityName +
            ' за ' + year + ' рік?');
    })

    function deleteAnnualReport(): void {
        $('#Yes').modal('hide');
        $('#Yes').unbind();
        var tr = $('#AnnualReportsTable tr.row-selected:first');
        var annualReportId = $(tr).find('td').eq(indexAnnualReportId).html();
        $.ajax({
            url: '/Documentation/DeleteAnnualReport',
            type: 'GET',
            cache: false,
            data: { id: annualReportId },
            success: function (message) {
                $(tr).find('td').eq(indexAnnualReportStatus).html(AnnualReportStatus.Unconfirmed);
                tr.click();
                tr.remove();
                showModalMessage($('#ModalSuccess'), message);
            },
            error: function (response) {
                if (response.status === 404) {
                    showModalMessage($('#ModalError'), response.responseText);
                }
                else {
                    var strURL = '/Error/HandleError?code=' + response.status;
                    window.open(strURL, '_self');
                }
            }
        });
    }

    $('#CreateAnnualReportLikeAdmin').click(function (e) {
        e.preventDefault();
        e.stopPropagation();
        var cityId = $('#CitiesList option').filter(':selected').val();
        var strURL = '/Documentation/CreateAnnualReportLikeAdmin?cityId=' + cityId;
        window.open(strURL, '_self')
    })
});

$('#annual-report-form').ready(function () {
    if ($('#ModalSuccess .modal-body:first p:first strong:first').contents().length != 0) {
        $('#ModalSuccess').modal('show');
    }
    else {
        if ($('#ModalError .modal-body:first p:first strong:first').contents().length != 0) {
            $('#ModalError').modal('show');
        }
    }
})

function setDisabled(elements: JQuery<HTMLElement>[], disabled: boolean) {
    for (let el of elements) {
        el.prop('disabled', disabled);
        if (disabled) {
            el.addClass('disabled');
        }
        else {
            el.removeClass('disabled');
        }
    }
}

function showModalMessage(modalWindow: JQuery<HTMLElement>, message: string) {
    modalWindow.find('.modal-body:first p:first strong:first').html(message);
    modalWindow.modal('show');
}