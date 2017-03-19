using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using SentenceAnalyserCore;

namespace SentenceAnalyser
{
    public class Program
    {
        static void Main(string[] args)
        {

            var txtFiles = Directory.GetFiles("Data", "*.txt")
                                     .Select(Path.GetFileNameWithoutExtension)
                                     .ToArray();

            foreach (var file in txtFiles)
            {
                var analyse = new Analyse();
                var analysis = analyse.Text(File.ReadAllText($@"Data\{file}.txt"));
                Directory.CreateDirectory("Output");
                File.WriteAllText($@"Output\{file}_data.json", JsonConvert.SerializeObject(analysis));
            }        
        }
    }
}
