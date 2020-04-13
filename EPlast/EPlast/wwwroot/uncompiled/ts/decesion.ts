function initialise() {
    $("tr.raport-click-row").dblclick(function () {
        const content = $(this).children().first().text();
        window.open(`/Documentation/CreatePDFAsync?objId=${content}`, "_blank");
    });
}

$(document).ready(function () {
    initialise();

    $(() => {
        $("#datepicker").datepicker({ dateFormat: "dd-mm-yy" }).datepicker("setDate", "0");
    });

    $(".show_hide").on('click',function () {
        $(this).parent("td").children(".hidden").removeClass("hidden");
        $(this).hide();
    });

    $("#dtReadRaport").DataTable({
        "language": {
            "url": "https://cdn.datatables.net/plug-ins/1.10.20/i18n/Ukrainian.json"
        },
        responsive: true,
        "createdRow": function (row, data, dataIndex) {
            $(row).addClass("raport-click-row");
        }
    });

    $('#dtReadRaport').on('page.dt', function () {
        $('html, body').animate({
            scrollTop: 100
        }, 200);
    });

    $("#CreateDecesionForm-submit").click((e) => {
        e.preventDefault();
        e.stopPropagation();
        let input: HTMLInputElement = <HTMLInputElement>document.getElementById("CreateDecesionFormFile");
        var files = input.files;
        if (files[0] != undefined && files[0].size >= 10485760) {
            alert("файл за великий (більше 10 Мб)");
            return;
        }
        var formData = new FormData();
        var decesionName = $("#Decesion-Name").val().toString();
        var decesionOrganizationId = $("#Decesion-Organization-ID option:selected").val().toString();
        var decesionTargetName = $("#autocomplete_input").val().toString();
        var decesionTargetId = $("#autocomplete_input_id_0").val().toString();
        var decesionDate = $("#datepicker").datepicker().val().toString();
        var decesionDescription = $("#Decesion-Description").val().toString()
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
                    var file = "";
                    if (files[0] != undefined) {
                        file = `<a asp-controller="Documentation" asp-action="Download" asp-route-id="${response.id}" asp-route-filename="${files[0].name}">${files[0].name}</a>`
                    }
                    $("#dtReadRaport").DataTable().row.add([response.id, response.decesionOrganization, decesionDecesionStatusType, decesionTargetName, decesionDescription, decesionDate, file])
                        .draw();
                } else {
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
$(document).ajaxComplete(function () {
    initialise();
});