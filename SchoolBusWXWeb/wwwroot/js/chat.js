var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub?groupName=onegroup").build();

connection.on("ReceiveMessage", function (user, message) {
    var encodedMsg = user + " says " + message;
    console.log(encodedMsg);
});

connection.start().then(function () {}).catch(function (err) {
    return console.error(err.toString());
});

//document.getElementById("sendButton").addEventListener("click", function (event) {
//    var user = document.getElementById("userInput").value;
//    var message = document.getElementById("messageInput").value;
//    connection.invoke("SendMessage", user, message).catch(function (err) {
//        return console.error(err.toString());
//    });
//    event.preventDefault();
//});