using System.Globalization;

namespace MauiCalculator;

public partial class ConverterPage : ContentPage
{
    private int _currentCategory = 0; // 0: Length, 1: Mass, 2: Temperature

    public ConverterPage()
    {
        InitializeComponent();
        SetCategory(0);
        StartCursorBlink();
    }

    private async void StartCursorBlink()
    {
        while (true)
        {
            lblConverterCursor.Opacity = lblConverterCursor.Opacity == 0 ? 1 : 0;
            await Task.Delay(500);
        }
    }

    private async void OnCalculatorTabTapped(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//MainPage");
    }

    private void OnCategorySelected(object sender, EventArgs e)
    {
        var btn = (Button)sender;
        if (btn == btnCatLength) SetCategory(0);
        else if (btn == btnCatMass) SetCategory(1);
        else if (btn == btnCatTemp) SetCategory(2);
    }

    private void SetCategory(int index)
    {
        _currentCategory = index;

        btnCatLength.BackgroundColor = index == 0 ? Color.FromArgb("#512BD4") : Color.FromArgb("#1C1C1E");
        btnCatLength.TextColor = index == 0 ? Colors.White : Color.FromArgb("#A1A1AA");

        btnCatMass.BackgroundColor = index == 1 ? Color.FromArgb("#512BD4") : Color.FromArgb("#1C1C1E");
        btnCatMass.TextColor = index == 1 ? Colors.White : Color.FromArgb("#A1A1AA");

        btnCatTemp.BackgroundColor = index == 2 ? Color.FromArgb("#512BD4") : Color.FromArgb("#1C1C1E");
        btnCatTemp.TextColor = index == 2 ? Colors.White : Color.FromArgb("#A1A1AA");

        pickerFrom.Items.Clear();
        pickerTo.Items.Clear();

        switch (index)
        {
            case 0:
                string[] lengths = ["Meter (m)", "Kilometer (km)", "Centimeter (cm)", "Millimeter (mm)", "Mile (mi)"];
                foreach (var item in lengths) { pickerFrom.Items.Add(item); pickerTo.Items.Add(item); }
                pickerFrom.SelectedIndex = 0;
                pickerTo.SelectedIndex = 1;
                break;
            case 1:
                string[] masses = ["Kilogram (kg)", "Gram (g)", "Milligram (mg)", "Pound (lb)"];
                foreach (var item in masses) { pickerFrom.Items.Add(item); pickerTo.Items.Add(item); }
                pickerFrom.SelectedIndex = 0;
                pickerTo.SelectedIndex = 1;
                break;
            case 2:
                string[] temps = ["Celsius (°C)", "Fahrenheit (°F)", "Kelvin (K)"];
                foreach (var item in temps) { pickerFrom.Items.Add(item); pickerTo.Items.Add(item); }
                pickerFrom.SelectedIndex = 0;
                pickerTo.SelectedIndex = 1;
                break;
        }

        lblFromVal.Text = "1";
        ComputeConversion();
    }

    private void OnUnitChanged(object sender, EventArgs e) => ComputeConversion();

    private void OnSwapUnitsClicked(object sender, EventArgs e)
    {
        int temp = pickerFrom.SelectedIndex;
        pickerFrom.SelectedIndex = pickerTo.SelectedIndex;
        pickerTo.SelectedIndex = temp;
        ComputeConversion();
    }

    private void OnKeypadDigit(object sender, EventArgs e)
    {
        string digit = ((Button)sender).Text;
        if (lblFromVal.Text == "0") lblFromVal.Text = digit;
        else lblFromVal.Text += digit;
        ComputeConversion();
    }

    private void OnKeypadDecimal(object sender, EventArgs e)
    {
        if (!lblFromVal.Text.Contains('.'))
        {
            lblFromVal.Text += ".";
            ComputeConversion();
        }
    }

    private void OnKeypadClear(object sender, EventArgs e)
    {
        lblFromVal.Text = "0";
        ComputeConversion();
    }

    private void OnKeypadBackspace(object sender, EventArgs e)
    {
        if (lblFromVal.Text.Length > 1)
            lblFromVal.Text = lblFromVal.Text[..^1];
        else
            lblFromVal.Text = "0";
        ComputeConversion();
    }

    private void OnKeypadNegate(object sender, EventArgs e)
    {
        if (double.TryParse(lblFromVal.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out double v) && v != 0)
        {
            lblFromVal.Text = (-v).ToString(CultureInfo.InvariantCulture);
            ComputeConversion();
        }
    }

    private void OnKeypadOk(object sender, EventArgs e) => ComputeConversion();

    private void ComputeConversion()
    {
        if (pickerFrom.SelectedIndex < 0 || pickerTo.SelectedIndex < 0) return;
        if (!double.TryParse(lblFromVal.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out double input))
        {
            lblToVal.Text = "0";
            return;
        }

        int from = pickerFrom.SelectedIndex;
        int to = pickerTo.SelectedIndex;
        if (from == to)
        {
            lblToVal.Text = input.ToString(CultureInfo.InvariantCulture);
            return;
        }

        double result = 0;

        if (_currentCategory == 0) // Length
        {
            double[] toBase = [1.0, 1000.0, 0.01, 0.001, 1609.344];
            double meters = input * toBase[from];
            result = meters / toBase[to];
        }
        else if (_currentCategory == 1) // Mass
        {
            double[] toBase = [1000.0, 1.0, 0.001, 453.59237];
            double grams = input * toBase[from];
            result = grams / toBase[to];
        }
        else if (_currentCategory == 2) // Temperature
        {
            double celsius = from switch
            {
                0 => input,
                1 => (input - 32.0) * 5.0 / 9.0,
                2 => input - 273.15,
                _ => input
            };

            result = to switch
            {
                0 => celsius,
                1 => (celsius * 9.0 / 5.0) + 32.0,
                2 => celsius + 273.15,
                _ => celsius
            };
        }

        lblToVal.Text = Math.Round(result, 4).ToString(CultureInfo.InvariantCulture);
    }
}