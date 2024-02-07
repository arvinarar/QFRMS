$(document).ready(function () {
    //Message box hide
    $("#xButtonSuccess").click(function (e) {
        $('#messageBoxSuccess').AddClass("hide");
    });
    $("#xButtonError").click(function (e) {
        $('#messageBoxError').AddClass("hide");
    });
});