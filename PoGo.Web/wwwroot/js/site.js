$(function () {
    $('[data-toggle="tooltip"]').tooltip();

    setTimeout(() => {
        if ($(".ads > iframe").length === 0)
            $("#adblockAlert").removeAttr("hidden");
    }, 2000);
});