using CSVEditor.Dialogues;
using CSVEditor.Utility;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Controls;

namespace CSVEditor
{
    internal class FileWorker
    {
        public bool Linked { get; private set; } = false;
        public bool AllSelected { get; set; } = false;
        public int MaxPages { get ; private set; }

        private FileOrder _order = new FileOrder();
        private DialogueAssembler _dialogueAssembler = new DialogueAssembler();

        private ObservableCollection<Dialogue> _dialogues;

        private DataGrid _grid;
        private TextBlock _dialogueCount;

        private string _currentPath = "";

        public void Initialize(Button[] buttons, DataGrid grid, ObservableCollection<Dialogue> data, TextBlock dialogueCount)
        {
            _order.Initialize(buttons);
            _dialogues = data;
            _dialogueCount = dialogueCount;

            _grid = grid;
            _grid.CurrentCellChanged += DialoguesGridCurrentCellChanged;
        }

        public void AddRow()
        {
            _dialogues.Add(new Dialogue() { Index = (_dialogues.Count + 1).ToString(), Selected = AllSelected,
                Question = "...", Answer01 = "...", Answer02 = "...", Answer03 = "...", Answer04 = "..." });

            QuickFile.AddLine(_currentPath, "...,...,...,...,...");

            _dialogueCount.Text = $"{_dialogues.Count} dialogues";
        }

        public void DeleteSelectedRows()
        {
            for (int i = _dialogues.Count - 1; i >= 0; i--)
                if (_dialogues[i].Selected)
                {
                    List<string> data = File.ReadAllLines(_currentPath).ToList();
                    data.RemoveAt(i + 1);
                    File.WriteAllLines(_currentPath, data);
                }

            for (int i = _dialogues.Count - 1; i >= 0; i--)
                if (_dialogues[i].Selected)
                    _dialogues.RemoveAt(i);

            _dialogueCount.Text = $"{_dialogues.Count} dialogues";
        }

        private void DialoguesGridCurrentCellChanged(object sender, EventArgs e)
        {
            DataGrid dataGrid = sender as DataGrid;

            if (dataGrid.SelectedCells.Count > 0)
                for (int i = 0; i < _dialogues.Count; i++)
                    if (_dialogues[i] == _grid.SelectedItem)
                    {
                        Dialogue temp = new Dialogue
                        {
                            Question = _dialogues[i].Question.Replace(",", "[comma]"),
                            Answer01 = _dialogues[i].Answer01.Replace(",", "[comma]"),
                            Answer02 = _dialogues[i].Answer02.Replace(",", "[comma]"),
                            Answer03 = _dialogues[i].Answer03.Replace(",", "[comma]"),
                            Answer04 = _dialogues[i].Answer04.Replace(",", "[comma]")
                        };

                        string value = _dialogueAssembler.GetCompiledDialogue(temp);
                        int lineIndex = i + 1;
                        QuickFile.RewriteLine(_currentPath, lineIndex, value);
                    }
        }

        private void CellEdit(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (e.EditAction != DataGridEditAction.Commit)
                return;
        }

        public void CreateTemplate()
        {
            FileInfo file = _dialogueAssembler.CreateTemplate();
            SetCurrentFile(Path.GetFileNameWithoutExtension(file.Name), file.FullName);
        }

        public void SetCurrentFile(int index)
        {
            DialogueFile dialogue = _order.SetIndexedFile(index);

            if (dialogue != null)
                UpdateFile(dialogue.Path);
        }

        public void SetCurrentFile(string name, string path)
        {
            _order.ChangeFile(Path.GetFileNameWithoutExtension(name), path);
            UpdateFile(path);
        }

        private void UpdateFile(string path)
        {
            _currentPath = path;

            _dialogues.Clear();

            string[] currentData = File.ReadAllLines(path);

            for (int i = 1; i < currentData.Length; i++)
            {
                string[] parts = _dialogueAssembler.GetParsedDialogue(currentData[i]);

                if (parts.Length == 5)
                    _dialogues.Add(new Dialogue() { Index = (_dialogues.Count + 1).ToString(), Selected = AllSelected, Question = parts[0], Answer01 = parts[1], Answer02 = parts[2], Answer03 = parts[3], Answer04 = parts[4] });
            }

            _dialogueCount.Text = $"{_dialogues.Count} dialogues";
            _grid.ItemsSource = _dialogues;

            Linked = true;
        }

        public void ClearFile()
        {
            _order.ClearFile();
        }
    }
}
