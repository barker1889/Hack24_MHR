window.requestAnimationFrame = window.requestAnimationFrame || window.mozRequestAnimationFrame ||
                               window.webkitRequestAnimationFrame || window.msRequestAnimationFrame;

var heat = simpleheat('heatmap-canvas')
        .max(18), frame;

var radius = 20;
var blur = 20;

function draw() {
    heat.draw();
    frame = null;
}

var file = getParameterByName('filename', window.location);

console.log(file);

$.ajax('/api/heatmapdata?width=600&height=600&filename=' + file)
    .done(function(e) {
        heat.data(e);
        draw();
    });

$('input[type=range]').on('input', function () {
    $(this).trigger('change');
});

$('#range-blur').on('change', function(e) {
    blur = $(this).val();

    heat.radius(+radius, +blur);
    frame = frame || window.requestAnimationFrame(draw);
});

$('#range-intensity').on('change', function (e) {
    radius = $(this).val();

    heat.radius(+radius, +blur);
    frame = frame || window.requestAnimationFrame(draw);
});

function getParameterByName(name, url) {
    if (!url) {
        url = window.location.href;
    }
    name = name.replace(/[\[\]]/g, "\\$&");
    var regex = new RegExp("[?&]" + name + "(=([^&#]*)|&|#|$)"),
        results = regex.exec(url);
    if (!results) return null;
    if (!results[2]) return '';
    return decodeURIComponent(results[2].replace(/\+/g, " "));
}