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


        var positiveWordString = ''
        for (var idx = 0; idx < e.PositiveWords.length; idx++) {
            positiveWordString += '<span>' + e.PositiveWords[idx].Word + ',&nbsp;</span>';
        }

        $('#positive-word-associated')[0].innerHTML = positiveWordString;


        //public WordCloudModel[] WordCloudModels { get; set; }
        //public SentenceWordAnalysis[] PositiveWords { get; set; }
        //public SentenceWordAnalysis[] NegativeWords { get; set; }
        //public SentenceWordAnalysis[] ArousingWords { get; set; }
        //public SentenceWordAnalysis[] CalmWords { get; set; }
    });