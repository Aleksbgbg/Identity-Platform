$(document).ready(function() {
    const image = $("img#user-image");

    $("input.form-control-file").change(function() {
        if (this.files === undefined || this.files[0] === undefined) {
            return;
        }

        const fileReader = new FileReader();

        fileReader.onload = function(e) {
            image.attr("src", e.target.result);
        }

        fileReader.readAsDataURL(this.files[0]);
    });
});