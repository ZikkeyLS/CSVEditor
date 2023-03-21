using System.Collections.Generic;
using System.IO;

namespace CSVEditor.Dialogues
{
    internal class DialogueAssembler
    {
        private const string FolderName = "Dialogues";
        private const string FileFormat = "Question,Answer01,Answer02,Answer03,Answer04";

        public FileInfo CreateTemplate()
        {
            TryMakeDirectory();

            int id = Directory.GetFiles(FolderName).Length + 1; // order by 1-st, 2-nd, etc...
            string filePath = $"{FolderName}/Dialogue.{id}";

            File.Create(filePath).Close();
            File.WriteAllText(filePath, FileFormat);

            return new FileInfo(filePath);
        }

        public string[] GetParsedDialogue(List<string> fileData, int index)
        {
            if (index >= fileData.Count)
                return new string[] {};

            string initial = fileData[index];
            return initial.Split(',');
        }

        public void AddDialogue(List<string> fileData, string question, string answer01, string answer02, string answer03, string answer04)
        {
            fileData.Add($"{question},{answer01},{answer02},{answer03},{answer04}");
        }

        public void RemoveDialogue(List<string> fileData, int index)
        {
            fileData.RemoveAt(index);
        }

        public string GetCompiledDialogue(string[] parts)
        {
            string final = "";

            for (int i = 0; i < parts.Length; i++)
            {
                string separator = i != parts.Length - 1 ? "," : "";
                final += parts[i] + separator;
            }

            return final;
        }

        public FileInfo GetFile(string path) => new FileInfo(path);

        public void RemoveTemplate(string name)
        {
            File.Delete($"{FolderName}/{name}");
        }

        private void TryMakeDirectory()
        {
            if (Directory.Exists(FolderName) == false)
                Directory.CreateDirectory(FolderName);
        }
    }
}
