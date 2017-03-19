using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;

namespace SentenceAnalyserCore
{
    public class Analyse
    {
        public RankedOutput Text(string text)
        {

            var lexiconStream = Assembly
                .GetExecutingAssembly()
                .GetManifestResourceStream("SentenceAnalyserCore.output_depechemoode.txt");

            var wordScores = JsonConvert.DeserializeObject<List<WordScore>>(StreamToString(lexiconStream));
            var sentenceDataOneLine = text.Replace("\r\n", " ").Replace("\n", " ").Replace(",", "");
            var sentenceData = sentenceDataOneLine.Split(new[] { ".", "?", "!", "..." }, StringSplitOptions.RemoveEmptyEntries);
            var wordCountDictionary = new Dictionary<string, int>();
            var wordList = new List<string>();

            var sentenceList = new Dictionary<string, decimal[]>();

            foreach (var sentence in sentenceData)
            {
                var wordCount = 0;
                var sentenceScore = new decimal[] { 0, 0, 0, 0 };
                foreach (var word in sentence.Split(' '))
                {
                    if (string.IsNullOrEmpty(word)) continue;
                    var strippedWord = word.StripPunctuation();

                    var foundWord = wordScores.FirstOrDefault(w => w.Word == strippedWord);
                    if (foundWord == null) continue;

                    if (wordCountDictionary.ContainsKey(strippedWord))
                    {
                        wordCountDictionary[strippedWord]++;
                    }
                    else
                    {
                        wordCountDictionary.Add(strippedWord, 1);
                    }

                    wordCount++;

                    sentenceScore[0] += foundWord.Arousal;
                    sentenceScore[1] += foundWord.Valence;
                }

                if (wordCount != 0)
                {
                    sentenceScore[2] = sentenceScore[0] / wordCount;
                    sentenceScore[3] = sentenceScore[1] / wordCount;
                }

                if (!sentenceList.ContainsKey(sentence))
                    sentenceList.Add(sentence, sentenceScore);
            }

            wordCountDictionary.OrderByDescending(kv => kv.Value);
            wordList = wordCountDictionary.Keys.ToList();


            var rankedSentences = sentenceList.Select(sentence => new RankedSentence
            {
                Arousal = sentence.Value[0],
                Valence = sentence.Value[1],
                Input = sentence.Key
            }).ToList();


            var commonWordSampleSize = 50;

            var topNegativeSentences = rankedSentences
                .Where(s => !string.IsNullOrWhiteSpace(s.Input)).OrderBy(s => s.Valence).Take(commonWordSampleSize);
            var topPositiveSentences = rankedSentences
                .Where(s => !string.IsNullOrWhiteSpace(s.Input)).OrderByDescending(s => s.Valence).Take(commonWordSampleSize);
            var topArousingSentences = rankedSentences
                .Where(s => !string.IsNullOrWhiteSpace(s.Input)).OrderByDescending(s => s.Arousal).Take(commonWordSampleSize);
            var topBoringSentences = rankedSentences
                .Where(s => !string.IsNullOrWhiteSpace(s.Input)).OrderBy(s => s.Arousal).Take(commonWordSampleSize);

            var output = new RankedOutput
            {
                RankedSentences = rankedSentences,
                Words = wordList,
                NegativeWords = GetCommonWords(topNegativeSentences),
                PositiveWords = GetCommonWords(topPositiveSentences),
                ArousingWords = GetCommonWords(topArousingSentences),
                BoredWords = GetCommonWords(topBoringSentences)
            };

            return output;
        }

        private List<SentenceWordAnalysis> GetCommonWords(IEnumerable<RankedSentence> sentence)
        {
            var result = new List<SentenceWordAnalysis>();

            foreach (var rankedSentence in sentence)
            {
                foreach (var sentenceWord in rankedSentence.Input.Split(' '))
                {
                    var existingSentenceWordAnalysis = result.FirstOrDefault(w => w.Word.Equals(sentenceWord, StringComparison.OrdinalIgnoreCase));

                    if (existingSentenceWordAnalysis != null)
                    {
                        existingSentenceWordAnalysis.Count++;
                    }
                    else
                    {
                        result.Add(new SentenceWordAnalysis {Count = 1, Word = sentenceWord});
                    }
                }
            }

            return result;
        }

        private static string StreamToString(Stream stream)
        {
            stream.Position = 0;
            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
            {
                return reader.ReadToEnd();
            }
        }
    }

    public class SentenceWordAnalysis
    {
        public string Word { get; set; }
        public int Count { get; set; }
    }

    public class RankedOutput
    {
        public List<RankedSentence> RankedSentences { get; set; }

        public List<string> Words { get; set; }

        public List<SentenceWordAnalysis> NegativeWords { get; set; }
        public List<SentenceWordAnalysis> PositiveWords { get; set; }
        public List<SentenceWordAnalysis> ArousingWords { get; set; }
        public List<SentenceWordAnalysis> BoredWords { get; set; }

        public List<SentenceWordAnalysis> ExcitedWords { get; set; }
        public List<SentenceWordAnalysis> AngryWords { get; set; }
        public List<SentenceWordAnalysis> DepressedWords { get; set; }
        public List<SentenceWordAnalysis> CalmWords { get; set; }
    }
}
