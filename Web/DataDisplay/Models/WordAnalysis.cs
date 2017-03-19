using System.Collections.Generic;
using SentenceAnalyserCore;

namespace DataDisplay.Models
{
    public class WordAnalysis
    {
        public HeatmapDataPoint[] RankedSentences { get; set; }
        public string[] Words { get; set; }

        public SentenceWordAnalysis[] NegativeWords { get; set; }
        public SentenceWordAnalysis[] PositiveWords { get; set; }
        public SentenceWordAnalysis[] ArousingWords { get; set; }
        public SentenceWordAnalysis[] BoredWords { get; set; }

        public SentenceWordAnalysis[] ExcitedWords { get; set; }
        public SentenceWordAnalysis[] AngryWords { get; set; }
        public SentenceWordAnalysis[] DepressedWords { get; set; }
        public SentenceWordAnalysis[] CalmWords { get; set; }
    }
}