function uploadFiles(inputId) {
    let input = document.getElementById(inputId);
    var files = input.files;
    var formData = new FormData();
    formData.append("ID", $('#eventId').val());
    for (var i = 0; i != files.length; i++) {
        formData.append("files", files[i]);
    }
    $.ajax({
        url: "/Action/FillEventGallery",
        data: formData,
        processData: false,
        contentType: false,
        type: "POST",
        success: function (data) {
            $("#uploadModal").modal('show');
            $('#carouselBlock').load(document.URL + ' #carouselBlock');
        },
        error: function () {
            $("#FAIL").modal('show');
        },
    });
}
//# sourceMappingURL=eventGallery.js.map