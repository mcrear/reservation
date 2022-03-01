$(document).ready(function () {
    fillLocations();
    fillDate();
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

function fillDate() {
    document.getElementById("dateInput").value = '2022-03-01';
}