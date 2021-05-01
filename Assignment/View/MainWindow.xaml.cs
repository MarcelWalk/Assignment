using System.Windows;
using AntonPaar.ViewModel;

namespace AntonPaar
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel();
        }

        //private void Button_Click(object sender, RoutedEventArgs e)
        //{
        //}

        //private async void Button_Click_1(object sender, RoutedEventArgs e)
        //{
        //    var parser = new AsciiFileParser();
        //    var reporter = new ProgressReporter();

        //    reporter.ProgressMade += (o, args) =>
        //    {
        //        Dispatcher.BeginInvoke((Action) (() => { ProgressBar.Value = args.Progress; }));
        //    };

        //    var resFile = await Task.Run(() =>
        //        parser.ParseFile(new FileInfo("C:/Users/walkm/Desktop/SampleLong.txt"), reporter));
        //}
    }
}