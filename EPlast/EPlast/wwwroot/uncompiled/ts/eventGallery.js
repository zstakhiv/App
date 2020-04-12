function uploadFiles(inputId) {
    let input = document.getElementById(inputId);
    var files = input.files;
    var formData = new FormData();
    formData.append("ID", $('#eventId').val());
    for (var i = 0; i != files.length; i++) {
        formData.append("files", files[i]);
    }
    $("#carouselBlock").addClass("progress-cursor");
    $("#files").addClass("progress-cursor");
    $.ajax({
        url: "/Action/FillEventGallery",
        data: formData,
        processData: false,
        contentType: false,
        type: "POST",
        success: function () {
            $("#uploadModal").modal('show');
        },
        error: function () {
            $("#carouselBlock").removeClass("progress-cursor");
            $("#files").removeClass("progress-cursor");
            $("#FAIL").modal('show');
        },
    });
}
//# sourceMappingURL=eventGallery.js.map