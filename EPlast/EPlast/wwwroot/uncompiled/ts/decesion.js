$(document).ready(() => {
    $(() => {
        $("#datepicker").datepicker({ dateFormat: "yy/mm/dd" }).datepicker("setDate", "0");
    });
    $("#dtReadRaport").DataTable({
        "language": {
            "url": "https://cdn.datatables.net/plug-ins/1.10.20/i18n/Ukrainian.json"
        }
    });
    $("tr.raport-click-row").dblclick(function () {
        const content = $(this).children().first().text();
        window.open(`/Documentation/CreatePDFAsync?objId=${content}`, "_blank");
    });
    $("#CreateDecesionForm-submit").click(e => {
        e.preventDefault();
        e.stopPropagation();
        var formData = $("#CreateDecesionForm").serialize();
        $.ajax({
            url: "/Documentation/SaveDecesionAsync",
            type: "POST",
            cache: false,
            enctype: "multipart/form-data",
            data: formData,
            async: true,
            success(response) {
                if (response.success) {
                    $("#CreateDecesionModal").modal("hide");
                    $("#ModalSuccess .modal-body:first p:first strong:first").html(response.Text);
                    $("#ModalSuccess").modal("show");
                }
            },
            error() {
                $("#CreateDecesionModal").modal("hide");
                $("#ModalError.modal-body:first p:first strong:first").html("�� ������� ������ ���!");
            }
        });
    });
});
//# sourceMappingURL=decesion.js.map