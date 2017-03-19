using System.IO;
using DataDisplay.Models;
using Newtonsoft.Json;
using SentenceAnalyserCore;

namespace DataDisplay.Data
{
    public static class DataFile
    {
        private const string RawContentFile = "C:\\Hack24Input\\RawContent.txt";

        private static readonly object FileLock = new object();
        private static readonly object WorkingFileLock = new object();
        private static readonly object RawFileLock = new object();

        public static WordAnalysis GetContents(string filename)
        {
            if (string.IsNullOrWhiteSpace(filename) || filename == "null")
            {
                filename = "working";
            }

            var inputFile = $"C:\\Hack24Input\\{filename}_Data.json";

            lock (filename == "working" ?  WorkingFileLock : FileLock)
            {
                using (var fs = File.OpenRead(inputFile))
                {
                    using (var reader = new StreamReader(fs))
                    {
                        var fileContents = reader.ReadToEnd();
                        return JsonConvert.DeserializeObject<WordAnalysis>(fileContents);
                    }
                }
            }
        }

        public static void AppendRawContent(string content)
        {
            lock (RawFileLock)
            {
                using (var writer = File.AppendText(RawContentFile))
                {
                    writer.Write(content + ".");
                }
            }
        }

        public static void UpdateWorkingFile()
        {
            var workingDataOutput = $"C:\\Hack24Input\\working_Data.json";

            lock (WorkingFileLock)
            {
                if (File.Exists(workingDataOutput))
                {
                    File.Delete(workingDataOutput);
                }
                
                var analysis = new Analyse().Text(File.ReadAllText(RawContentFile));
                File.WriteAllText(workingDataOutput, JsonConvert.SerializeObject(analysis));
            }
        }
    }
}