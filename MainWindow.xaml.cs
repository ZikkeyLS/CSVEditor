using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CSVEditor
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            CloseApplicationButton.Click += CloseApplicationButtonClick;
            MinimizeApplicationButton.Click += MinimizeApplicationButtonClick;

            var brushConverter = new BrushConverter();
            ObservableCollection<Dialogue> dialogues = new ObservableCollection<Dialogue>();

            for (int i = 0; i < 20; i++)
                dialogues.Add(new Dialogue { Question = i.ToString(), Answer01 = "0" + (i * 1).ToString(), Answer02 = "0" + (i * 2).ToString(), Answer03 = "0" + (i * 3).ToString(), Answer04 = "0" + (i * 4).ToString() });

            DialoguesGrid.ItemsSource = dialogues;
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

        private void MinimizeApplicationButtonClick(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void CloseApplicationButtonClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
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

    public class Dialogue
    {
        public string Question { get; set; }
        public string Answer01 { get; set; }
        public string Answer02 { get; set; }
        public string Answer03 { get; set; }
        public string Answer04 { get; set; }
    }
}
