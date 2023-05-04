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

namespace MartApp
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtnSelVege_Click(object sender, RoutedEventArgs e)
        {
            CategoryPage.Source = new Uri("VegePage.xaml", UriKind.Relative);
        }

        private void BtnSelMeat_Click(object sender, RoutedEventArgs e)
        {
            CategoryPage.Source = new Uri("MeatPage.xaml", UriKind.Relative);
        }

        private void BtnSelSeafood_Click(object sender, RoutedEventArgs e)
        {
            CategoryPage.Source = new Uri("SeafoodPage.xaml", UriKind.Relative);

        }

        private void BtnSelSnack_Click(object sender, RoutedEventArgs e)
        {
            CategoryPage.Source = new Uri("SnackPage.xaml", UriKind.Relative);

        }

        private void BtnSelDrink_Click(object sender, RoutedEventArgs e)
        {
            CategoryPage.Source = new Uri("DrinkPage.xaml", UriKind.Relative);
        }
    }
}
