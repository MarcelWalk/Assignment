using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Assignment.Model;
using Assignment.Poco;
using Assignment.ViewModel.Base;
using FileParser;
using Microsoft.Win32;

namespace Assignment.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        private bool _isClearingEnabled;
        private bool _isParsingAborted;
        private bool _isPreparingParsing;
        private AsciiFileParser _parser;
        private double _parsingProgress;
        private string _parsingProgressLabelText = Constants.ZERO_PERCENT;
        private string _selectedFilePath;
        private ObservableCollection<WordCountEntry> _wordCountCollection = new ObservableCollection<WordCountEntry>();

        public MainWindowViewModel()
        {
            StartButtonCommand = new RelayCommand(StartParsing, StartAllowed);
            SelectFileButtonCommand = new RelayCommand(SelectFile);
            CancelButtonCommand = new RelayCommand(CancelParsing, CancelAllowed);
        }

        public bool IsClearingEnabled
        {
            get => _isClearingEnabled;
            set => SetField(ref _isClearingEnabled, value);
        }

        public ObservableCollection<WordCountEntry> WordCountCollection
        {
            get => _wordCountCollection;
            set => SetField(ref _wordCountCollection, value);
        }

        public bool IsPreparingParsing
        {
            get => _isPreparingParsing;
            set => SetField(ref _isPreparingParsing, value);
        }

        public double ParsingProgress
        {
            get => _parsingProgress;
            set => SetField(ref _parsingProgress, value);
        }

        public string ParsingProgressLabelText
        {
            get => _parsingProgressLabelText;
            set => SetField(ref _parsingProgressLabelText, value);
        }

        public string SelectedFilePath
        {
            get => _selectedFilePath;
            private set => SetField(ref _selectedFilePath, value);
        }

        public RelayCommand StartButtonCommand { get; }
        public RelayCommand SelectFileButtonCommand { get; }
        public RelayCommand CancelButtonCommand { get; }

        public void SelectFile(object obj)
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = Constants.OPEN_FILE_DIALOG_FILTER;
            if (openFileDialog.ShowDialog() == true)
                SelectedFilePath = openFileDialog.FileName;
        }

        public async void StartParsing(object filePath)
        {
            WordCountCollection.Clear();
            _isParsingAborted = false;

            _parser = new AsciiFileParser();
            var progReporter = new ProgressReporter();

            progReporter.ProgressMade += ProgReporterOnProgressMade;

            var resFile = await Task.Run(() => _parser.ParseFile(filePath.ToString(), progReporter));

            if (_isParsingAborted && IsClearingEnabled) return;

            foreach (var (key, value) in resFile.OrderByDescending(kvp => kvp.Value))
                WordCountCollection.Add(KeyValueMapper.MapToWordCountEntry(key, value));
        }

        private void ProgReporterOnProgressMade(object? sender, ProgressEventArgs e)
        {
            if (e.Progress < 0)
            {
                IsPreparingParsing = true;
                ParsingProgressLabelText = Constants.ZERO_PERCENT;
            }
            else
            {
                IsPreparingParsing = false;
                ParsingProgress = e.Progress;
                ParsingProgressLabelText = $"{ParsingProgress}%";
            }
        }

        public void CancelParsing(object obj)
        {
            _isParsingAborted = true;
            _parser?.AbortProcessing();
        }

        public bool StartAllowed(object obj)
        {
            return !string.IsNullOrEmpty((string) obj);
        }

        public bool CancelAllowed(object obj)
        {
            return _parser is {IsParsing: true};
        }
    }
}