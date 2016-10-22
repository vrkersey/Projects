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
        private Database database;
        private User searchedUser;

        public MainWindow()
        {
            database = new Database();

            InitializeComponent();

            addMenuItem.Click += new RoutedEventHandler(Switch2User);
            searchMenuItem.Click += new RoutedEventHandler(Switch2Search);
            userImageBtn.Click += new RoutedEventHandler(importImage);
            
        }

        private void Switch2Search(Object sender, RoutedEventArgs e)
        {
            addMenuItem.IsEnabled = true;
            searchMenuItem.IsEnabled = false;
            searchGrid.Visibility = Visibility.Visible;
            userGrid.Visibility = Visibility.Hidden;
        }

        private void Switch2User(Object sender, RoutedEventArgs e)
        {
            addMenuItem.IsEnabled = false;
            searchMenuItem.IsEnabled = true;
            searchGrid.Visibility = Visibility.Hidden;
            userGrid.Visibility = Visibility.Visible;

            if (sender.GetType() == typeof(MenuItem))
                saveUser.Content = "Add";
        }

        private void importImage(Object sender, RoutedEventArgs e)
        {
            OpenFileDialog newDialog = new OpenFileDialog();
            newDialog.Filter = "Image files (*.bmp, *.gif, *.ico, *.jpg, *.png, *.wdp, *.tiff)|*.bmp; *.gif; *.ico; *.jpg; *.png; *.wdp; *.tiff|All files(*.*)|*.*";
            if (newDialog.ShowDialog() == true)
            {
                try
                {
                    userPicture.Source = new BitmapImage(new Uri(newDialog.FileName));
                }
                catch
                {
                    MessageBox.Show("Invalid filetype");
                }
            }
        }

        private void searchBtn_Click(object sender, RoutedEventArgs e)
        {
            Switch2User(sender, e);
            User user = database.searchUser(searchTxtBox.Text);
            fNameTxtBox.Text = user.FirstName;
            lNameTxtBox.Text = user.LastName;
            addressTxtBox.Text = user.Address;
            ageTxtBox.Text = user.Age.ToString();
            interestTxtBox.Text = user.Interests;
            searchedUser = user;
        }

        private void saveUser_Click(object sender, RoutedEventArgs e)
        {
            int age;
            if ((string)saveUser.Content == "Add")
            {
                User user = new User(fNameTxtBox.Text, lNameTxtBox.Text);
                user.Address = addressTxtBox.Text;
                user.Interests = interestTxtBox.Text;
                if (int.TryParse(ageTxtBox.Text, out age))
                    user.Age = age;
                database.addUser(user);
            }
            else
            {
                searchedUser.Address = addressTxtBox.Text;
                searchedUser.Interests = interestTxtBox.Text;
                if (int.TryParse(ageTxtBox.Text, out age))
                    searchedUser.Age = age;
            }
        }
    }
}
