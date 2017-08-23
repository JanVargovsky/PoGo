'use strict';

$(function () {
    $('[data-toggle="tooltip"]').tooltip();

    setTimeout(function () {
        if (typeof window.qs === 'undefined' && $(".ads").length != 0) $("#adblockAlert").removeAttr("hidden");
    }, 2000);
});

