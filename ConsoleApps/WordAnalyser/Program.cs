using System.Collections.Generic;
using System.Linq;
using ConsoleApplication1;
using Newtonsoft.Json;

namespace WordAnalyser
{
    class Program
    {
        private static readonly Dictionary<string,decimal> happyWords = new Dictionary<string,decimal>()
                                                          {
                                                                { "joy", 0.8m },
                                                                { "positive", 0.5m },
                                                                { "surprise", 0.2m },
                                                                { "trust", 0.1m }
                                                          };
        private static readonly Dictionary<string, decimal> unhappyWords = new Dictionary<string, decimal>()
                                                          {
                                                                { "anger", -0.75m },
                                                                { "disgust", -0.9m },
                                                                { "fear", -0.3m },
                                                                { "negative", -0.5m }
                                                          };
        private static readonly Dictionary<string, decimal> excitedWords = new Dictionary<string, decimal>()
                                                          {
                                                                { "anger", 0.7m },
                                                                { "anticipation", 0.5m },
                                                                { "disgust", 0.3m },
                                                                { "fear", 0.8m },
                                                                { "joy", 0.7m },
                                                                { "surprise", 0.6m  }
                                                          };
        static void Main(string[] args)
        {
            var wordScoreList = new List<WordScore>();
            var lines = System.IO.File.ReadAllLines(@"lexicon.txt");
            var currentWord = new WordScore();
            foreach (var line in lines)
            {
                var wordAndScore = line.Split('\t');

                if (currentWord.Word != wordAndScore[0])
                {
                    if (currentWord.Association != null)
                    {
                        AssignHappy(currentWord);
                        AssignExcitement(currentWord);
                    }

                    var wordScore = new WordScore
                                    {
                                        Word = wordAndScore[0],
                                        Association = new Dictionary<string, bool>()
                                                      {
                                                          {
                                                              wordAndScore[1],
                                                              wordAndScore[2] == "1"
                                                          }
                                                      }
                                    };

                    wordScoreList.Add(wordScore);
                    currentWord = wordScore;
                }
                else
                {
                    currentWord.Association.Add(wordAndScore[1], wordAndScore[2] == "1");
                }
                
                
            }

            var jsonWordScoreList = JsonConvert.SerializeObject(wordScoreList, Formatting.Indented);
            System.IO.File.WriteAllText("lexicon.json", jsonWordScoreList);
        }

        private static void AssignExcitement(WordScore currentWord)
        {
            currentWord.Arousal = currentWord.Association.Where(a => excitedWords.Keys.Contains(a.Key) && a.Value).Sum(word => excitedWords[word.Key]);
        }

        private static void AssignHappy(WordScore currentWord)
        {

            var happyTotal = currentWord.Association.Where(a => happyWords.Keys.Contains(a.Key) && a.Value).Sum(word => happyWords[word.Key]);
            var unhappyTotal = currentWord.Association.Where(a => unhappyWords.Keys.Contains(a.Key) && a.Value).Sum(word => unhappyWords[word.Key]);

            currentWord.Valence = happyTotal + unhappyTotal;
        }
    }
}
