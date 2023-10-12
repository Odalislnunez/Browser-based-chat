"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

//Disable the send button until connection is established.
document.getElementById("sendButton").disabled = true;

connection.on("ReceiveMessage", function (messages) {
    var ul = document.getElementById("messagesList");

    ul.innerHTML = '';
    messages.forEach(function (message) {
        var li = document.createElement("li");
        li.textContent = `${message.user}: ${message.msg} (${message.time})`;
        ul.appendChild(li);
    });
});

connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;
    var roomID = document.getElementById("roomIdInput").value;
    connection.invoke("JoinGroup", roomID).catch(function (err) {
        return console.error(err.toString());
    });
    connection.invoke("GetRoomChats", roomID).catch(function (err) {
        return console.error(err.toString());
    });
}).catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("sendButton").addEventListener("click", function (event) {
    var roomID = document.getElementById("roomIdInput").value;
    var msg = document.getElementById("messageInput").value;
    var email = document.getElementById("emailInput").value;

    connection.invoke("SendMessage", msg, roomID, email).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});
