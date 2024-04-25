using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MathGame
{
    public partial class MainWindow : Window
    {
        int TargetNumber;
        int CurrentNumber;
        Operator CurrentOperator;
        int[] Choices = null!;

        public MainWindow()
        {
            InitializeComponent();
            ResetToDefault();
        }

        private async void ResetToDefault(bool setTarget = true)
        {
            Random r = new();
            CurrentNumber = 0;
            Current.Content = CurrentNumber;
            CurrentOperator = default;

            MainGrid.IsEnabled = false;
            Loading loadingScreen = new();
            loadingScreen.Show();
            await Task.Delay(1);

            int j = 0;
            Choices = new int[6];
            bool possible = false;
            while (!possible)
            {
                j++;
                if (j > 1000 && !setTarget)
                {
                    MessageBox.Show("No possible solution found. Generating new numbers...", "No Solution Found", MessageBoxButton.OK, MessageBoxImage.Error);
                    j = 0;
                    setTarget = true;
                }
                if (setTarget) TargetNumber = r.Next(1, 1000);
                for (int i = 0; i < 6; i++)
                {
                    Choices[i] = r.Next(1, 100);
                }

                if (Choices.Any(n => n == 0 || n == TargetNumber)) continue;
                
                var checkTask = Task.Run(CheckIfPossible);
                while (!checkTask.IsCompleted) await Task.Delay(1);
                possible = checkTask.Result;
            }

            var buttons = FirstNumberRow.Children.OfType<Button>().Concat(SecondNumberRow.Children.OfType<Button>()).ToList();
            for (int i = 0; i < 6; i++)
            {
                buttons[i].Content = Choices[i];
            }

            loadingScreen.Close();
            MainGrid.IsEnabled = true;

            Target.Content = TargetNumber;
            Target.Foreground = Brushes.Black;
            
            Current.Visibility = Visibility.Visible;
            FirstNumberRow.Children.OfType<Button>().Concat(SecondNumberRow.Children.OfType<Button>()).ToList().ForEach(b => b.IsEnabled = true);
        }

        private async Task<bool> CheckIfPossible()
        {
            await Task.Delay(1);
            var permutations = GetPermutations(Choices, 6);
            return permutations.Any(permutation => CheckIfPossible(permutation.ToArray()));
        }

        private static IEnumerable<IEnumerable<int>> GetPermutations(int[] list, int length)
        {
            if (length == 1) return list.Select(t => new int[] { t });
            return GetPermutations(list, length - 1)
                .SelectMany(t => list.Where(e => !t.Contains(e)), (t1, t2) => t1.Append(t2));
        }

        private bool CheckIfPossible(params int[] numbers)
        {
            if (numbers.Any(n => n == 0)) return false;
            return numbers.Length switch
            {
                2 => numbers[0] + numbers[1] == TargetNumber || numbers[0] - numbers[1] == TargetNumber || numbers[1] - numbers[0] == TargetNumber || numbers[0] * numbers[1] == TargetNumber || numbers[0] / numbers[1] == TargetNumber || numbers[1] / numbers[0] == TargetNumber,
                //3 => CheckIfPossible(numbers[0] + numbers[1], numbers[2]) || CheckIfPossible(numbers[0] - numbers[1], numbers[2]) || CheckIfPossible(numbers[1] - numbers[0], numbers[2]) || CheckIfPossible(numbers[0] * numbers[1], numbers[2]) || CheckIfPossible(numbers[0] / numbers[1], numbers[2]) || CheckIfPossible(numbers[1] / numbers[0], numbers[2]) || CheckIfPossible(numbers[0] + numbers[2], numbers[1]) || CheckIfPossible(numbers[0] - numbers[2], numbers[1]) || CheckIfPossible(numbers[2] - numbers[0], numbers[1]) || CheckIfPossible(numbers[0] * numbers[2], numbers[1]) || CheckIfPossible(numbers[0] / numbers[2], numbers[1]) || CheckIfPossible(numbers[2] / numbers[0], numbers[1]) || CheckIfPossible(numbers[1] + numbers[2], numbers[0]) || CheckIfPossible(numbers[1] - numbers[2], numbers[0]) || CheckIfPossible(numbers[2] - numbers[1], numbers[0]) || CheckIfPossible(numbers[1] * numbers[2], numbers[0]) || CheckIfPossible(numbers[1] / numbers[2], numbers[0]) || CheckIfPossible(numbers[2] / numbers[1], numbers[0]),
                //4 => CheckIfPossible(numbers[0] + numbers[1], numbers[2], numbers[3]) || CheckIfPossible(numbers[0] - numbers[1], numbers[2], numbers[3]) || CheckIfPossible(numbers[1] - numbers[0], numbers[2], numbers[3]) || CheckIfPossible(numbers[0] * numbers[1], numbers[2], numbers[3]) || CheckIfPossible(numbers[0] / numbers[1], numbers[2], numbers[3]) || CheckIfPossible(numbers[1] / numbers[0], numbers[2], numbers[3]) || CheckIfPossible(numbers[0] + numbers[2], numbers[1], numbers[3]) || CheckIfPossible(numbers[0] - numbers[2], numbers[1], numbers[3]) || CheckIfPossible(numbers[2] - numbers[0], numbers[1], numbers[3]) || CheckIfPossible(numbers[0] * numbers[2], numbers[1], numbers[3]) || CheckIfPossible(numbers[0] / numbers[2], numbers[1], numbers[3]) || CheckIfPossible(numbers[2] / numbers[0], numbers[1], numbers[3]) || CheckIfPossible(numbers[0] + numbers[3], numbers[1], numbers[2]) || CheckIfPossible(numbers[0] - numbers[3], numbers[1], numbers[2]) || CheckIfPossible(numbers[3] - numbers[0], numbers[1], numbers[2]) || CheckIfPossible(numbers[0] * numbers[3], numbers[1], numbers[2]) || CheckIfPossible(numbers[0] / numbers[3], numbers[1], numbers[2]) || CheckIfPossible(numbers[3] / numbers[0], numbers[1], numbers[2]) || CheckIfPossible(numbers[1] + numbers[2], numbers[0], numbers[3]) || CheckIfPossible(numbers[1] - numbers[2], numbers[0], numbers[3]) || CheckIfPossible(numbers[2] - numbers[1], numbers[0], numbers[3]) || CheckIfPossible(numbers[1] * numbers[2], numbers[0], numbers[3]) || CheckIfPossible(numbers[1] / numbers[2], numbers[0], numbers[3]) || CheckIfPossible(numbers[2] / numbers[1], numbers[0], numbers[3]) || CheckIfPossible(numbers[1] + numbers[3], numbers[0], numbers[2]) || CheckIfPossible(numbers[1] - numbers[3], numbers[0], numbers[2]) || CheckIfPossible(numbers[3] - numbers[1], numbers[0], numbers[2]) || CheckIfPossible(numbers[1] * numbers[3], numbers[0], numbers[2]) || CheckIfPossible(numbers[1] / numbers[3], numbers[0], numbers[2]) || CheckIfPossible(numbers[3] / numbers[1], numbers[0], numbers[2]) || CheckIfPossible(numbers[2] + numbers[3], numbers[0], numbers[1]) || CheckIfPossible(numbers[2] - numbers[3], numbers[0], numbers[1]) || CheckIfPossible(numbers[3] - numbers[2], numbers[0], numbers[1]) || CheckIfPossible(numbers[2] * numbers[3], numbers[0], numbers[1]) || CheckIfPossible(numbers[2] / numbers[3], numbers[0], numbers[1]) || CheckIfPossible(numbers[3] / numbers[2], numbers[0], numbers[1]),
                _ => numbers.Select((_, i) => numbers.Where((_, index) => index != i).ToArray()).Any(CheckIfPossible)
            };
        }

        private void OperatorClick(object sender, RoutedEventArgs e)
        {
            CurrentOperator = ((Button)sender).Content.ToString() switch
            {
                "+" => Operator.Add,
                "-" => Operator.Subtract,
                "*" => Operator.Multiply,
                "/" => Operator.Divide,
                _ => Operator.None
            };
        }

        private void NumberClick(object sender, RoutedEventArgs e)
        {
            int num = int.Parse(((Button)sender).Content.ToString() ?? string.Empty);

            if (CurrentNumber == 0) CurrentNumber = num;
            else
            {
                CurrentNumber = CurrentOperator switch
                {
                    Operator.Add => CurrentNumber + num,
                    Operator.Subtract => CurrentNumber - num,
                    Operator.Multiply => CurrentNumber * num,
                    Operator.Divide => CurrentNumber / num,
                    _ => num
                };
            }

            if (CurrentNumber == TargetNumber)
            {
                Target.Foreground = Brushes.Green;
                Current.Visibility = Visibility.Hidden;
                FirstNumberRow.Children.OfType<Button>().Concat(SecondNumberRow.Children.OfType<Button>()).ToList().ForEach(b => b.IsEnabled = false);
            }

            Current.Content = CurrentNumber;
        }

        private enum Operator
        {
            None,
            Add,
            Subtract,
            Multiply,
            Divide
        }

        private void NewGame(object sender, RoutedEventArgs e) => ResetToDefault();

        private void OpenSettings(object sender, RoutedEventArgs e)
        {

        }

        private void EditTargetNumber(object sender, RoutedEventArgs e)
        {
            Target.Visibility = Visibility.Collapsed;

            TextBox tb = new()
            {
                Text = TargetNumber.ToString(),
                Style = Target.Style
            };
            tb.TextChanged += (o, args) =>
            {
                if (int.TryParse(tb.Text, out int result))
                {
                    TargetNumber = result;
                }
            };

            MainGrid.Children.Add(tb);
            
            tb.Focus();
            
            tb.LostFocus += (o, args) =>
            {
                Target.Visibility = Visibility.Visible;
                MainGrid.Children.Remove(tb);
                ResetToDefault(setTarget: false);
            };
        }
    }
}