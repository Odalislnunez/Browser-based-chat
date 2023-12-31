"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

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

connection.on("ReceiveMessageCommand", function (user, message, time, count) {
    var li = document.createElement("li");
    li.textContent = `${user}: ${message} (${time})`;

    var ul = document.getElementById("messagesList");
    ul.insertBefore(li, ul.firstChild);

    if (count > 50) {
        var lastLi = ul.querySelector('li:last-child');
        if (lastLi) {
            ul.removeChild(lastLi);
        }
    }
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

    if (msg.toLowerCase().includes("/stock=")) {
        $.ajax({
            type: "POST",
            url: '/RoomChat/GetStockQuote',
            dataType: "JSON",
            data: JSON.stringify({msg: msg, roomId: roomID, email: email}),
            contentType: "application/json; charset=utf-8",
            success: function (data) {},
            error: function (error) {}
        });
    }
    else {
        connection.invoke("SendMessage", msg, roomID, email).catch(function (err) {
            return console.error(err.toString());
        });
    }

    document.getElementById("messageInput").value = '';

    event.preventDefault();
});

function handleKeyPress(event) {
    if (event.key === "Enter") {
        document.getElementById('sendButton').click();
    }
}
