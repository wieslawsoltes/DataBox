using ReactiveUI;

namespace DataBoxDemo.ViewModels
{
    public class ItemViewModel : ViewModelBase
    {
        private string _column1;
        private string _column2;
        private string _column3;
        private bool _column4;
        private int _column5;

        public string Column1
        {
            get => _column1;
            set => this.RaiseAndSetIfChanged(ref _column1, value);
        }

        public string Column2
        {
            get => _column2;
            set => this.RaiseAndSetIfChanged(ref _column2, value);
        }

        public string Column3
        {
            get => _column3;
            set => this.RaiseAndSetIfChanged(ref _column3, value);
        }

        public bool Column4
        {
            get => _column4;
            set => this.RaiseAndSetIfChanged(ref _column4, value);
        }

        public int Column5
        {
            get => _column5;
            set => this.RaiseAndSetIfChanged(ref _column5, value);
        }

        public ItemViewModel(string column1, string column2, string column3, bool column4, int column5)
        {
            _column1 = column1;
            _column2 = column2;
            _column3 = column3;
            _column4 = column4;
            _column5 = column5;
        }

        public override string ToString()
        {
            return $"{_column5}";
        }
    }
}
