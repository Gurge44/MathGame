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

namespace MathGame;

public partial class MainWindow : Window
{
    int TargetNumber;
    int CurrentNumber;
    Operator CurrentOperator;
    int[] Choices;
    
    public MainWindow()
    {
        InitializeComponent();
        ResetToDefault();
    }

    private async void ResetToDefault()
    {
        Random r = new();
        TargetNumber = r.Next(1, 1000);
        Target.Content = TargetNumber;
        CurrentNumber = 0;
        Current.Content = CurrentNumber;
        CurrentOperator = default;
        
        NewButton.IsEnabled = false;
        
        Choices = new int[6];
        bool possible = false;
        while (!possible)
        {
            await Task.Delay(1);
            for (int i = 0; i < 6; i++)
            {
                Choices[i] = r.Next(1, 100);
            }
            possible = CheckIfPossible();
        }

        var buttons = FirstNumberRow.Children.OfType<Button>().Concat(SecondNumberRow.Children.OfType<Button>()).ToList();
        for (int i = 0; i < 6; i++)
        {
            buttons[i].Content = Choices[i];
        }
        
        NewButton.IsEnabled = true;
    }
    
    private bool CheckIfPossible()
    {
        var permutations = GetPermutations(Choices, 6);
        return permutations.Any(permutation => CheckIfPossible(permutation.ToArray()));
    }

    private static IEnumerable<IEnumerable<int>> GetPermutations(int[] list, int length)
    {
        if (length == 1) return list.Select(t => new int[] { t });
        return GetPermutations(list, length - 1)
            .SelectMany(t => list.Where(e => !t.Contains(e)),
                (t1, t2) => t1.Concat(new int[] { t2 }));
    }
    
    private bool CheckIfPossible(params int[] numbers)
    {
        if (numbers.Length == 2)
        {
            return numbers[0] + numbers[1] == TargetNumber || numbers[0] - numbers[1] == TargetNumber || numbers[1] - numbers[0] == TargetNumber || numbers[0] * numbers[1] == TargetNumber || numbers[0] / numbers[1] == TargetNumber || numbers[1] / numbers[0] == TargetNumber;
        }

        return numbers.Select((_, i) => numbers.Where((_, index) => index != i).ToArray()).Any(CheckIfPossible);
    }

    private void OperatorClick(object sender, RoutedEventArgs e)
    {
        CurrentOperator = ((Button)sender).Content.ToString() switch
        {
            "+" => Operator.Add,
            "-" => Operator.Subtract,
            "*" => Operator.Multiply,
            "/" => Operator.Divide,
            _ => throw new InvalidOperationException("Invalid operator")
        };
    }

    enum Operator
    {
        Add,
        Subtract,
        Multiply,
        Divide
    }
}