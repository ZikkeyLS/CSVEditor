using CSVEditor.Dialogues;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace CSVEditor
{
    public partial class MainWindow : Window
    {
        private DialogueAssembler _dialogueAssembler = new DialogueAssembler();
        private FileOrder _fileOrder = new FileOrder();

        public MainWindow()
        {
            InitializeComponent();
            _fileOrder.Initialize(new Button[3] { File01, File02, File03 });

            // var brushConverter = new BrushConverter();

            /*
                ObservableCollection<Dialogue> dialogues = new ObservableCollection<Dialogue>();

                for (int i = 0; i < 20; i++)
                    dialogues.Add(new Dialogue { Question = i.ToString(), Answer01 = "0" + (i * 1).ToString(), Answer02 = "0" + (i * 2).ToString(), Answer03 = "0" + (i * 3).ToString(), Answer04 = "0" + (i * 4).ToString() });

                DialoguesGrid.ItemsSource = dialogues;
             */
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
            FileInfo file = _dialogueAssembler.CreateTemplate();
            _fileOrder.ChangeFile(Path.GetFileNameWithoutExtension(file.Name), file.FullName, true);
        }

        private void SearchFileClick(object sender, MouseButtonEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.DefaultExt = ".csv";
            dialog.Filter = "CSV Files (*.csv)|*.csv";
            dialog.Multiselect = false;

            Nullable<bool> result = dialog.ShowDialog();

            if (result == true)
                _fileOrder.ChangeFile(Path.GetFileNameWithoutExtension(dialog.SafeFileName), dialog.FileName, true);
        }

        private void ClearFileClick(object sender, MouseButtonEventArgs e)
        {
            _fileOrder.ClearFile();
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
    }
}
