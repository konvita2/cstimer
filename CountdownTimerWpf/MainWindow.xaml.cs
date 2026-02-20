using System;
using System.Globalization;
using System.Windows;
using System.Windows.Threading;

namespace CountdownTimerWpf;

public partial class MainWindow : Window
{
    private readonly DispatcherTimer _timer;
    private TimeSpan _remaining;

    public MainWindow()
    {
        InitializeComponent();

        _timer = new DispatcherTimer
        {
            Interval = TimeSpan.FromSeconds(1)
        };

        _timer.Tick += Timer_Tick;
        UpdateWindowTitle();
    }

    private void StartButton_Click(object sender, RoutedEventArgs e)
    {
        if (!double.TryParse(MinutesTextBox.Text, NumberStyles.Float, CultureInfo.InvariantCulture, out var minutes) || minutes <= 0)
        {
            MessageBox.Show("Please enter a valid number of minutes greater than 0.", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        _remaining = TimeSpan.FromMinutes(minutes);
        StartButton.IsEnabled = false;
        MinutesTextBox.IsEnabled = false;

        UpdateWindowTitle();
        _timer.Start();
    }

    private void Timer_Tick(object? sender, EventArgs e)
    {
        _remaining -= TimeSpan.FromSeconds(1);

        if (_remaining <= TimeSpan.Zero)
        {
            _timer.Stop();
            _remaining = TimeSpan.Zero;
            UpdateWindowTitle();
            BringWindowToForeground();

            MessageBox.Show("Time is up!", "Timer Finished", MessageBoxButton.OK, MessageBoxImage.Information);

            StartButton.IsEnabled = true;
            MinutesTextBox.IsEnabled = true;
            return;
        }

        UpdateWindowTitle();
    }

    private void UpdateWindowTitle()
    {
        Title = _remaining > TimeSpan.Zero
            ? $"Countdown Timer - Remaining: {_remaining:mm\\:ss}"
            : "Countdown Timer";
    }

    private void BringWindowToForeground()
    {
        WindowStartupLocation = WindowStartupLocation.CenterScreen;
        Topmost = true;
        Activate();
        Focus();

        Left = (SystemParameters.WorkArea.Width - Width) / 2 + SystemParameters.WorkArea.Left;
        Top = (SystemParameters.WorkArea.Height - Height) / 2 + SystemParameters.WorkArea.Top;

        Topmost = false;
    }
}
