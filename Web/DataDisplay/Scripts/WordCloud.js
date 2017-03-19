$.ajax('/api/wordcloud?filename=' + file)
    .done(function (e) {

        console.log(e);

        d3.wordcloud()
            .size([500, 300])
            .fill(d3.scale.ordinal().range(["#884400", "#448800", "#888800", "#444400"]))
            .words(e.WordCloudModels)
            .start();

//        <div id="positive-word-associated"></div>
    
//<h2>Words associated with negative sentences</h2>
//<div id="negative-word-associated"></div>
    
//<h2>Words associated with arousing sentences</h2>
//<div id="arousing-word-associated"></div>
    
//<h2>Words associated with calm sentences</h2>
        //<div id="calm-word-associated"></div>


        var positiveWordString = '';
        for (var idx = 0; idx < e.PositiveWords.length && idx < 100; idx++) {
            positiveWordString += e.PositiveWords[idx].Word + ', ';
        }
        $('#positive-word-associated')[0].innerHTML = '<p>' + positiveWordString + '</p>';

        var negativeWordString = ''
        for (var idx = 0; idx < e.NegativeWords.length && idx < 100; idx++) {
            negativeWordString += e.NegativeWords[idx].Word + ', ';
        }
        $('#negative-word-associated')[0].innerHTML = '<p>' + negativeWordString + '</p>';

        var arousingWordString = ''
        for (var idx = 0; idx < e.ArousingWords.length && idx < 100; idx++) {
            arousingWordString += e.ArousingWords[idx].Word + ', ';
        }
        $('#arousing-word-associated')[0].innerHTML = '<p>' + arousingWordString + '</p>';


        var calmWordString = ''
        for (var idx = 0; idx < e.CalmWords.length && idx < 100; idx++) {
            calmWordString += e.CalmWords[idx].Word + ', ';
        }
        $('#calm-word-associated')[0].innerHTML = '<p>' + calmWordString + '</p>';

        //public WordCloudModel[] WordCloudModels { get; set; }
        //public SentenceWordAnalysis[] PositiveWords { get; set; }
        //public SentenceWordAnalysis[] NegativeWords { get; set; }
        //public SentenceWordAnalysis[] ArousingWords { get; set; }
        //public SentenceWordAnalysis[] CalmWords { get; set; }
    });