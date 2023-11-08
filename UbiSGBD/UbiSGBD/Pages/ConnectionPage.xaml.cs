using System.Data;

namespace UbiSGBD.Pages;

public partial class ConnectionPage : ContentPage
{
	public ConnectionPage()
	{
		InitializeComponent();

        foreach (IsolationLevel dia in Enum.GetValues(typeof(IsolationLevel)))
        {
            isolationLEvelPicker.Items.Add(dia.ToString());
            isolationLEvelBrowerPicker.Items.Add(dia.ToString());
            isolationLEvelLogPicker.Items.Add(dia.ToString());
        }

        isolationLEvelPicker.SelectedIndex = 2;
        isolationLEvelBrowerPicker.SelectedIndex = 2;
        isolationLEvelLogPicker.SelectedIndex = 2;
    }
}