// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(document).ready(function () {

    //Enable Tooltip everywhere
    $('[data-toggle="tooltip"]').tooltip()

    //Show toast if any
    $('.toast').toast('show');
});