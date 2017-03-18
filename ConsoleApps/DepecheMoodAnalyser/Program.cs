using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DepecheMoodAnalyser
{
    class Program
    {
        static void Main(string[] args)
        {
            var lexiconFile = "Data\\DepecheMood_normfreq.txt";

            var words = new List<WordScore>();

            using (var fs = File.OpenRead(lexiconFile))
            {
                using (var reader = new StreamReader(fs))
                {
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        words.Add(new WordScore(line));
                    }
                }
            }

            var calcualtedWords = new List<ValenceArousalScore>();

            foreach (var wordScore in words)
            {
                calcualtedWords.Add(new ValenceArousalScore
                {
                    Word = wordScore.Word,
                    Valance = CalculatePositiveValance(wordScore) - CalculateNegativeValance(wordScore),
                    Arousal = CalculatePositiveArousal(wordScore) - CalculateNegativeArousal(wordScore)
                });
            }

            var outputValue = JsonConvert.SerializeObject(calcualtedWords.ToArray(), Formatting.Indented);

            File.WriteAllText("output_depechemoode.txt", outputValue);
        }

        private static double CalculatePositiveValance(WordScore word)
        {
            return word.Happy 
                + word.Amused
                + word.Inspired;
        }

        private static double CalculateNegativeValance(WordScore word)
        {
            return word.Afraid
                + word.Sad
                + word.Angry
                + word.Annoyed;
        }

        private static double CalculatePositiveArousal(WordScore word)
        {
            return word.Inspired
                + word.Angry;
        }

        private static double CalculateNegativeArousal(WordScore word)
        {
            return word.Sad
                + word.DontCare;
        }
    }

    public class ValenceArousalScore
    {
        public string Word { get; set; }
        public double Valance { get; set; }
        public double Arousal { get; set; }
    }

    public class WordScore
    {
        public WordScore(string line)
        {
            var parts = line.Split('\t');

            Word = parts[0].Split('#')[0];
            Afraid = double.Parse(parts[1]);
            Amused = double.Parse(parts[2]);
            Angry = double.Parse(parts[3]);
            Annoyed = double.Parse(parts[4]);
            DontCare = double.Parse(parts[5]);
            Happy = double.Parse(parts[6]);
            Inspired = double.Parse(parts[7]);
            Sad = double.Parse(parts[8]);
        }

        public string Word { get; set; }
        public double Afraid { get; set; }
        public double Amused { get; set; }
        public double Angry { get; set; }
        public double Annoyed { get; set; }
        public double DontCare { get; set; }
        public double Happy { get; set; }
        public double Inspired { get; set; }
        public double Sad { get; set; }
    }
}
