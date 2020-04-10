$(document).ready(() => {
    $(() => {
        $("#datepicker").datepicker({ dateFormat: "yy/mm/dd" }).datepicker("setDate", "0");
    });
    $("#dtReadRaport").DataTable({
        "language": {
            "url": "https://cdn.datatables.net/plug-ins/1.10.20/i18n/Ukrainian.json"
        }
    });
    $('#dtReadRaport').on('page.dt', function () {
        $('html, body').animate({
            scrollTop: 100
        }, 200);
    });
    $("tr.raport-click-row").dblclick(function () {
        const content = $(this).children().first().text();
        window.open(`/Documentation/CreatePDFAsync?objId=${content}`, "_blank");
    });
    $("#CreateDecesionForm-submit").click((e) => {
        e.preventDefault();
        e.stopPropagation();
        let input = document.getElementById("CreateDecesionFormFile");
        var files = input.files;
        var formData = new FormData();
        var decesionName = $("#Decesion-Name").val().toString();
        var decesionOrganizationId = $("#Decesion-Organization-ID option:selected").val().toString();
        var decesionTargetName = $("#autocomplete_input").val().toString();
        var decesionTargetId = $("#autocomplete_input_id_0").val().toString();
        var decesionDate = $("#datepicker").datepicker().val().toString();
        var decesionDescription = $("#Decesion-Description").val().toString();
        var decesionDecesionStatusType = $("#Decesion-DecesionStatusType option:selected").text();
        formData.append("file", files[0]);
        formData.append("Decesion.Name", decesionName);
        formData.append("Decesion.Organization.ID", decesionOrganizationId);
        formData.append("Decesion.DecesionTarget.TargetName", decesionTargetName);
        formData.append("Decesion.DecesionTarget.ID", decesionTargetId);
        formData.append("Decesion.Date", decesionDate);
        formData.append("Decesion.Description", decesionDescription);
        formData.append("Decesion.DecesionStatusType", decesionDecesionStatusType);
        $.ajax({
            url: "/Documentation/SaveDecesionAsync",
            type: "POST",
            processData: false,
            contentType: false,
            enctype: "multipart/form-data",
            async: true,
            data: formData,
            success(response) {
                if (response.success) {
                    $("#CreateDecesionModal").modal("hide");
                    $("#ModalSuccess .modal-body:first p:first strong:first").html(response.text);
                    $("#ModalSuccess").modal("show");
                }
                else {
                    $("#CreateDecesionModal").modal("hide");
                    $("#ModalError.modal-body:first p:first strong:first").html("Не можливо додати звіт!");
                }
            },
            error() {
                $("#CreateDecesionModal").modal("hide");
                $("#ModalError.modal-body:first p:first strong:first").html("Не можливо додати звіт!");
            }
        });
    });
});
//# sourceMappingURL=decesion.js.map