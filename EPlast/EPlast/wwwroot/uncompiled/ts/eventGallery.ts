function uploadFiles(inputId: string) {
    let input: HTMLInputElement = <HTMLInputElement>document.getElementById(inputId);
    var files = input.files;
    var formData = new FormData();
    formData.append("ID", <string>$('#eventId').val());
    for (var i = 0; i != files.length; i++) {
        formData.append("files", files[i]);
    }
    $.ajax(
        {
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