using System.Globalization;

namespace MauiCalculator;

public partial class MainPage : ContentPage
{
    private double _firstOperand;
    private string _currentOperator = string.Empty;
    private bool _isNewEntry = true;
    private bool _hasError = false;

    // Multiplication symbol normalization
    private const string MultiplySign = "×";
    private const string DivideSign = "÷";
    private const string AddSign = "+";
    private const string SubtractSign = "-";

    public MainPage()
    {
        InitializeComponent();
        StartCursorBlink();
    }

    private async void StartCursorBlink()
    {
        while (true)
        {
            lblCursor.Opacity = lblCursor.Opacity == 0 ? 1 : 0;
            await Task.Delay(500);
        }
    }

    private void OnDigitClicked(object sender, EventArgs e)
    {
        if (_hasError) ResetCalculator();

        var button = (Button)sender;
        string digit = button.Text;

        if (_isNewEntry || lblDisplay.Text == "0")
        {
            lblDisplay.Text = digit;
            _isNewEntry = false;
        }
        else
        {
            lblDisplay.Text += digit;
        }
    }

    private void OnDecimalClicked(object sender, EventArgs e)
    {
        if (_hasError) ResetCalculator();

        if (_isNewEntry)
        {
            lblDisplay.Text = "0.";
            _isNewEntry = false;
        }
        else if (!lblDisplay.Text.Contains('.'))
        {
            lblDisplay.Text += ".";
        }
    }

    private void OnOperatorClicked(object sender, EventArgs e)
    {
        if (_hasError) return;

        var button = (Button)sender;
        string selectedOp = button.Text.Trim();

        if (!string.IsNullOrEmpty(_currentOperator) && !_isNewEntry)
        {
            CalculateResult();
        }

        if (double.TryParse(lblDisplay.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out double val))
        {
            _firstOperand = val;
            _currentOperator = selectedOp;
            lblEquation.Text = $"{_firstOperand} {_currentOperator}";
            _isNewEntry = true;
        }
    }

    private void OnEqualsClicked(object sender, EventArgs e)
    {
        if (_hasError || string.IsNullOrEmpty(_currentOperator)) return;

        CalculateResult();
        _currentOperator = string.Empty;
        lblEquation.Text = string.Empty;
    }

    private void CalculateResult()
    {
        if (!double.TryParse(lblDisplay.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out double secondOperand))
            return;

        double result = 0;

        switch (_currentOperator)
        {
            case AddSign:
                result = _firstOperand + secondOperand;
                break;
            case SubtractSign:
                result = _firstOperand - secondOperand;
                break;
            case MultiplySign:
            case "*":
            case "x":
                result = _firstOperand * secondOperand;
                break;
            case DivideSign:
            case "/":
                if (secondOperand == 0)
                {
                    lblDisplay.Text = "Cannot divide by 0";
                    lblEquation.Text = string.Empty;
                    _hasError = true;
                    return;
                }
                result = _firstOperand / secondOperand;
                break;
            default:
                return;
        }

        string equationText = $"{_firstOperand} {_currentOperator} {secondOperand} = {result.ToString(CultureInfo.InvariantCulture)}";
        AddHistoryItem(equationText);

        lblDisplay.Text = result.ToString(CultureInfo.InvariantCulture);
        _firstOperand = result;
        _isNewEntry = true;
    }

    private void AddHistoryItem(string entry)
    {
        var historyLabel = new Label
        {
            Text = entry,
            FontSize = 18,
            TextColor = Color.FromArgb("#71717A"),
            HorizontalTextAlignment = TextAlignment.End
        };

        var tap = new TapGestureRecognizer();
        tap.Tapped += (s, e) =>
        {
            string[] parts = entry.Split('=');
            if (parts.Length == 2)
            {
                lblDisplay.Text = parts[1].Trim();
                _isNewEntry = true;
            }
        };
        historyLabel.GestureRecognizers.Add(tap);

        historyContainer.Children.Add(historyLabel);
        _ = scrollHistory.ScrollToAsync(historyContainer, ScrollToPosition.End, animated: true);
    }

    private void OnClearClicked(object sender, EventArgs e)
    {
        ResetCalculator();
    }

    private void OnDeleteClicked(object sender, EventArgs e)
    {
        if (_hasError || _isNewEntry) return;

        if (lblDisplay.Text.Length > 1)
        {
            lblDisplay.Text = lblDisplay.Text[..^1];
        }
        else
        {
            lblDisplay.Text = "0";
            _isNewEntry = true;
        }
    }

    private void OnPercentageClicked(object sender, EventArgs e)
    {
        if (_hasError) return;

        if (double.TryParse(lblDisplay.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out double val))
        {
            double res = val / 100.0;
            AddHistoryItem($"{val}% = {res.ToString(CultureInfo.InvariantCulture)}");
            lblDisplay.Text = res.ToString(CultureInfo.InvariantCulture);
            _isNewEntry = true;
        }
    }

    private void OnNegateClicked(object sender, EventArgs e)
    {
        if (_hasError || lblDisplay.Text == "0") return;

        if (double.TryParse(lblDisplay.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out double val))
        {
            val = -val;
            lblDisplay.Text = val.ToString(CultureInfo.InvariantCulture);
        }
    }

    private void OnSqrtClicked(object sender, EventArgs e)
    {
        if (_hasError) return;
        if (double.TryParse(lblDisplay.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out double val))
        {
            if (val < 0)
            {
                lblDisplay.Text = "Invalid input";
                _hasError = true;
                return;
            }
            double res = Math.Sqrt(val);
            AddHistoryItem($"√({val}) = {res.ToString(CultureInfo.InvariantCulture)}");
            lblDisplay.Text = res.ToString(CultureInfo.InvariantCulture);
            _isNewEntry = true;
        }
    }

    private void OnSquareClicked(object sender, EventArgs e)
    {
        if (_hasError) return;
        if (double.TryParse(lblDisplay.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out double val))
        {
            double res = Math.Pow(val, 2);
            AddHistoryItem($"sqr({val}) = {res.ToString(CultureInfo.InvariantCulture)}");
            lblDisplay.Text = res.ToString(CultureInfo.InvariantCulture);
            _isNewEntry = true;
        }
    }

    private void OnReciprocalClicked(object sender, EventArgs e)
    {
        if (_hasError) return;
        if (double.TryParse(lblDisplay.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out double val))
        {
            if (val == 0)
            {
                lblDisplay.Text = "Cannot divide by 0";
                _hasError = true;
                return;
            }
            double res = 1.0 / val;
            AddHistoryItem($"1/({val}) = {res.ToString(CultureInfo.InvariantCulture)}");
            lblDisplay.Text = res.ToString(CultureInfo.InvariantCulture);
            _isNewEntry = true;
        }
    }

    private void OnPiClicked(object sender, EventArgs e)
    {
        if (_hasError) ResetCalculator();
        lblDisplay.Text = Math.PI.ToString(CultureInfo.InvariantCulture);
        _isNewEntry = true;
    }

    private void ResetCalculator()
    {
        lblDisplay.Text = "0";
        lblEquation.Text = string.Empty;
        _firstOperand = 0;
        _currentOperator = string.Empty;
        _isNewEntry = true;
        _hasError = false;
    }

    private void OnOpenModalClicked(object sender, EventArgs e)
    {
        modalOverlay.IsVisible = true;
    }

    private void OnCloseModalClicked(object sender, EventArgs e)
    {
        modalOverlay.IsVisible = false;
    }

    private void OnConfirmClearHistoryClicked(object sender, EventArgs e)
    {
        historyContainer.Children.Clear();
        modalOverlay.IsVisible = false;
    }

    private async void OnConverterTabTapped(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//ConverterPage");
    }
}