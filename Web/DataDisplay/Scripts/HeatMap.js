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

$.ajax('/api/heatmapdata?width=600&height=600')
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

