$.ajax('/api/wordcloud?filename=' + file)
    .done(function (e) {
        d3.wordcloud()
            .size([500, 300])
            .fill(d3.scale.ordinal().range(["#884400", "#448800", "#888800", "#444400"]))
            .words(e)
            .start();
    });