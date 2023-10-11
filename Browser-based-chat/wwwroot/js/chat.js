"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

//Disable the send button until connection is established.
document.getElementById("sendButton").disabled = true;

connection.on("ReceiveMessage", function (user, message, time) {
    var li = document.createElement("li");
    document.getElementById("messagesList").appendChild(li);
    // We can assign user-supplied strings to an element's textContent because it
    // is not interpreted as markup. If you're assigning in any other way, you 
    // should be aware of possible script injection concerns.
    li.textContent = `${user}: ${message} (${time})`;
});

connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;
    var roomID = document.getElementById("roomIdInput").value;
    connection.invoke("JoinGroup", roomID).catch(function (err) {
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
