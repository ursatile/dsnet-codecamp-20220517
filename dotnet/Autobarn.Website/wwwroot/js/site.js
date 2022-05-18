
function displayNotification(user, message) {
    console.log(user);
    console.log(message);
    const data = JSON.parse(message);
    const $div = $(`
<div>${data.Make} ${data.Model} (${data.Year}, ${data.Color})<br />
PRICE: ${data.Price} ${data.CurrencyCode}<br />
<a href="/vehicles/details/${data.Registration}">click for more!</a>
</div>`);
    const $container = $("#signalr-notifications");
    $div.css("background-color", data.Color);
    $container.prepend($div);
    window.setTimeout(function() {
            $div.fadeOut(2000,
                function() {
                    $div.remove();
                });
        },
        5000);
}
function connectToSignalR() {
    var conn = new signalR.HubConnectionBuilder().withUrl("/hub").build();
    conn.on("ThisIsAMagicString", displayNotification);
    conn.start().then(function() {
        console.log('Connected to SignalR!');
    }).catch(function(err) {
        console.log(`Connection failed: ${err}`);
    });
}

$(document).ready(connectToSignalR);
