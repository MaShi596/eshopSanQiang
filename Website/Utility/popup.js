$(document).ready(function() {
    if ($("#editwindow").length > 0) {
        $("#editwindow").bind("click", function() { $("#floatBoxItem").show(); });
    }

    if ($("#floatBoxItem .title span").length > 0) {
        $("#floatBoxItem .title span").bind("click", function() { $("#floatBoxItem").hide(); });
    }

    if ($("#floatBoxItem .bottonHandle .short-btn-close").length > 0) {
        $("#floatBoxItem .bottonHandle .short-btn-close").bind("click", function() { $("#floatBoxItem").hide(); });
    }
});