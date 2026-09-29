using System.Globalization;

namespace MauiCalculator;

public partial class ConverterPage : ContentPage
{
    public ConverterPage()
    {
        InitializeComponent();
        pickerCategory.SelectedIndex = 0;
    }

    private async void OnCalculatorTabTapped(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//MainPage");
    }

    private void OnCategoryChanged(object sender, EventArgs e)
    {
        pickerFromUnit.Items.Clear();
        pickerToUnit.Items.Clear();

        switch (pickerCategory.SelectedIndex)
        {
            case 0: // Longueur
                string[] lengthUnits = ["Mètre (m)", "Kilomètre (km)", "Centimètre (cm)", "Mile (mi)"];
                foreach (var u in lengthUnits) { pickerFromUnit.Items.Add(u); pickerToUnit.Items.Add(u); }
                pickerFromUnit.SelectedIndex = 0;
                pickerToUnit.SelectedIndex = 1;
                break;

            case 1: // Masse
                string[] massUnits = ["Kilogramme (kg)", "Gramme (g)", "Livre (lb)"];
                foreach (var u in massUnits) { pickerFromUnit.Items.Add(u); pickerToUnit.Items.Add(u); }
                pickerFromUnit.SelectedIndex = 0;
                pickerToUnit.SelectedIndex = 1;
                break;

            case 2: // Température
                string[] tempUnits = ["Celsius (°C)", "Fahrenheit (°F)"];
                foreach (var u in tempUnits) { pickerFromUnit.Items.Add(u); pickerToUnit.Items.Add(u); }
                pickerFromUnit.SelectedIndex = 0;
                pickerToUnit.SelectedIndex = 1;
                break;
        }

        PerformConversion();
    }

    private void OnInputValuesChanged(object sender, EventArgs e)
    {
        PerformConversion();
    }

    private void PerformConversion()
    {
        if (pickerFromUnit.SelectedIndex == -1 || pickerToUnit.SelectedIndex == -1) return;
        if (!double.TryParse(txtFromValue.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out double input))
        {
            lblToResult.Text = "-";
            return;
        }

        double converted = 0;
        int cat = pickerCategory.SelectedIndex;
        int from = pickerFromUnit.SelectedIndex;
        int to = pickerToUnit.SelectedIndex;

        if (from == to)
        {
            lblToResult.Text = input.ToString(CultureInfo.InvariantCulture);
            return;
        }

        if (cat == 0) // Longueur (base: Mètre)
        {
            double[] toMeter = [1.0, 1000.0, 0.01, 1609.34];
            double meters = input * toMeter[from];
            converted = meters / toMeter[to];
        }
        else if (cat == 1) // Masse (base: Gramme)
        {
            double[] toGram = [1000.0, 1.0, 453.592];
            double grams = input * toGram[from];
            converted = grams / toGram[to];
        }
        else if (cat == 2) // Température
        {
            if (from == 0 && to == 1) converted = (input * 9 / 5) + 32;       // C -> F
            else if (from == 1 && to == 0) converted = (input - 32) * 5 / 9;  // F -> C
        }

        lblToResult.Text = Math.Round(converted, 4).ToString(CultureInfo.InvariantCulture);
    }
}