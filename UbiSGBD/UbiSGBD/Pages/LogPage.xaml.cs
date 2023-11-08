namespace UbiSGBD.Pages;

public partial class LogPage : ContentPage
{
	public LogPage()
	{
		InitializeComponent();

        foreach (var e in new[] { 10, 20, 30, 50, 100, 500 })
        {
            quantidadePicker.Items.Add(e.ToString());
        }

        quantidadePicker.SelectedIndex = 1;
    }
}