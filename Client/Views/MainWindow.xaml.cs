using Client.ViewModels;
using System.Windows;

namespace Client.Views;

public partial class MainWindow : Window
{
    public static ViewModelMain ViewModelInstance { get; } = new ViewModelMain();

    public MainWindow()
    {
        InitializeComponent();
        DataContext = ViewModelInstance;
    }

    void Slider_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
    {
        ViewModelInstance.UpdateSizeMainPacket();
    }

    async void Window_Loaded(object sender, RoutedEventArgs e)
    {
        await Task.Run(ClientForSum.Connecting);
    }

    private void ComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
    {
        ViewModelInstance.PackageSent.UpdateFullPackageAsString();
    }
}