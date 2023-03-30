using System.Windows.Controls;

namespace CSVEditor
{
    public class DialogueFile
    {
        public string Name;
        public string Path;
    }

    internal class FileOrder
    {
        private Button[] _buttons;
        private DialogueFile[] _dialogueFiles = new DialogueFile[3] { new DialogueFile(), new DialogueFile(), new DialogueFile() };
        private int _currentIndex = -1;

        public void Initialize(Button[] fileButtons)
        {
            _buttons = fileButtons;
        }

        public void ChangeFile(string name, string path)
        {
            _currentIndex = (_currentIndex == _dialogueFiles.Length - 1)? 0 : _currentIndex + 1;
            _dialogueFiles[_currentIndex].Name = name;
            _dialogueFiles[_currentIndex].Path = path;

            _buttons[_currentIndex].Content = name;
        }

        public DialogueFile SetIndexedFile(int index)
        {
            if (_dialogueFiles[index].Name == "")
                return null;

            _currentIndex = index;
            return _dialogueFiles[index];
        }
        
        public void ClearFile()
        {
            if (_currentIndex == -1)
                return;

            _dialogueFiles[_currentIndex].Name = "";
            _dialogueFiles[_currentIndex].Path = "";

            _buttons[_currentIndex].Content = "";
            _currentIndex = _currentIndex == 0 ? -1 : _currentIndex - 1;
        }
    }
}
