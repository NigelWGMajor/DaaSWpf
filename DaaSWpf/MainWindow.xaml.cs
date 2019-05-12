using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace DaaSWpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DaasVM _vm;
        // used to simplify passing of animation commands:
        private static FrameworkElement _self;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = _vm = new DaasVM();
            _self = this;
        }

        internal static void FadeIn()
        {
            var x = ((Storyboard)(_self.TryFindResource("FadeIn")));
                x.Begin();
        }

        private void Credits_Launch(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            System.Diagnostics.Process.Start(@"https://icanhazdadjoke.com/");
        }

        private void MainWindow_OnClosing(object sender, CancelEventArgs e)
        {
            _vm.Close();
            _vm = null;
        }
    }
}
