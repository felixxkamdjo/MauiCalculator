using System.Globalization;

namespace MauiCalculator;

public partial class MainPage : ContentPage
{
    private double _firstOperand;
    private string _currentOperator = string.Empty;
    private bool _isNewEntry = true;
    private bool _hasError = false;

    public MainPage()
    {
        InitializeComponent();
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
        string selectedOp = button.Text;

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
            case "+":
                result = _firstOperand + secondOperand;
                break;
            case "-":
                result = _firstOperand - secondOperand;
                break;
            case "×":
                result = _firstOperand * secondOperand;
                break;
            case "÷":
                if (secondOperand == 0)
                {
                    lblDisplay.Text = "Division par 0 impossible";
                    lblEquation.Text = string.Empty;
                    _hasError = true;
                    return;
                }
                result = _firstOperand / secondOperand;
                break;
            default:
                return;
        }

        lblDisplay.Text = result.ToString(CultureInfo.InvariantCulture);
        _firstOperand = result;
        _isNewEntry = true;
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
            val /= 100.0;
            lblDisplay.Text = val.ToString(CultureInfo.InvariantCulture);
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

    private void ResetCalculator()
    {
        lblDisplay.Text = "0";
        lblEquation.Text = string.Empty;
        _firstOperand = 0;
        _currentOperator = string.Empty;
        _isNewEntry = true;
        _hasError = false;
    }
}