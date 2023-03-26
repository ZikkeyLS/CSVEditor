using CSVEditor.Dialogues;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CSVEditor
{
    public partial class MainWindow : Window
    {
        public ObservableCollection<Dialogue> Dialogues { get; set; } = new ObservableCollection<Dialogue>();
        private FileWorker _fileWorker = new FileWorker();

        public MainWindow()
        {
            InitializeComponent();
            _fileWorker.Initialize(new Button[3] { File01, File02, File03 }, DialoguesGrid, Dialogues, DialogueCount);
        }

        private void OnTextBoxKeyDown(object sender, KeyEventArgs e)
        {
            if (Key.Return == e.Key &&
                0 < (ModifierKeys.Shift & e.KeyboardDevice.Modifiers))
            {
                var tb = (TextBox)sender;
                var caret = tb.CaretIndex;
                tb.Text = tb.Text.Insert(caret, Environment.NewLine);
                tb.CaretIndex = caret + 1;
                e.Handled = true;
            }
        }

        private void MinimizeApplicationButtonClick(object sender, MouseButtonEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void CloseApplicationButtonClick(object sender, MouseButtonEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void CreateFileClick(object sender, MouseButtonEventArgs e)
        {
            _fileWorker.CreateTemplate();
        }

        private void SearchFileClick(object sender, MouseButtonEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.DefaultExt = ".csv";
            dialog.Filter = "CSV Files (*.csv)|*.csv";
            dialog.Multiselect = false;

            Nullable<bool> result = dialog.ShowDialog();

            if (result == true)
                _fileWorker.SetCurrentFile(Path.GetFileNameWithoutExtension(dialog.SafeFileName), dialog.FileName);
        }

        private void ClearFileClick(object sender, MouseButtonEventArgs e)
        {
            _fileWorker.ClearFile();
        }

        private void BorderClick(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                DragMove();
        }

        private void BorderLeftClick(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount != 2)
                return;

            if (WindowState == WindowState.Maximized)
            {
                WindowState = WindowState.Normal;
                Width = 1080;
                Height = 720;
            }
            else
            {
                WindowState = WindowState.Maximized;
            }

        }

        private void AddRowClick(object sender, RoutedEventArgs e)
        {
            if(_fileWorker.Linked)
                _fileWorker.AddRow();
        }
    }
}
