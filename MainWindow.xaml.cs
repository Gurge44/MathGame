using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace MathGame
{
    public partial class MainWindow
    {
        private bool AllowEditorExit = true;
        private int[] Choices = null!;
        private int CurrentNumber;
        private Operator CurrentOperator;
        public int Difficulty = 4;
        public bool HardMode = false;
        private int NumbersUsed = 0;
        private int TargetNumber;

        public MainWindow()
        {
            Instance = this;
            InitializeComponent();
            ResetToDefault();
        }

        public static MainWindow Instance { get; private set; } = null!;

        private async void ResetToDefault(bool setTarget = true)
        {
            Random r = new();
            CurrentNumber = 0;
            Current.Content = CurrentNumber;
            CurrentOperator = default;
            NumbersUsed = 0;
            NumbersUsedLabel.Visibility = HardMode ? Visibility.Visible : Visibility.Hidden;

            MainGrid.Children.OfType<TextBox>().ToList().ForEach(ExitEditor);
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
                if (j > 500 && !setTarget)
                {
                    loadingScreen.Hide();
                    MessageBox.Show("No possible solution found. Generating new numbers...", "No Solution Found", MessageBoxButton.OK, MessageBoxImage.Error);
                    loadingScreen.Show();
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
            UpdateUsageLabel();
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
                3 when Difficulty >= 3 => CheckIfPossible(numbers[0] + numbers[1], numbers[2]) || CheckIfPossible(numbers[0] - numbers[1], numbers[2]) || CheckIfPossible(numbers[1] - numbers[0], numbers[2]) || CheckIfPossible(numbers[0] * numbers[1], numbers[2]) || CheckIfPossible(numbers[0] / numbers[1], numbers[2]) || CheckIfPossible(numbers[1] / numbers[0], numbers[2]) || CheckIfPossible(numbers[0] + numbers[2], numbers[1]) || CheckIfPossible(numbers[0] - numbers[2], numbers[1]) || CheckIfPossible(numbers[2] - numbers[0], numbers[1]) || CheckIfPossible(numbers[0] * numbers[2], numbers[1]) || CheckIfPossible(numbers[0] / numbers[2], numbers[1]) || CheckIfPossible(numbers[2] / numbers[0], numbers[1]) || CheckIfPossible(numbers[1] + numbers[2], numbers[0]) || CheckIfPossible(numbers[1] - numbers[2], numbers[0]) || CheckIfPossible(numbers[2] - numbers[1], numbers[0]) || CheckIfPossible(numbers[1] * numbers[2], numbers[0]) || CheckIfPossible(numbers[1] / numbers[2], numbers[0]) || CheckIfPossible(numbers[2] / numbers[1], numbers[0]),
                4 when Difficulty >= 4 => CheckIfPossible(numbers[0] + numbers[1], numbers[2], numbers[3]) || CheckIfPossible(numbers[0] - numbers[1], numbers[2], numbers[3]) || CheckIfPossible(numbers[1] - numbers[0], numbers[2], numbers[3]) || CheckIfPossible(numbers[0] * numbers[1], numbers[2], numbers[3]) || CheckIfPossible(numbers[0] / numbers[1], numbers[2], numbers[3]) || CheckIfPossible(numbers[1] / numbers[0], numbers[2], numbers[3]) || CheckIfPossible(numbers[0] + numbers[2], numbers[1], numbers[3]) || CheckIfPossible(numbers[0] - numbers[2], numbers[1], numbers[3]) || CheckIfPossible(numbers[2] - numbers[0], numbers[1], numbers[3]) || CheckIfPossible(numbers[0] * numbers[2], numbers[1], numbers[3]) || CheckIfPossible(numbers[0] / numbers[2], numbers[1], numbers[3]) || CheckIfPossible(numbers[2] / numbers[0], numbers[1], numbers[3]) || CheckIfPossible(numbers[0] + numbers[3], numbers[1], numbers[2]) || CheckIfPossible(numbers[0] - numbers[3], numbers[1], numbers[2]) || CheckIfPossible(numbers[3] - numbers[0], numbers[1], numbers[2]) || CheckIfPossible(numbers[0] * numbers[3], numbers[1], numbers[2]) || CheckIfPossible(numbers[0] / numbers[3], numbers[1], numbers[2]) || CheckIfPossible(numbers[3] / numbers[0], numbers[1], numbers[2]) || CheckIfPossible(numbers[1] + numbers[2], numbers[0], numbers[3]) || CheckIfPossible(numbers[1] - numbers[2], numbers[0], numbers[3]) || CheckIfPossible(numbers[2] - numbers[1], numbers[0], numbers[3]) || CheckIfPossible(numbers[1] * numbers[2], numbers[0], numbers[3]) || CheckIfPossible(numbers[1] / numbers[2], numbers[0], numbers[3]) || CheckIfPossible(numbers[2] / numbers[1], numbers[0], numbers[3]) || CheckIfPossible(numbers[1] + numbers[3], numbers[0], numbers[2]) || CheckIfPossible(numbers[1] - numbers[3], numbers[0], numbers[2]) || CheckIfPossible(numbers[3] - numbers[1], numbers[0], numbers[2]) || CheckIfPossible(numbers[1] * numbers[3], numbers[0], numbers[2]) || CheckIfPossible(numbers[1] / numbers[3], numbers[0], numbers[2]) || CheckIfPossible(numbers[3] / numbers[1], numbers[0], numbers[2]) || CheckIfPossible(numbers[2] + numbers[3], numbers[0], numbers[1]) || CheckIfPossible(numbers[2] - numbers[3], numbers[0], numbers[1]) || CheckIfPossible(numbers[3] - numbers[2], numbers[0], numbers[1]) || CheckIfPossible(numbers[2] * numbers[3], numbers[0], numbers[1]) || CheckIfPossible(numbers[2] / numbers[3], numbers[0], numbers[1]) || CheckIfPossible(numbers[3] / numbers[2], numbers[0], numbers[1]),
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

            NumbersUsed++;
            UpdateUsageLabel();

            bool success = CurrentNumber == TargetNumber;
            if (success)
            {
                Target.Foreground = Brushes.Green;
                Current.Visibility = Visibility.Hidden;
                DisableNumberButtons();
            }

            Current.Content = CurrentNumber;

            if (success) return;

            if (HardMode && NumbersUsed >= Difficulty)
            {
                Current.Foreground = Brushes.Red;
                var beforeBg = ResetButton.Background;
                ResetButton.Background = Brushes.Yellow;
                LateTask(() => ResetButton.Background = beforeBg, 2000);
                DisableNumberButtons();
            }

            return;

            void DisableNumberButtons()
            {
                FirstNumberRow.Children.OfType<Button>().Concat(SecondNumberRow.Children.OfType<Button>()).ToList().ForEach(b => b.IsEnabled = false);
            }
        }

        public void UpdateUsageLabel()
        {
            if (!HardMode) return;
            NumbersUsedLabel.Content = $"{NumbersUsed}/{Difficulty}";
        }

        private void NewGame(object sender, RoutedEventArgs e) => ResetToDefault();

        private void OpenSettings(object sender, RoutedEventArgs e)
        {
            new Settings().Show();
        }

        private static async void LateTask(Action action, int delay)
        {
            await Task.Delay(delay);
            action();
        }

        private void EditTargetNumber(object sender, RoutedEventArgs e)
        {
            Target.Visibility = Visibility.Collapsed;

            TextBox tb = new()
            {
                Text = TargetNumber.ToString(),
                Style = Target.Style,
                FontSize = Target.FontSize,
                HorizontalAlignment = Target.HorizontalAlignment,
                VerticalAlignment = Target.VerticalAlignment,
            };
            tb.TextChanged += (_, _) =>
            {
                AllowEditorExit = false;
                LateTask(() => AllowEditorExit = true, 100);
                if (int.TryParse(tb.Text, out int result))
                {
                    TargetNumber = result;
                }
            };

            MainGrid.Children.Add(tb);

            tb.Focus();

            tb.MouseLeave += (_, _) =>
            {
                if (AllowEditorExit) ExitEditor(tb);
            };
        }

        private void ExitEditor(UIElement tb)
        {
            Target.Content = TargetNumber;
            Target.Visibility = Visibility.Visible;
            MainGrid.Children.Remove(tb);
            ResetToDefault(setTarget: false);
        }

        private void ResetGame(object sender, RoutedEventArgs e)
        {
            bool done = CurrentNumber == TargetNumber;
            CurrentNumber = 0;
            Current.Content = CurrentNumber;
            NumbersUsed = 0;
            UpdateUsageLabel();
            if (done) return;

            FirstNumberRow.Children.OfType<Button>().Concat(SecondNumberRow.Children.OfType<Button>()).ToList().ForEach(b => b.IsEnabled = true);
            Current.Foreground = Brushes.Black;
        }

        private void Help(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Welcome to Math Game! The goal of the game is to reach the target number using the numbers provided. You can add, subtract, multiply, and divide the numbers to reach the target number. You can also change the target number by clicking on the edit icon in the top left. You can set how many steps are required to reach the target number in the settings which you can open by the wheel icon in the top left. To reset your current number to 0, click the reset icon in the top right. To get a new target number and restart, click the NEW button in the bottom right. Good luck!", "Help", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private enum Operator
        {
            None,
            Add,
            Subtract,
            Multiply,
            Divide
        }
    }
}