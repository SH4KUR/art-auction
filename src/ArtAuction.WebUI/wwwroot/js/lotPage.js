"use strict"

var auctionNumber = parseInt(document.getElementById('auctionNumber').value);

var connection = new signalR.HubConnectionBuilder().withUrl('/lotPageHub').build();

connection.on('RefreshChatMessages', function () {
    fetch(auctionNumber + '/GetMessages', {
            method: 'GET',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            }
        })
        .then(response => response.json())
        .then(data => showMessages(data))
        .function(error => console.error('Unable to fetch messages.', error));
});

connection.on('RefreshCurrentPrice', function () {

});

connection
    .start()
    .then(function () {
        connection.invoke('JoinAuctionLotRoom', auctionNumber);
        connection.invoke('RefreshChatMessages', auctionNumber);
    })
    .catch(function (err) {
        return console.error(err.toString());
    });

function showMessages(messages) {
    messages.forEach(message => {
        var messageElem = document.createElement('li');
        messageElem.textContent = '[' + message.dateTime + ']' + message.userLogin + ' - ' + message.messageText;

        document.getElementById('chat').appendChild(messageElem);
    });
}

function sendMessage() {

}