using CSVEditor.Dialogues;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Controls;

namespace CSVEditor
{
    internal class FileWorker
    {
        private FileOrder _order = new FileOrder();
        private DialogueAssembler _dialogueAssembler = new DialogueAssembler();

        private DataGrid _grid;
        private ObservableCollection<Dialogue> _data;

        private string _currentPath = "";
        private int _pageID = 1;

        private const int limitCellsPerPage = 10;

        public void Initialize(Button[] buttons, DataGrid grid)
        {
            _order.Initialize(buttons);
            _grid = grid;
        }

        public void CreateTemplate()
        {
            FileInfo file = _dialogueAssembler.CreateTemplate();
            SetCurrentFile(Path.GetFileNameWithoutExtension(file.Name), file.FullName);
        }

        public void SetCurrentFile(string name, string path)
        {
            _order.ChangeFile(Path.GetFileNameWithoutExtension(name), path);

            ObservableCollection<Dialogue> data = new ObservableCollection<Dialogue>();

            string[] currentData = File.ReadAllLines(path);

            for (int i = 1; i < _pageID * limitCellsPerPage; i++)
            {
                if (i + 1 > currentData.Length)
                    break;

                string[] parts = _dialogueAssembler.GetParsedDialogue(currentData[i]);

                if (parts.Length == 5)
                    data.Add(new Dialogue() { Question = parts[0], Answer01 = parts[1], Answer02 = parts[2], Answer03 = parts[3], Answer04 = parts[4] });
            }

            _grid.ItemsSource = data;
        }

        public void ClearFile()
        {
            _order.ClearFile();
        }
    }
}
