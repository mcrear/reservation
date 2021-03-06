// sayfa ilk ayağa kalktığında çalışacak olan fonksiyon
$(document).ready(function () {
    // lokasyon listelerinin doldurulması
    fillLocations();

    // bir sonraki günün tarih alanına açılışta setlenmesi
    setTomorrow();

    // datepicker içerisindeki alandan minimum tarih değeri yapılandırması
    setMin();

    //Select2 nesneleri için search metodlarının tanımlanması
    search();

    // Takas işlemi için görsele onClick eklenmesi
    changeId();
});

function fillLocations() {
    $.ajax({
        url: "/home/getlocations",
        type: 'POST',
        success(data) {
            $("#SourceId").empty();
            $("#DestinationId").empty();
            var source = $("#SetSourceId").val();
            var destination = $("#SetDestinationId").val();
            var value;
            var text;
            $.each(data, function () {

                value = this['value'];
                text = this['text'];

                if (value == source)
                    $("#SourceId").append($("<option selected>"
                        + "</option> ").val(value).html(text));
                else
                    $("#SourceId").append($("<option>"
                        + "</option> ").val(value).html(text));


                if (value == destination)
                    $("#DestinationId").append($("<option selected>"
                        + "</option> ").val(value).html(text));
                else
                    $("#DestinationId").append($("<option>"
                        + "</option> ").val(value).html(text));


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

function setMin() {
    var today = new Date();

    var dd = today.getDate();
    var mm = today.getMonth() + 1;

    var yyyy = today.getFullYear();
    if (dd < 10) { dd = '0' + dd } if (mm < 10) { mm = '0' + mm } today = yyyy + '-' + mm + '-' + dd;
    $('#dateInput').attr('min', today);
}

function validateForm() {
    var source = $("#SourceId").val();
    var destination = $("#DestinationId").val();

    if (source == destination) {
        alert("Kalkış ve Varış noktaları aynı olamaz.");
        return false;
    }

    var today = new Date();

    var date = new Date($('#dateInput').val());
    var day = date.getDate();
    var month = date.getMonth() + 1;
    var year = date.getFullYear();


    if (dateInput < today) {
        alert("Geçmiş bir tarihe ait seferleri sorgulayamazsınız.");
        return false;
    }
}

function search() {
    $('#DestinationId, #SourceId').select2({
        ajax: {
            url: '/home/searchlocations',
            type: 'POST',
            data: function (params) {
                var query = {
                    search: params.term
                }
                return query;
            },
            processResults: function (data) {

                // Transforms the top-level key of the response object from 'items' to 'results'
                return {
                    results: data
                };
            }
        }
    });
}

function changeId() {

    $("#img_changeId").click(function () {
        var tempsource = $("#SourceId").val()
        var tempdestination = $("#DestinationId").val()
        $('#SourceId').val(tempdestination).trigger('change');
        $('#DestinationId').val(tempsource).trigger('change');
    })

}
