"use strict";

$(function () {
    $('[data-toggle="tooltip"]').tooltip();

    setTimeout(function () {
        if ($(".ads > iframe").length === 0) $("#adblockAlert").removeAttr("hidden");
    }, 2000);
});

