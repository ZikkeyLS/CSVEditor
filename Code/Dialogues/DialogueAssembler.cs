using System;
using System.Collections.Generic;
using System.IO;

namespace CSVEditor.Dialogues
{
    internal class DialogueAssembler
    {
        private const string FolderName = "Dialogues";
        private const string FileFormat = "Question,Answer01,Answer02,Answer03,Answer04\n";

        public FileInfo CreateTemplate()
        {
            TryMakeDirectory();

            int id = Directory.GetFiles(FolderName).Length + 1; // order by 1-st, 2-nd, etc...
            string filePath = $"{FolderName}/Dialogue({id}).csv";

            File.Create(filePath).Close();
            File.WriteAllText(filePath, FileFormat);

            return new FileInfo(filePath);
        }

        public string[] GetParsedDialogue(string data)
        {
            string[] final = data.Replace("[next_line]", Environment.NewLine).Split(',');
            
            for (int i = 0; i < final.Length; i++)
                final[i] = final[i].Replace("[comma]", ",");

            return final;
        }

        public string GetCompiledDialogue(Dialogue dialogue)
        {
            return $"{dialogue.Question},{dialogue.Answer01},{dialogue.Answer02},{dialogue.Answer03},{dialogue.Answer04}";
        }

        private void TryMakeDirectory()
        {
            if (Directory.Exists(FolderName) == false)
                Directory.CreateDirectory(FolderName);
        }
    }
}
