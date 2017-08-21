$(function () {
    $('[data-toggle="tooltip"]').tooltip();

    setTimeout(() => {
        if (typeof window.qs === 'undefined')
            $("#adblockAlert").removeAttr("hidden");
    }, 2000);
});