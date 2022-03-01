$(document).ready(function () {
    fillLocations();
    setTomorrow();
});

function fillLocations() {
    $.ajax({
        url: "/home/getlocations",
        type: 'POST',
        success(data) {
            $("#SourceId").empty();
            $("#DestinationId").empty();

            $.each(data, function () {
                $("#SourceId").append($("<option>"
                    + "</option> ").val(this['value']).html(this['text']));

                $("#DestinationId").append($("<option>"
                    + "</option> ").val(this['value']).html(this['text']));
            });
        }
    });
}

function setTomorrow() {
    var today = new Date();
    var tomorrow = new Date();

    tomorrow.setDate(today.getDate() + 1);

    var dd = tomorrow.getDate();
    var mm = tomorrow.getMonth() + 1; 

    var yyyy = tomorrow.getFullYear();
    if (dd < 10) { dd = '0' + dd } if (mm < 10) { mm = '0' + mm } today = yyyy + '-' + mm + '-' + dd;
    $('#dateInput').attr('value', today);
    $("#tomorrow").css("background", "#707070");
    $("#today").css("background", "#fff");
}

function setToday() {
    var today = new Date();

    var dd = today.getDate();
    var mm = today.getMonth() + 1;

    var yyyy = today.getFullYear();
    if (dd < 10) { dd = '0' + dd } if (mm < 10) { mm = '0' + mm } today = yyyy + '-' + mm + '-' + dd;
    $('#dateInput').attr('value', today);
    $("#today").css("background", "#707070");
    $("#tomorrow").css("background", "#fff");
}