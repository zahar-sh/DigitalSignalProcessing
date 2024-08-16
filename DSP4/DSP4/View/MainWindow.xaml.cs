using DSP4.Model;
using DSP4.ViewModel;
using System.Windows;

namespace DSP4
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void SignalTypeChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is MainWindowViewModel viewModel)
            {
                if (parabolicRadioButton.IsChecked.GetValueOrDefault())
                {
                    viewModel.SmoothingType = SmoothingType.Parabolic;
                } 
                else if (slidingRadioButton.IsChecked.GetValueOrDefault())
                {
                    viewModel.SmoothingType = SmoothingType.Sliding;
                }
                else if (medianRadioButton.IsChecked.GetValueOrDefault())
                {
                    viewModel.SmoothingType = SmoothingType.Median;
                }
            }
        }
    }
}
