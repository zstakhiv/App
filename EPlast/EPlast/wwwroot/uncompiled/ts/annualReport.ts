$('#view-annual-reports-form').ready(function () {
    $('#AnnualReportsTable').DataTable({
        'language': {
            'url': "https://cdn.datatables.net/plug-ins/1.10.20/i18n/Ukrainian.json"
        }
    });

    const AnnualReportStatus = {
        Unconfirmed: 'Непідтверджений',
        Confirmed: 'Підтверджений',
        Canceled: 'Скасований'
    }

    $('#AnnualReportsTable tbody tr').click(function () {
        setDisabled([$('#reviewAnnualReport'), $('#confirmAnnualReport'), $('#cancelAnnualReport')], true);
        var selected = $(this).hasClass('row-selected');
        $('#AnnualReportsTable tr').removeClass('row-selected');
        if (!selected) {
            $(this).addClass('row-selected');
            if ($(this).find('td:last').html() === AnnualReportStatus.Unconfirmed) {
                setDisabled([$('#confirmAnnualReport'), $('#cancelAnnualReport')], false);
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
        var annualReportId = tr.find(':first').html();
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
        var annualReportId = tr.find(':first').html();
        $.ajax({
            url: '/Documentation/ConfirmAnnualReport',
            type: 'GET',
            cache: false,
            data: { id: annualReportId },
            success: function (message) {
                tr.find(':last').html(AnnualReportStatus.Confirmed);
                setDisabled([$('#confirmAnnualReport'), $('#cancelAnnualReport')], true);
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
    })

    $('#cancelAnnualReport').click(function (e) {
        e.preventDefault();
        e.stopPropagation();
        var tr = $('#AnnualReportsTable tr.row-selected:first');
        var annualReportId = tr.find(':first').html();
        $.ajax({
            url: '/Documentation/CancelAnnualReport',
            type: 'GET',
            cache: false,
            data: { id: annualReportId },
            success: function (message) {
                tr.find(':last').html(AnnualReportStatus.Canceled);
                setDisabled([$('#confirmAnnualReport'), $('#cancelAnnualReport')], true);
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
    })

    $('#CreateAnnualReport').click(function (e) {
        e.preventDefault();
        e.stopPropagation();
        var cityId = $('#CitiesList option').filter(':selected').val();
        var strURL = '/Documentation/CreateAnnualReportAsAdmin?cityId=' + cityId;
        window.open(strURL, '_self')
    })
});

$('#annual-report-form').ready(function () {
    if ($('#ModalSuccess .modal-body:first p:first strong:first').contents().length != 0) {
        $('#ModalSuccess').modal('show');
        $('#CreateAnnualReport').prop('disabled', true);
    }
    else {
        if ($('#ModalError .modal-body:first p:first strong:first').contents().length != 0) {
            $('#ModalError').modal('show');
            $('#CreateAnnualReport').prop('disabled', true);
        }
    }
})

function setDisabled(elements: JQuery<HTMLElement>[], disabled: boolean) {
    for (let el of elements)
        el.prop('disabled', disabled);
}

function showModalMessage(modalWindow: JQuery<HTMLElement>, message: string) {
    modalWindow.find('.modal-body:first p:first strong:first').html(message);
    modalWindow.modal('show');
}