document.getElementById("createButton").addEventListener("click", function (event) {
    var roomName = document.getElementById("roomInput").value;
    var email = document.getElementById("emailInput").value;

    $.ajax({
        type: "POST",
        url: '/RoomChat/Create',
        dataType: "JSON",
        data: JSON.stringify({ email: email, description: roomName }),
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            location.reload();
        },
        error: function (error) {
            alert(error.message);
        }
    });

    document.getElementById("roomInput").value = '';

    event.preventDefault();
});

function DeleteButton (roomId) {
    var email = document.getElementById("emailInput").value;

    $.ajax({
        type: "POST",
        url: '/RoomChat/Delete',
        dataType: "JSON",
        data: JSON.stringify({ email: email, roomId: roomId }),
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            location.reload();
        },
        error: function (error) {
            alert(error.responseText);
        }
    });
}
