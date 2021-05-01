using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using AntonPaar.Model;
using AntonPaar.Poco;
using AntonPaar.ViewModel.Base;
using FileParser;
using Microsoft.Win32;

namespace AntonPaar.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        private AsciiFileParser _parser;
        private double _parsingProgress;
        private string _parsingProgressLabelText = "0%";
        private string _selectedFilePath;
        private ObservableCollection<WordCountEntry> _wordCountCollection = new ObservableCollection<WordCountEntry>();

        public MainWindowViewModel()
        {
            StartButtonCommand = new RelayCommand(StartParsing, StartAllowed);
            SelectFileButtonCommand = new RelayCommand(SelectFile);
            CancelButtonCommand = new RelayCommand(CancelParsing, CancelAllowed);
        }

        public ObservableCollection<WordCountEntry> WordCountCollection
        {
            get => _wordCountCollection;
            set => SetField(ref _wordCountCollection, value);
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

        private bool _isParsingFile = false;

        public void SelectFile(object obj)
        {
            var openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
                SelectedFilePath = openFileDialog.FileName;
        }

        public async void StartParsing(object filePath)
        {
            WordCountCollection.Clear();

            _parser = new AsciiFileParser();
            var progReporter = new ProgressReporter();

            progReporter.ProgressMade += ProgReporterOnProgressMade;

            _isParsingFile = true;

            var resFile = await Task.Run(() => _parser.ParseFile(filePath.ToString(), progReporter));
            
            _isParsingFile = false;

            foreach (var (key, value) in resFile)
                WordCountCollection.Add(KeyValueMapper.MapToWordCountEntry(key, value));
        }

        private void ProgReporterOnProgressMade(object? sender, ProgressEventArgs e)
        {
            ParsingProgress = e.Progress;
            ParsingProgressLabelText = $"{ParsingProgress}%";
        }

        private void CancelParsing(object obj)
        {
            _parser?.AbortProcessing();
        }

        public bool StartAllowed(object obj)
        {
            return !string.IsNullOrEmpty((string)obj);
        }

        public bool CancelAllowed(object obj)
        {
            return _parser != null && _isParsingFile;
        }
    }
}