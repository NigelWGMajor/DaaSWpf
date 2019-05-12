using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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

        private void Credits_Launch(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Process.Start(@"https://icanhazdadjoke.com/");
        }
    }
}
