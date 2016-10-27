using System;
using System.Collections.Generic;

namespace PeopleSearchApp
{
    public class Database
    {
        private List<User> users; //Used when returning all users and searching by a full name

        /// <summary>
        /// Initiate a new Database
        /// </summary>
        public Database()
        {
            // Instantiate users
            users = new List<User>();
        }

        /// <summary>
        /// Method for adding new users to the database
        /// </summary>
        /// <param name="user">User to be added</param>
        /// <returns>Was the action completed successfully?</returns>
        public bool addUser(User user)
        {
            if (user == null) throw new ArgumentNullException();

            if (userchecker(user)) //Make sure the user isn't already in the database
            {
                users.Add(user);
                return true;
            }
            return false;

        }
        
        //Helper method for finding out if the database already contains the user
        private bool userchecker(User user)
        {
            foreach (User u in users)
            {
                if (u.Equals(user))
                    return false;
            }
            return true;
        }

        /// <summary>
        /// A way of getting all the users in the Database
        /// </summary>
        /// <returns>All users</returns>
        public IEnumerable<User> allUsers()
        {
            return new HashSet<User>(users);
        }

        /// <summary>
        /// Search the database for a user based on the users name
        /// </summary>
        /// <param name="name">LastName, FirstName</param>
        /// <returns>User</returns>
        public User searchUser(string name)
        {
            foreach (User u in users)
                if (u.Name == name)
                    return u;
            return null;
        }

        /// <summary>
        /// Searches the database for a user that has a first name or last name that matches the prefix
        /// </summary>
        /// <param name="prefix">partial name</param>
        /// <returns>Users that match the prefix</returns>
        public IEnumerable<User> prefix(string prefix)
        {
            List<User> returnList = new List<User>();
            if (prefix != "")
            {
                foreach (User u in users)
                {
                    //slim down the firstName based on prefix length then see if it matches prefix
                    if (u.FirstName.Substring(0, prefix.Length > u.FirstName.Length ? u.FirstName.Length : prefix.Length).ToUpper() == prefix.ToUpper())
                        returnList.Add(u);

                    //slim down the lastName based on prefix length then see if it matches prefix
                    if (u.LastName.Substring(0, prefix.Length > u.LastName.Length ? u.LastName.Length : prefix.Length).ToUpper() == prefix.ToUpper())
                        returnList.Add(u);
                }
            }
            return returnList;
        }
    }
}
