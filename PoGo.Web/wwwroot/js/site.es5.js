'use strict';

$(function () {
    $('[data-toggle="tooltip"]').tooltip();

    setTimeout(function () {
        if (typeof window.qs === 'undefined') $("#adblockAlert").removeAttr("hidden");
    }, 2000);
});

