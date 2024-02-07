// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(document).ready(function () {

    //AJAX Search and Pagination Script
    function RefreshPaginationElements() {
        if ($('#PrevFlag').val() == "False") {
            $('#PreviousBtn').removeClass("btn-success");
            $('#PreviousBtn').addClass("disabled");
        }
        else {
            $('#PreviousBtn').addClass("btn-success");
            $('#PreviousBtn').removeClass("disabled");
        }
        if ($('#NextFlag').val() == "False") {
            $('#NextBtn').removeClass("btn-success");
            $('#NextBtn').addClass("disabled");
        }
        else {
            $('#NextBtn').addClass("btn-success");
            $('#NextBtn').removeClass("disabled");
        }

        var StartIndex = $('#StartIndex').val();
        var LastIndex = $('#LastIndex').val();
        var TotalItems = $('#TotalItems').val();
        var string = (TotalItems == 0) ? 'No entries found' : 'Showing ' + StartIndex + ' to ' + LastIndex + ' of ' + TotalItems + ' entries';

        $('#ItemIndex').val(string);
    }

    RefreshPaginationElements()

    var pathname = window.location.pathname;
    pathname = pathname + '/Search';

    $(document).keyup(function (event) {
        if ($("#SearchField").is(":focus") && event.key == "Enter") {
            var searchType = $('#SearchType').val();
            var searchInput = $("#SearchField").val();

            $.ajax({
                url: pathname,
                type: "post",
                data: { "searchType": searchType, "searchInput": searchInput },
                async: true,
                success: function (data) {
                    $("#UserList").html(data);
                    RefreshPaginationElements();
                }
            });
        }
    });
    
    $("#Search").click(function (e) {
        var searchType = $('#SearchType').val();
        var searchInput = $("#SearchField").val();

        $.ajax({
            url: pathname,
            type: "post",
            data: { "searchType": searchType, "searchInput": searchInput },
            async: true,
            success: function (data) {
                $("#UserList").html(data);
                RefreshPaginationElements();
            }
        });
    });

    $('select').on('change', function () {
        var searchType = null;
        var searchInput = null;
        $("#SearchField").val('');

        $.ajax({
            url: pathname,
            type: "post",
            data: { "searchType": searchType, "searchInput": searchInput },
            async: true,
            success: function (data) {
                $("#UserList").html(data);
                RefreshPaginationElements();
            }
        });
    });

    $("#PreviousBtn").click(function () {
        var searchType = $('#SearchType').val();
        var searchInput = $("#SearchField").val();
        var pageNumber = parseInt($('#PageIndex').val(), 10) - 1;

        $.ajax({
            url: pathname,
            type: "post",
            data: { "searchType": searchType, "searchInput": searchInput, "pageNumber": pageNumber },
            async: true,
            success: function (data) {
                $("#UserList").html(data);
                RefreshPaginationElements();
            }
        });
    });

    $("#NextBtn").click(function () {
        var searchType = $('#SearchType').val();
        var searchInput = $("#SearchField").val();
        var pageNumber = parseInt($('#PageIndex').val(), 10) + 1;

        $.ajax({
            url: pathname,
            type: "post",
            data: { "searchType": searchType, "searchInput": searchInput, "pageNumber": pageNumber },
            async: true,
            success: function (data) {
                $("#UserList").html(data);
                RefreshPaginationElements();
            }
        });
    });
});