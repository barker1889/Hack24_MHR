window.requestAnimationFrame = window.requestAnimationFrame || window.mozRequestAnimationFrame ||
                               window.webkitRequestAnimationFrame || window.msRequestAnimationFrame;

var heat = simpleheat('heatmap-canvas')
        .max(18);

$.ajax('/api/heatmapdata?width=700&height=600')
    .done(function(e) {
        console.log(e);
        heat
            .data(e)
            .draw();
    });