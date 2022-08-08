$(document).ready(function () {

    $(".for-js").click(function (e) {
        
        e.preventDefault();

        $(".check-item").click(function () {
            if ($(this).is(":checked")) {
                console.log($(this).val());
            }
        });

    });

});
