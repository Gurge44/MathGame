using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace MathGame
{
    public partial class Settings
    {
        public Settings()
        {
            InitializeComponent();
            DifficultyComboBox.SelectedIndex = MainWindow.Instance.Difficulty - 2;
            HardModeCheckBox.IsChecked = MainWindow.Instance.HardMode;
        }

        private void Save(object sender, RoutedEventArgs e)
        {
            MainWindow.Instance.Difficulty = DifficultyComboBox.SelectedIndex + 2;
            MainWindow.Instance.HardMode = HardModeCheckBox.IsChecked ?? false;
            Close();
        }

        private void Cancel(object sender, RoutedEventArgs e) => Close();

        private void ButtonMouseEnter(object sender, MouseEventArgs e)
        {
            var button = (Button)sender;
            button.Background = Brushes.White;
            button.Foreground = Brushes.Black;
        }

        private void SaveButtonMouseLeave(object sender, MouseEventArgs e)
        {
            var button = (Button)sender;
            button.Background = Brushes.DodgerBlue;
            button.Foreground = Brushes.White;
        }

        private void CancelButtonMouseLeave(object sender, MouseEventArgs e)
        {
            var button = (Button)sender;
            button.Background = Brushes.Black;
            button.Foreground = Brushes.White;
        }
    }
}