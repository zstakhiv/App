declare const CurrentClubInClubMembers: number;
let ClubMemberId: number;
$(document).ready(function () {

    $('.ClubMemberToAdmin').click(function (event) {
        $('#addClubAdmin').modal('show');
        const button = $(event.target).parent();
        ClubMemberId = button.data('adminid');
        console.log(ClubAdminId);
    });
});

function AddClubAdmin() {
    const input = <HTMLInputElement>$('#AdminTypeName')[0];
    const AdminTypeName = input.value;
    if (AdminTypeName === null) {
        alert('������ ����� ����������');
        return;
    }
    alert(AdminTypeName);

    const input2 = <HTMLInputElement>$('#ClubAdminStartDate')[0];
    const StartDateValue = input2.value;
    const StartDate = StartDateValue + ' 00:00:00';
    if (StartDateValue === null) {
        alert('������ ���� ������� ����������');
        return;
    }
    alert(StartDate);


    const input3 = <HTMLInputElement>$('#ClubAdminEndDate')[0];
    const EndDateValue = input3.value;
    let EndDate = null;
    if (EndDateValue !== '-') {
        EndDate = EndDateValue + ' 00:00:00';
    }
    alert(EndDateValue);


    const adminsData = {
        adminId: ClubMemberId,
        clubIndex: CurrentClub,
        enddate: EndDate,
        startdate: StartDate,
        AdminType: AdminTypeName
    };

    $.ajax({
        url: '/Club/AddToClubAdministration',
        type: 'POST',
        data: JSON.stringify(adminsData),
        contentType: 'application/json; charset=utf-8',
        timeout: 5000
    }).done((result) => {
        if (result === 1) {
            alert('������ ������� ���� ������ ������');
        }
    });
}