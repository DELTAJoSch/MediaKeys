using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

// this is suboptimal, but I couldn't be bothered to write this test application in a neater way
// TODO: Refactor
namespace MediaKeysTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        MediaKeys.Control control = new MediaKeys.Control();

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            control.PlayPause();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            control.Skip();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            control.SkipBack();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            control.Stop();
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            int? volume = control.GetCurrentVolume();
            vol.Text = volume.ToString();
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            control.IncreaseVolume();
        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            control.DecreaseVolume();
        }

        private void Button_Click_7(object sender, RoutedEventArgs e)
        {
            int volume = Convert.ToInt32(toSet.Text);
            control.SetVolume(volume);
        }
    }
}
