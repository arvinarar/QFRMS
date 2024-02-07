const weekday = ["Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"];
const monthNames = ["January", "February", "March", "April", "May", "June",
        "July", "August", "September", "October", "November", "December"
];

function setDateTime() {
    var d = new Date();
    var day = weekday[d.getDay()] + ", ";
    var date = d.getDate() + ", ";
    var month = monthNames[d.getMonth()] + " ";
    var year = d.getFullYear() + " ";

    var hour = (d.getHours() == 0) ? 12 : (d.getHours() > 12) ? d.getHours() - 12 : d.getHours();
    var minute = (d.getMinutes() < 10) ? "0" + d.getMinutes() : d.getMinutes();
    var second = (d.getSeconds() < 10) ? "0" + d.getSeconds() : d.getSeconds();
    var timeOfDay = (d.getHours() < 12) ? " AM" : " PM";
    
    var time = hour + ":" + minute + ":" + second + timeOfDay;
    return day + month + date + year + time;
}

setInterval(function () {
    var currentTime = setDateTime();
    document.getElementById("timer").innerHTML = currentTime;
}, 1000);

$(document).ready(function () {
    var currentTime = setDateTime();
    document.getElementById("timer").innerHTML = currentTime;
});