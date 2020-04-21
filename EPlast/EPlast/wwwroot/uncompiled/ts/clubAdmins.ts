declare const CurrentClub: number;
let ClubAdminId: number;
$(document).ready(function () {

    $('#dtClubAdmins').DataTable({
        "language": {
            "url": "https://cdn.datatables.net/plug-ins/1.10.20/i18n/Ukrainian.json"
        }
    });
    $('.changeEnddatebutton').click(function (event) {
        $('#changeEnddate').modal('show');
        const button = event.target;
        ClubAdminId = $(button).data('adminId');
    });
});

function ChooseEndDate() {
    const input = <HTMLInputElement>$('#txtToEndDate')[0];
    const date = input.valueAsDate;
    console.log(input);
    console.log(date);
    console.log(ClubAdminId);


    $.ajax({
        url: '/Club/AddEndDate',
        method: 'POST',
        data: JSON.stringify({ enddate: date, clubIndex: CurrentClub, adminId: ClubAdminId }),
        contentType: 'application/json; charset=utf-8',
        timeout: 5000
    }).done((result) => {
        if (result === 1) {
            $('a[data-adminId="' + ClubAdminId + '"]').parent().siblings('.ClubAdminEndDate')[0].innerHTML = date.toLocaleDateString();
        }
    });
}