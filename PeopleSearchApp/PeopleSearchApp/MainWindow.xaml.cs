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
        private Database database; // used to store all users
        private User searchedUser; // the searched user

        public MainWindow()
        {
            database = new Database(); 
            addSomeUsers(); //adds some default users to manipulate

            InitializeComponent();

            addMenuItem.Click += new RoutedEventHandler(Switch2User); // adds an event handler to the File -> Add Menu Item
            searchMenuItem.Click += new RoutedEventHandler(Switch2Search); // adds an event handler to the File -> Search Menu Item
            userImageBtn.Click += new RoutedEventHandler(importImage); // adds an event handler for the button that adds a picture to a user
            closeMenuItem.Click += new RoutedEventHandler(closeWindow); // adds an event handler to the File -> Close Menu Item

            // adds an event handler for when text changes in the Combo Box
            searchTxtBox.AddHandler(System.Windows.Controls.Primitives.TextBoxBase.TextChangedEvent, new System.Windows.Controls.TextChangedEventHandler(textTyped));
            //searchTxtBox.IsDropDownOpen = true;
        }

        // Event Handler for when text is typed in the Combo Box
        private void textTyped(object sender, RoutedEventArgs e)
        {
            dropDownListBox.Items.Clear(); //removes all Items in the Combo Box

            if (searchTxtBox.Text != null && searchTxtBox.Text != "")
            {
                dropDownListBox.IsEnabled = true;
                // a container to hold all elements that will be added to the ComboBox
                List<User> list = new List<User>(database.prefix(searchTxtBox.Text));

                //if (list.Count == 1) // if there is only one item in the container
                //{
                //    dropDownListBox.Items.Add(list.First<User>().Name);
                //    searchBtn.Focus();
                //}

                // if there are more one item in the collection then add them to the comboBox
                if (list.Count >= 1)
                {
                    foreach (User u in list)
                    {
                        ListBoxItem item = new ListBoxItem();
                        //item.Name = u.FirstName + "." + u.LastName;
                        item.Content = u.Name;
                        item.MouseDoubleClick += listboxItemClick;
                        dropDownListBox.Items.Add(item);
                    }
                }
            }
            else
                dropDownListBox.IsEnabled = false;
        }

        private void listboxItemClick(object sender, MouseButtonEventArgs e)
        {
            ListBoxItem item = (ListBoxItem)(sender);
            searchTxtBox.Text = item.Content.ToString();
            searchBtn_Click(sender, e);
        }

        // switch to the Search Screen and activate the Add menu button
        private void Switch2Search(Object sender, RoutedEventArgs e)
        {
            addMenuItem.IsEnabled = true;
            searchMenuItem.IsEnabled = false;
            searchGrid.Visibility = Visibility.Visible;
            userGrid.Visibility = Visibility.Hidden;
        }

        //switch to the User screen and activate the Search menu button
        private void Switch2User(Object sender, RoutedEventArgs e)
        {
            addMenuItem.IsEnabled = false;
            searchMenuItem.IsEnabled = true;
            searchGrid.Visibility = Visibility.Hidden;
            userGrid.Visibility = Visibility.Visible;

            if (sender.GetType() == typeof(MenuItem))// if the event sender was the add menu button we are adding instead of editing
            {
                fNameTxtBox.IsEnabled = true;
                lNameTxtBox.IsEnabled = true;
                saveUser.Content = "Add";
            }
        }

        // Event handler for adding a picture to the user profile
        private void importImage(Object sender, RoutedEventArgs e)
        {
            OpenFileDialog newDialog = new OpenFileDialog();
            
            // acceptable file formats
            newDialog.Filter = "Image files (*.bmp, *.gif, *.ico, *.jpg, *.png, *.wdp, *.tiff)|*.bmp; *.gif; *.ico; *.jpg; *.png; *.wdp; *.tiff";
            if (newDialog.ShowDialog() == true)
            {
                try
                {
                    // make sure the picture will work
                    userPicture.Source = new BitmapImage(new Uri(newDialog.FileName));
                }
                catch
                {
                    MessageBox.Show("Invalid filetype");
                }
            }
            
        }

        // Event handler for Search button. 
        // Finds a user that has the
        private void searchBtn_Click(object sender, RoutedEventArgs e)
        {
            dropDownListBox.IsEnabled = false;
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
                searchedUser.PicFilename = userPicture.Source.ToString();
            }
            else
            {
                searchedUser.Address = addressTxtBox.Text;
                searchedUser.Interests = interestTxtBox.Text;
                searchedUser.PicFilename = userPicture.Source.ToString();
                if (int.TryParse(ageTxtBox.Text, out age))
                    searchedUser.Age = age;
                searchedUser.PicFilename = userPicture.Source.ToString();
            }
            clearTextBoxes();
            Switch2Search(sender,e);
        }

        private void clearTextBoxes()
        {
            addressTxtBox.Text = "";
            ageTxtBox.Text = "";
            fNameTxtBox.Text = "";
            interestTxtBox.Text = "";
            lNameTxtBox.Text = "";
            searchTxtBox.Text = "";
            //searchTxtBox.IsDropDownOpen = false;
            saveUser.Content = "Save";
            userPicture.Source = new BitmapImage(new Uri("pack://application:,,,/PeopleSearchApp;component/Resources/default.png"));
        }

        private void closeWindow(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void addSomeUsers()
        {
            User u1 = new User("John", "Doe");
            u1.Address = "123 State St";
            User u2 = new User("Jane", "Doe");
            u2.Address = "321 Main St";
            User u3 = new User("Ricky", "Bobby");
            User u4 = new User("George", "Washington");
            database.addUser(u1);
            database.addUser(u2);
            database.addUser(u3);
            database.addUser(u4);
        }


    }
}
