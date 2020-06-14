function readURL(input) {
    $('#image').removeAttr("src");
    if (input.files && input.files[0]) {
       
        var reader = new FileReader();

        reader.onload = function (e) {
            $('#image').attr('src', e.target.result);
        };

        reader.readAsDataURL(input.files[0]);
    }
}
$("#imgInput").change(function () {
    readURL(this);
});

function readPDF(input) {
    $('#pdf').removeAttr("hidden");  
    $('#pdf').removeAttr("src");
    if (input.files && input.files[0]) {
       
        var reader = new FileReader();

        reader.onload = function (e) {
            $('#pdf').attr('src', e.target.result);
        };

        reader.readAsDataURL(input.files[0]);
    }
    
}
$("#pdfInput").change(function () {
    
    readPDF(this);
});