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
            addSomeUsers();

            

            InitializeComponent();

            foreach (User u in database.allUsers())
                searchTxtBox.Items.Add(u.Name);

            addMenuItem.Click += new RoutedEventHandler(Switch2User);
            searchMenuItem.Click += new RoutedEventHandler(Switch2Search);
            userImageBtn.Click += new RoutedEventHandler(importImage);
            searchTxtBox.AddHandler(System.Windows.Controls.Primitives.TextBoxBase.TextChangedEvent, new System.Windows.Controls.TextChangedEventHandler(textTyped));
        }

        private void textTyped(object sender, RoutedEventArgs e)
        {
            searchTxtBox.Items.Clear();
            searchTxtBox.IsDropDownOpen = true;
            List<User> list = new List<User>(database.prefix(searchTxtBox.Text));
            if (list.Count == 1)
                searchTxtBox.Text = list.First<User>().Name;
            if (searchTxtBox.Text != null && list.Count!=0 && searchTxtBox.Text != "")
            {
                
                foreach (User u in list)
                    searchTxtBox.Items.Add(u.Name);
            }
            else if (searchTxtBox.Text == "")
                foreach (User u in database.allUsers())
                    searchTxtBox.Items.Add(u.Name);
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
            {
                fNameTxtBox.IsEnabled = true;
                lNameTxtBox.IsEnabled = true;
                saveUser.Content = "Add";
            }
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
            searchedUser.PicFilename = newDialog.FileName;
        }

        private void searchBtn_Click(object sender, RoutedEventArgs e)
        {
            User user = database.searchUser(searchTxtBox.Text);
            if (user == null)
                MessageBox.Show("Unable to find a user by the name " + searchTxtBox.Text);
            else
            {
                Switch2User(sender, e);
                fNameTxtBox.Text = user.FirstName;
                lNameTxtBox.Text = user.LastName;
                addressTxtBox.Text = user.Address;
                ageTxtBox.Text = user.Age.ToString();
                interestTxtBox.Text = user.Interests;
                if (user.PicFilename != null)
                    userPicture.Source = new BitmapImage(new Uri(user.PicFilename));
                searchedUser = user;

                fNameTxtBox.IsEnabled = false;
                lNameTxtBox.IsEnabled = false;
            }
        }

        private void saveUser_Click(object sender, RoutedEventArgs e)
        {
            int age;
            if ((string)saveUser.Content == "Add")
            {
                searchedUser = new User(fNameTxtBox.Text, lNameTxtBox.Text);
                searchedUser.Address = addressTxtBox.Text;
                searchedUser.Interests = interestTxtBox.Text;
                searchedUser.PicFilename = userPicture.Source.ToString();
                if (int.TryParse(ageTxtBox.Text, out age))
                    searchedUser.Age = age;
                database.addUser(searchedUser);
            }
            else
            {
                searchedUser.Address = addressTxtBox.Text;
                searchedUser.Interests = interestTxtBox.Text;
                searchedUser.PicFilename = userPicture.Source.ToString();
                if (int.TryParse(ageTxtBox.Text, out age))
                    searchedUser.Age = age;
            }
        }

        private void addSomeUsers()
        {
            User u1 = new User("John", "Doe");
            User u2 = new User("Jane", "Doe");
            User u3 = new User("Ricky", "Bobby");
            User u4 = new User("George", "Washington");
            database.addUser(u1);
            database.addUser(u2);
            database.addUser(u3);
            database.addUser(u4);
        }
    }
}
