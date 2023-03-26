using System.ComponentModel;

namespace CSVEditor.Dialogues
{
    public class Dialogue : INotifyPropertyChanged
    {
        public bool Selected { get; set; } = false;
        public string Question { get; set; }
        public string Answer01 { get; set; }
        public string Answer02 { get; set; }
        public string Answer03 { get; set; }
        public string Answer04 { get; set; }

        public event PropertyChangedEventHandler PropertyChanged = (sender, e) => { };

        public void OnProperyChanged(string name) 
        {
            PropertyChanged(this, new PropertyChangedEventArgs(name));
        } 
    }
}
