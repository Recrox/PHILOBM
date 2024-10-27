using System.Windows;
using System.Windows.Controls;

namespace PHILOBM.Controls;

public partial class LabeledInput : UserControl
{
    public LabeledInput()
    {
        InitializeComponent();
    }

    public string Label
    {
        get { return (string)GetValue(LabelProperty); }
        set { SetValue(LabelProperty, value); }
    }

    public static readonly DependencyProperty LabelProperty =
        DependencyProperty.Register("Label", typeof(string), typeof(LabeledInput), new PropertyMetadata(string.Empty, OnLabelChanged));

    private static void OnLabelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is LabeledInput labeledInput)
        {
            labeledInput.LabelControl.Content = e.NewValue.ToString();
        }
    }

    public string Text
    {
        get { return TextBoxControl.Text; }
        set { TextBoxControl.Text = value; }
    }
}
