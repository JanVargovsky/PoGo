$(function () {
    $('[data-toggle="tooltip"]').tooltip();

    setTimeout(() => {
        if (typeof window.qs === 'undefined' && $(".ads").length !== 0)
            $("#adblockAlert").removeAttr("hidden");
    }, 2000);
});