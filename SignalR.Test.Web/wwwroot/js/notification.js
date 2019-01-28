"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/notificationHub").build();

connection.on("ReceiveNotification", function (message) {
    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    var li = document.createElement("li");
    li.textContent = msg;
    document.getElementById("messagesList").appendChild(li);
});

connection.on("ReceiveTask", function (taskName) {
    var li = document.createElement("li");
    li.textContent = taskName;
    document.getElementById("taksList").appendChild(li);
});

connection.start().catch(function (err) {
    return console.error(err.toString());
});

//document.getElementById("sendMessage").addEventListener("click", function (event) {
//    var user = document.getElementById("userInput").value;
//    var message = document.getElementById("messageInput").value;
//    connection.invoke("SendNotification", user, message).catch(function (err) {
//        return console.error(err.toString());
//    });
//    event.preventDefault();
//});

document.getElementById("sendMessage").addEventListener("click", function (event) {
    var user = document.getElementById("userInput").value;
    var messageText = document.getElementById("messageInput").value;

    $.ajax({
        type: 'POST',
        url: 'api/Notification/SendMessage?userName=' + user + '&messageText=' + messageText
    });

    event.preventDefault();
});

document.getElementById("sendTask").addEventListener("click", function (event) {
    var taskName = document.getElementById("taskNameInput").value;

    $.ajax({
        type: 'POST',
        url: 'api/Notification?taskName=' + taskName
    });

    event.preventDefault();
});