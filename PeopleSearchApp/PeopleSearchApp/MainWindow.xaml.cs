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

            InitializeComponent();

            addMenuItem.Click += Switch2User; // adds an event handler to the File -> Add Menu Item
            searchMenuItem.Click += Switch2Search; // adds an event handler to the File -> Search Menu Item
            userImageBtn.Click += importImage; // adds an event handler for the button that adds a picture to a user
            closeMenuItem.Click += closeWindow; // adds an event handler to the File -> Close Menu Item
            addUsersMenuItem.Click += AddUsersMenuItem_Click; // adds an event handler to the Application -> add default users Menu Item
            aboutMenuItem.Click += AboutMenuItem_Click; // adds an event handler to the Application -> help Menu Item

            // adds an event handler for when text changes in the Combo Box
            searchTxtBox.AddHandler(System.Windows.Controls.Primitives.TextBoxBase.TextChangedEvent, new System.Windows.Controls.TextChangedEventHandler(textTyped));
            
        }

        // Event Handler for when text is typed in the Combo Box
        private void textTyped(object sender, RoutedEventArgs e)
        {
            dropDownListBox.Items.Clear(); //removes all Items in the Combo Box
            if (searchTxtBox.Text != null && searchTxtBox.Text != "")
            {
                dropDownListBox.Visibility = Visibility.Visible;
                // a container to hold all elements that will be added to the ComboBox
                List<User> list = new List<User>(database.prefix(searchTxtBox.Text));
                if (list.Count > 0)
                    foreach (User u in list) //Loop to add Users to the ListBox
                    {
                        ListBoxItem item = new ListBoxItem();
                        item.Name = u.LastName + "_" + u.FirstName;
                        item.Content = u.Name + "\t\tAge: " + u.Age + "\nAddress: " + u.Address;
                        item.MouseDoubleClick += listboxItemClick; //adds the double click event handler to the ListBoxItem
                        dropDownListBox.Items.Add(item);
                    }
                else
                {
                    ListBoxItem item = new ListBoxItem();
                    item.Content = "-Click to Add-";
                    item.MouseDoubleClick += Switch2User;
                    dropDownListBox.Items.Add(item);
                }

            }
            else
                dropDownListBox.Visibility = Visibility.Hidden;
        }

        // Event Handler for when a ListBoxItem is doubleclicked
        private void listboxItemClick(object sender, MouseButtonEventArgs e)
        {
            ListBoxItem item = (ListBoxItem)(sender);
            searchTxtBox.Text = item.Name.Replace("_", ", ");
            searchBtn_Click(sender, e);
        }

        // switch to the Search Screen and activate the Add menu button
        private void Switch2Search(Object sender, RoutedEventArgs e)
        {
            clearTextBoxes();
            addMenuItem.IsEnabled = true;
            searchMenuItem.IsEnabled = false;
            searchGrid.Visibility = Visibility.Visible;
            userGrid.Visibility = Visibility.Hidden;
        }

        //switch to the User screen and activate the Search menu button
        private void Switch2User(Object sender, RoutedEventArgs e)
        {
            clearTextBoxes();
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
        // Finds a user that has the same name
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

        // Event handler for save/add button
        private void saveUser_Click(object sender, RoutedEventArgs e)
        {
            int age;
            if ((string)saveUser.Content == "Add") // add creates a new user
            {
                searchedUser = new User(fNameTxtBox.Text, lNameTxtBox.Text);
                searchedUser.Address = addressTxtBox.Text;
                searchedUser.Interests = interestTxtBox.Text;
                searchedUser.PicFilename = userPicture.Source.ToString();
                if (int.TryParse(ageTxtBox.Text, out age))
                    searchedUser.Age = age;
                searchedUser.PicFilename = userPicture.Source.ToString();
                database.addUser(searchedUser);
            }
            else //save uses the searched user
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

        // Helper method for clearing everything when switching active screens
        private void clearTextBoxes()
        {
            addressTxtBox.Text = "";
            ageTxtBox.Text = "";
            fNameTxtBox.Text = "";
            interestTxtBox.Text = "";
            lNameTxtBox.Text = "";
            searchTxtBox.Text = "";
            dropDownListBox.Items.Clear();
            saveUser.Content = "Save";
            userPicture.Source = new BitmapImage(new Uri("pack://application:,,,/PeopleSearchApp;component/Resources/default.png"));
        }

        // Event handler for CloseMenuItem
        private void closeWindow(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        // Event handler for HelpMenuItem
        private void AboutMenuItem_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Adding a new User: click File -> Add \n\n"+
                            "Searching for a User: Type in the first or last name of the User then double click on the correct user \n\n"+
                            "Need more help? seek the README.txt File");
        }

        // Event handler for addDefaultUsers which just adds some users
        private void AddUsersMenuItem_Click(object sender, RoutedEventArgs e)
        {
            addSomeUsers();
            addManyUsers();
        }

        // Adds 4 default users
        private void addSomeUsers()
        {
            User u1 = new User("John", "Doe");
            u1.Address = "Unkown";
            u1.Age = 0;
            u1.Interests = "Being Mysterious";
            database.addUser(u1);

            User u2 = new User("Jane", "Doe");
            u2.Address = "Unkown";
            u2.Age = 0;
            u2.Interests = "Being Mysterious";
            database.addUser(u2);

            User u3 = new User("Kimble", "Rod");
            u3.Age = 19;
            u3.Address = "2020 State St";
            u3.Interests = "Partying, Stuntman";
            database.addUser(u3);

            User u4 = new User("Ron", "Burgundy");
            u4.Address = "123 Main St, San Diego";
            u4.Age = 42;
            u4.Interests = "Being lead Anchorman for Channel 4";
            database.addUser(u4);

        }

        // Adds Alot of users, Used for testing
        private void addManyUsers()
        {
            int numberOfUsers = 10000;
            for (int i = 0; i < numberOfUsers; i++)
            {
                User u = new User(((char)(66 + i % 25)).ToString(), "LastName");
                u.Age = i;
                database.addUser(u);
            }
        }
    }
}
