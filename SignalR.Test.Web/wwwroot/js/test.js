document.getElementById("getDocFile").addEventListener("click", function (event) {
    $.ajax({
        type: 'GET',
        url: 'api/Notification/GetFile',
        success: function (data) {
            alert(data);
        },
        error: function (error) {
            alert(error);
        }
    });

    //$.ajax({
    //    type: 'GET',
    //    url: 'http://185.59.101.152:8080/API/REST/Files/Download?uid=16827568-0b4e-4a09-a36f-f5a83a1e5911',
    //    headers: {
    //        "AuthToken": "51f8e6b7a-1973-4748-8a63-a5941f63fb35"
    //    },
    //    contentType: "application/json",
    //    crossDomain: true,
    //    dataType: 'jsonp',
    //    success: function (data) {
    //        alert(data);
    //    },
    //    error: function (xhr, status, error) {
    //        alert(error);
    //    }
    //});

    event.preventDefault();
});