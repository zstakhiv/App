$(document).ready(function () {
    initialise();
    var arr = ["#Decesion-Name", "#datepicker", "#Decesion-Description", "#autocomplete_input"];
    $(() => {
        $("#datepicker").datepicker({ dateFormat: "dd-mm-yy" }).datepicker("setDate", "0");
    });
    $(".show_hide").on('click', function () {
        $(this).parent("td").children(".hidden").removeClass("hidden");
        $(this).hide();
    });
    createDecesionDataTable();
    function ClearFormData() {
        arr.forEach(function (element) {
            $(element).val("");
        });
    }
    function CheckFormData() {
        var bool = true;
        arr.forEach(function (element) {
            if ($(element).val().toString().length == 0) {
                console.log($(element).val().toString().length);
                $(element).parent("div").children(".field-validation-valid").text("Це поле має бути заповнене.");
                bool = false;
            }
            else
                $(element).parent("div").children(".field-validation-valid").text("");
        });
        if (!bool)
            return false;
        return true;
    }
    $("#CreateDecesionForm-submit").click((e) => {
        e.preventDefault();
        e.stopPropagation();
        if (!CheckFormData())
            return;
        let input = document.getElementById("CreateDecesionFormFile");
        var files = input.files;
        if (files[0] != undefined && files[0].size >= 10485760) {
            alert("файл за великий (більше 10 Мб)");
            return;
        }
        $("#CreateDecesionForm-submit").prop('disabled', true);
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
        formData.append("Decesion.Date", decesionDate.split("-").reverse().join("-"));
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
                $("#CreateDecesionForm-submit").prop('disabled', false);
                if (response.success) {
                    ClearFormData();
                    $("#CreateDecesionModal").modal("hide");
                    $("#ModalSuccess .modal-body:first p:first strong:first").html(response.text);
                    $("#ModalSuccess").modal("show");
                    var file = "";
                    if (files[0] != undefined) {
                        file = `<a asp-controller="Documentation" asp-action="Download" asp-route-id="${response.id}" asp-route-filename="${files[0].name}">${files[0].name}</a>`;
                    }
                    $("#dtReadDecesion").DataTable().row.add([response.id, response.decesionOrganization, decesionDecesionStatusType, decesionTargetName, decesionDescription, decesionDate, file])
                        .draw();
                }
                else {
                    $("#CreateDecesionModal").modal("hide");
                    $("#ModalError.modal-body:first p:first strong:first").html("Не можливо додати звіт!");
                }
            },
            error() {
                $("#CreateDecesionForm-submit").prop('disabled', false);
                $("#CreateDecesionModal").modal("hide");
                $("#ModalError.modal-body:first p:first strong:first").html("Не можливо додати звіт!");
            }
        });
    });
});
$(document).ajaxComplete(function () {
    initialise();
});
function initialise() {
    $("tr.decesion-click-row").dblclick(function () {
        const content = $(this).children().first().text();
        window.open(`/Documentation/CreatePDFAsync?objId=${content}`, "_blank");
    });
}
function createDecesionDataTable() {
    $("#dtReadDecesion").one("preInit.dt", function () {
        var button = $(`<button id="createDecesionButton" class="btn btn-sm btn-primary btn-management" data-toggle="modal" data-target="#CreateDecesionModal">Додати нове рішення</button>`);
        $("#dtReadDecesion_filter label").append(button);
    });
    $("#dtReadDecesion").DataTable({
        "language": {
            "url": "https://cdn.datatables.net/plug-ins/1.10.20/i18n/Ukrainian.json"
        },
        responsive: true,
        "createdRow": function (row, data, dataIndex) {
            $(row).addClass("decesion-click-row");
        },
    });
    $('#dtReadDecesion').on('page.dt', function () {
        $('html, body').animate({
            scrollTop: 100
        }, 200);
    });
}
//# sourceMappingURL=decesion.js.map