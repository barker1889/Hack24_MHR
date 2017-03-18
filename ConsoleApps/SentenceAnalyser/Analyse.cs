using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;

namespace SentenceAnalyser
{
    public class Analyse
    {
        public RankedOutput Text(string text)
        {

            var lexiconStream = Assembly
                .GetExecutingAssembly()
                .GetManifestResourceStream("SentenceAnalyser.output_depechemoode.txt");

            var wordScores = JsonConvert.DeserializeObject<List<WordScore>>(StreamToString(lexiconStream));
            var sentenceDataOneLine = text.Replace("\r\n", " ");
            var sentenceData = sentenceDataOneLine.Split(new[] { ".", "?", "!", "..." }, StringSplitOptions.RemoveEmptyEntries);
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

                    wordList.Add(word);

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

            var output = new RankedOutput
                         {
                             RankedSentences = sentenceList.Select(sentence => new RankedSentence
                                                                               {
                                                                                   Arousal = sentence.Value[0],
                                                                                   Valence = sentence.Value[1],
                                                                                   Input = sentence.Key
                                                                               }).ToList(),
                             Words = wordList
                         };

            return output;
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

    public class RankedOutput
    {
        public List<RankedSentence> RankedSentences { get; set; }

        public List<string> Words { get; set; }
    }
}
