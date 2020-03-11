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