﻿using System.Collections.Generic;

namespace SentenceAnalyser
{
    public class WordScore
    {
        public string Word { get; set; }
        public Dictionary<string, bool> Association { get; set; }
        public decimal Valence { get; set; }
        public decimal Arousal { get; set; }
    }
}
