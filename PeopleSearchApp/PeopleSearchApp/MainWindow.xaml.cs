using Microsoft.Win32;
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

namespace PeopleSearchApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            addMenuItem.Click += new RoutedEventHandler(Switch2User);
            searchMenuItem.Click += new RoutedEventHandler(Switch2Search);
            userImageBtn.Click += new RoutedEventHandler(importImage);
        }

        public void Switch2Search(Object sender, RoutedEventArgs e)
        {
            addMenuItem.IsEnabled = true;
            searchMenuItem.IsEnabled = false;
            searchGrid.Visibility = Visibility.Visible;
            userGrid.Visibility = Visibility.Hidden;
        }

        public void Switch2User(Object sender, RoutedEventArgs e)
        {
            addMenuItem.IsEnabled = false;
            searchMenuItem.IsEnabled = true;
            searchGrid.Visibility = Visibility.Hidden;
            userGrid.Visibility = Visibility.Visible;

            if (sender.GetType() == typeof(MenuItem))
                saveUser.Content = "Add";
        }

        public void importImage(Object sender, RoutedEventArgs e)
        {
            OpenFileDialog newDialog = new OpenFileDialog();
            newDialog.Filter = "Image files (*.png;*.jpeg)|*.png;*.jpeg|All files(*.*)|*.*";
            if (newDialog.ShowDialog() == true)
            {
                try
                {

                   // userPicture.Source = new Image(newDialog.FileName).Source;// newDialog.OpenFile();
                }catch
                {

                }
            }
        }
    }
}
