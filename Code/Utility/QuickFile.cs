using System;
using System.IO;
using System.Threading.Tasks;

namespace CSVEditor.Utility
{
    internal static class QuickFile
    {
        public static async void RewriteLine(string path, int lineIndex, string value)
        {
            await Task.Run(() => 
            {
                string[] data = File.ReadAllLines(path);
                data[lineIndex] = value.Replace(Environment.NewLine, "[next_line]");
                File.WriteAllLines(path, data);
            });
        }

        public async static void AddLine(string path, string content)
        {
            using (StreamWriter writer = new StreamWriter(path, true))
            {
                await writer.WriteLineAsync(content);
            }
        }
    }
}
