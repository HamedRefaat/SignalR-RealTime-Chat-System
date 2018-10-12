$(document).ready(function () {

    $("#UserEmail").focusout(function () {

        var val = $(this).val();

        var re = /^(([^<>()[\]\\.,;:\s@@\"]+(\.[^<>()[\]\\.,;:\s@@\"]+)*)|(\".+\"))@@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;

        if (re.test(val)) {

            $.ajax(
                {
                    url: "/Account/AlredyExiestes",
                    type: "POST",
                    data: { Email: val }


                }

            ).done(function (response) {
                $("#valmal").text(response);
                if (response != "")
                    toastr.error(response);
                $("#UserEmail").effect("shake", { direction: "right", times: 10, distance: 5 }, 1000);


            }).fail(function (response) {


            });
        }

    });

});