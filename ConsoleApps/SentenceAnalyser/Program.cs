using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace SentenceAnalyser
{
    class Program
    {
        static void Main(string[] args)
        {
            var wordScores = JsonConvert.DeserializeObject<List<WordScore>>(System.IO.File.ReadAllText(@"output_depechemoode.txt"));

            var txtFiles = Directory.GetFiles("Data", "*.txt")
                                     .Select(Path.GetFileNameWithoutExtension)
                                     .ToArray();

            foreach (var file in txtFiles)
            {
                var sentenceDataOneLine = File.ReadAllText($@"Data\{file}.txt").Replace("\r\n", " ");
                var sentenceData = sentenceDataOneLine.Split(new[] { ".", "?", "!", "..." }, StringSplitOptions.RemoveEmptyEntries);

                var sentenceList = new Dictionary<string, decimal[]>();

                foreach (var sentence in sentenceData.Take(3000))
                {
                    var wordCount = 0;
                    var sentenceScore = new decimal[] { 0, 0, 0, 0 };
                    foreach (var word in sentence.Split(' '))
                    {
                        if (string.IsNullOrEmpty(word)) continue;
                        var strippedWord = word.StripPunctuation();

                        var foundWord = wordScores.FirstOrDefault(w => w.Word == strippedWord);
                        if (foundWord == null) continue;

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
                    //Console.WriteLine($"Sentence Score = {sentenceScore} :: Sentence Total = {sentenceTotal[0]}, {sentenceTotal[1]}");
                    //Console.ReadLine();
                }

                var arousalMax = sentenceList.OrderByDescending(s => s.Value[0]).First();
                var valenceMax = sentenceList.OrderByDescending(s => s.Value[1]).First();

                var arousalMin = sentenceList.OrderByDescending(s => s.Value[0]).Last();
                var valenceMin = sentenceList.OrderByDescending(s => s.Value[1]).Last();

                Console.WriteLine($"Highest Arousal: { arousalMax.Value[0] } -- { arousalMax.Key }");
                Console.WriteLine($"Highest Valence: { valenceMax.Value[1] } -- { valenceMax.Key}");

                Console.WriteLine($"Lowest Arousal: { arousalMin.Value[0] } -- { arousalMin.Key }");
                Console.WriteLine($"Lowest Valence: { valenceMin.Value[1] } -- { valenceMin.Key}");


                var arousalMaxAvg = sentenceList.OrderByDescending(s => s.Value[2]).First();
                var valenceMaxAvg = sentenceList.OrderByDescending(s => s.Value[3]).First();

                var arousalMinAvg = sentenceList.OrderByDescending(s => s.Value[2]).Last();
                var valenceMinAvg = sentenceList.OrderByDescending(s => s.Value[3]).Last();

                Console.WriteLine($"Highest Arousal Avg: { arousalMaxAvg.Value[2] } -- { arousalMaxAvg.Key }");
                Console.WriteLine($"Highest Valence Avg: { valenceMaxAvg.Value[3] } -- { valenceMaxAvg.Key}");

                Console.WriteLine($"Lowest Arousal Avg: { arousalMinAvg.Value[2] } -- { arousalMinAvg.Key }");
                Console.WriteLine($"Lowest Valence Avg: { valenceMinAvg.Value[3] } -- { valenceMinAvg.Key}");

                var rankedList = sentenceList.Select(sentence => new RankedSentence
                {
                    Arousal = sentence.Value[0],
                    Valence = sentence.Value[1],
                    Input = sentence.Key
                }).ToList();

                var rankedListAvg = sentenceList.Select(sentence => new RankedSentence
                {
                    Arousal = sentence.Value[2],
                    Valence = sentence.Value[3],
                    Input = sentence.Key
                }).ToList();

                Directory.CreateDirectory("Output");
                File.WriteAllText($@"Output\{file}_Data.json", JsonConvert.SerializeObject(rankedList, Formatting.Indented));
                File.WriteAllText($@"Output\{file}_DataAverage.json", JsonConvert.SerializeObject(rankedListAvg, Formatting.Indented));
            }
          
        }
    }

    public static class StringExtension
    {
        public static string StripPunctuation(this string s)
        {
            var sb = new StringBuilder();
            foreach (char c in s)
            {
                if (!char.IsPunctuation(c))
                    sb.Append(c);
            }
            return sb.ToString();
        }
    }

    public class RankedSentence
    {
        public decimal Valence { get; set; }
        public decimal Arousal { get; set; }
        public string Input { get; set; }
    }
}
