using System.IO;
using DataDisplay.Models;
using Newtonsoft.Json;

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

        public static void WriteRawContent(string content)
        {
            lock (RawFileLock)
            {
                using (var writer = File.AppendText(RawContentFile))
                {
                    writer.Write(content);
                }
            }
        }

        public static void UpdateWorkingFile()
        {
            lock (WorkingFileLock)
            {
                var inputFile = $"C:\\Hack24Input\\working_Data.json";

                File.Delete(inputFile);

                //var analyse = new Analyse();
                //var analysis = analyse.Text(File.ReadAllText($@"Data\{file}.txt"));
                //Directory.CreateDirectory("Output");
                //File.WriteAllText($@"Output\{file}_data.json", JsonConvert.SerializeObject(analysis));
            }
        }
    }
}